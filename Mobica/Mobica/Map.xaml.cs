using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Microsoft.Phone.Tasks;
using Mobica.GeocodeService;
using Mobica.RouteService;
using Microsoft.Phone.Shell;

namespace Mobica
{
    public partial class Map : PhoneApplicationPage
    {
        private ProgressIndicator progressIndicator;
        readonly
        static private string bingMapKey = "Anrr8rj5XPCnmotc5oPRXtih0FDPTMUVIA2We2x6kRyefVNAkSlVCR1C1LL0ZPLn";
        internal GeocodeService.GeocodeResult startPoint = null;
        internal GeocodeService.GeocodeResult endPoint = null;
        internal Address endAddress = null;
        private StorageSetting _sets = new StorageSetting();
        BingMapHelper bmh = new BingMapHelper(bingMapKey);


        public Map()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string cityName = "";
            if (NavigationContext.QueryString.TryGetValue("city", out cityName))
            {
                endAddress = MobicaAddresses.FindDetails(cityName);
            }
            else
            {
                MessageBox.Show("Choosen city does not exist in system: " + cityName);
            }

            createProgressIndicator();
            //znajdź współrzędne celu
            bmh.Geocode(endAddress.GetAddressAsString(), EndingPointReceived, 1);
        }

        private void createProgressIndicator()
        {
            if (null == progressIndicator )
                progressIndicator = new ProgressIndicator()
                {
                    IsVisible = true,
                    IsIndeterminate = true,
                    Text = "Calculating route ..."
                };

            SystemTray.SetProgressIndicator(this, progressIndicator);
        }

        private void StartingPointReceived(Mobica.GeocodeService.GeocodeResult coordinates)
        {
            startPoint = coordinates;
            //MessageBox.Show("I've got it!" + startPoint.Locations[0].Latitude + " " + startPoint.Locations[0].Longitude);
        }

        private void EndingPointReceived(Mobica.GeocodeService.GeocodeResult coordinates)
        {
            try
            {
                //MessageBox.Show("Current location:" + curentLocation.Latitude + " " + curentLocation.Longitude);
                endPoint = coordinates;
                System.Device.Location.GeoCoordinate curentLocation = GPS.currentCoordinate;
                GeocodeResult currentPoint = new GeocodeResult();
                {
                    currentPoint.Locations = new System.Collections.ObjectModel.ObservableCollection<Mobica.GeocodeService.GeocodeLocation>();
                    currentPoint.Locations.Add(new GeocodeService.GeocodeLocation());
                    currentPoint.Locations[0].Latitude = curentLocation.Latitude;
                    currentPoint.Locations[0].Longitude = curentLocation.Longitude;
                    currentPoint.DisplayName = "You are here";
                }
                //MessageBox.Show("I've got it!" + endPoint.Locations[0].Latitude + " " + endPoint.Locations[0].Longitude);
                TravelMode tm = TravelMode.Driving;
                if (_sets.GetValue("RouteMode") == "Walking mode")
                    tm = TravelMode.Walking;
                bmh.CalculateRoute(map1, new GeocodeResult[] { currentPoint, endPoint }, RutedMapReceived, tm);
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show("Position in GPS does not exist: " + e.Message);
            }
        }

        private void RutedMapReceived(Microsoft.Phone.Controls.Maps.Map routedMap)
        {
            try
            {
                if (null == routedMap)
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                map1 = routedMap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception occured: " + ex.Message);
            }
            finally
            {
                if (null != progressIndicator)
                    progressIndicator.IsVisible = false;
            }
        }

        private void ReverseGeocodingReceived(String address)
        {
            //MessageBox.Show("Received address: " + address);
        }

      
    }
}