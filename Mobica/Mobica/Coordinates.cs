using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Effects;
using System.ServiceModel;
using System.Collections.Generic;
using Mobica.GeocodeService;
using Mobica.RouteService;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Device.Location;

namespace Mobica
{
    class BingMapHelper
    {
        private String bingMapKey;
        private Dictionary<int, Helper.GeocodeResultReceived> GeocodeResponseSendHere = null;
        private Helper.MapResultReceived MapResponseSendHere = null;
        //private Helper.ReverseGeocodeResultReceived ReverseGeoSendHere = null;
        private Microsoft.Phone.Controls.Maps.Map routedMap = null;
        private TimeSpan timeout = new TimeSpan(0, 0, 30);

        public BingMapHelper(String key)
        {
            bingMapKey = key;
            GeocodeResponseSendHere = new Dictionary<int, Helper.GeocodeResultReceived>();
        }

        /* zamienia adres na współrzędne
         * poprzez wysłanie requestu i oczekiwanie na odpowiedź
         * @args:
         * address - string z adresem który bedzie zmianeiony na wspolrzedne
         * cr - funkcja ktora zostanie wywlana gdy przyjda wspolrzedne
         * counter - ktory event z kolei jest wysylany z requestem
        */
        public void Geocode(String address, Helper.GeocodeResultReceived cr, int counter)
        {
            GeocodeResponseSendHere.Add(counter, new Helper.GeocodeResultReceived(cr));
            GeocodeServiceClient geocodeService = new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");

            GeocodeRequest geocodeRequest = new GeocodeRequest();
            {
                geocodeRequest.Credentials = new Credentials() { ApplicationId = bingMapKey };
                geocodeRequest.Query = address;
                geocodeRequest.Options = new GeocodeOptions()
                {
                    Filters = new System.Collections.ObjectModel.ObservableCollection<FilterBase>(),
                    Count = 1
                };

            };
            geocodeRequest.Options.Filters.Add(new ConfidenceFilter() { MinimumConfidence = Mobica.GeocodeService.Confidence.High });
            geocodeService.GeocodeAsync(geocodeRequest, counter);

            geocodeService.GeocodeCompleted += (sender, e) => GeocodeCompleted(sender, e);
        }

        //otrzymane współrzędne zostaną wysłane na zapisaną wcześniej do słownika funkcję
        private void GeocodeCompleted(object sender, GeocodeService.GeocodeCompletedEventArgs e)
        {
           if (e.Result.Results.Count > 0)
           {
               GeocodeResult coordinatesReceived = e.Result.Results[0];
               //MessageBox.Show("Latitude: " + coordinatesReceived.Locations[0].Latitude + "\nLongitude: " + coordinatesReceived.Locations[0].Longitude);
               if (GeocodeResponseSendHere.ContainsKey(System.Convert.ToInt32(e.UserState)))
               {
                   GeocodeResponseSendHere[System.Convert.ToInt32(e.UserState)](coordinatesReceived);
                   GeocodeResponseSendHere.Remove(System.Convert.ToInt32(e.UserState));
               }
           }
           else
               MessageBox.Show("No location found");
        }

        /*oblicza trasę na podstawie otrzymanych współrzędnych
         * jesli waypointów mniej niż dwa informacja
         * w przeciwnym wypadku zapisz je na mapie
        */
        public void CalculateRoute(Microsoft.Phone.Controls.Maps.Map map, GeocodeService.GeocodeResult[] coordinates, Helper.MapResultReceived croute, TravelMode tm = TravelMode.Driving)
        {
            if (coordinates.Length < 2)
            {
                MessageBox.Show("Too small number od location: you need startPoint and endpoint at least");
                return;
            }
            try
            {
                MapResponseSendHere = new Helper.MapResultReceived(croute);
                routedMap = map;
                RouteService.RouteServiceClient routeService = new RouteService.RouteServiceClient("BasicHttpBinding_IRouteService");
                routeService.Endpoint.Binding.ReceiveTimeout = timeout;
                routeService.Endpoint.Binding.SendTimeout = timeout;
                routeService.Endpoint.Binding.OpenTimeout = timeout;

                RouteRequest routeRequest = new RouteRequest();
                {
                    routeRequest.Credentials = new Microsoft.Phone.Controls.Maps.Credentials() { ApplicationId = bingMapKey };
                    routeRequest.Options = new RouteService.RouteOptions() 
                    { RoutePathType = RouteService.RoutePathType.Points, Mode = tm };
                }
                routeRequest.Waypoints = new System.Collections.ObjectModel.ObservableCollection<RouteService.Waypoint>();
                foreach (GeocodeService.GeocodeResult coord in coordinates)
                {
                    if (null != coord)
                    {
                        if (coordinates[coordinates.Length - 1] != coord) //jesli to ostatni punkt ustaw: MOBICA
                            routeRequest.Waypoints.Add(GeocodeToWaypoint(coord));
                        else
                            routeRequest.Waypoints.Add(GeocodeToWaypoint(coord, "MOBICA"));
                    }
                }
                
                routeService.CalculateRouteCompleted += (sender, e) => CalculateRouteCompleted(sender, e);
                routeService.CalculateRouteAsync(routeRequest, coordinates.Length);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception occurred: " + ex.Message);
 
            }
        }


        private void CalculateRouteCompleted(object sender, RouteService.CalculateRouteCompletedEventArgs e)
        {
            try
            {
                if ((e.Result.ResponseSummary.StatusCode == RouteService.ResponseStatusCode.Success) & (e.Result.Result.Legs.Count != 0))
                {
                    var markers = new List<GeoCoordinate>();
                    Microsoft.Phone.Controls.Maps.MapPolyline routeLine = SetRouteProperties();
                    int i = 1;
                    foreach (Microsoft.Phone.Controls.Maps.Platform.Location p in e.Result.Result.RoutePath.Points)
                    {
                        GeoCoordinate gc = new GeoCoordinate(p.Latitude, p.Longitude);
                        routeLine.Locations.Add(gc); //pin location is the same as start and end points
                        if ((e.Result.Result.RoutePath.Points.Count == i) || (1 == i))
                        {
                            Microsoft.Phone.Controls.Maps.Pushpin pin = new Microsoft.Phone.Controls.Maps.Pushpin();
                            pin.Location = gc;
                            if (1 == i)
                            {
                                pin.Background = new SolidColorBrush(Colors.Red);
                                pin.Content = "You are here";
                            }
                            else
                            {
                                pin.Background = new SolidColorBrush(Colors.Purple);
                                pin.Content = "MOBICA";
                            }

                            routedMap.Children.Add(pin);
                        }
                        markers.Add(gc);
                        i++;
                    }

                    if (null != routedMap)
                    {
                        routedMap.Children.Add(routeLine);
                    }

                    routedMap.ZoomLevel = 19;
                    
                    routedMap.SetView(Microsoft.Phone.Controls.Maps.LocationRect.CreateLocationRect(markers));
                    markers.Clear();
                    MapResponseSendHere(routedMap);
                }
            }
            catch (Exception ex)
            {
                if ("No route was found for the waypoints provided." == ex.Message)
                    MessageBox.Show("You are too far away from your destination, please find another Mobica office.");
                else
                    MessageBox.Show("An exception occured: " + ex.Message);

                MapResponseSendHere(null);
            }
        }

        //desc - pozwala ustawic wlasny opis waypointa
        private Waypoint GeocodeToWaypoint(GeocodeResult result, String desc = "")
        {
            RouteService.Waypoint waypoint = new RouteService.Waypoint();
            try
            {
                if (String.IsNullOrEmpty(desc))
                    waypoint.Description = result.DisplayName;
                else
                    waypoint.Description = desc;
                waypoint.Location = new Microsoft.Phone.Controls.Maps.Platform.Location();
                waypoint.Location.Latitude = result.Locations[0].Latitude;
                waypoint.Location.Longitude = result.Locations[0].Longitude;
            }
            catch
            {
                MessageBox.Show("Waypoint does not exist");
            }
            return waypoint;
        }

        private Microsoft.Phone.Controls.Maps.MapPolyline SetRouteProperties()
        {
            Color routeColor = Colors.Blue;
            SolidColorBrush routeBrush = new SolidColorBrush(routeColor);
            Microsoft.Phone.Controls.Maps.MapPolyline routeLine = new Microsoft.Phone.Controls.Maps.MapPolyline();
            routeLine.Locations = new Microsoft.Phone.Controls.Maps.LocationCollection();
            routeLine.Stroke = routeBrush;
            routeLine.Opacity = 0.65;
            routeLine.StrokeThickness = 5.0;
            return routeLine;
        }

        //na podstawie współrzędnych zwraca adres do funkcji getAddress
        /*public void ReverseGeocodeRequest(double latitude, double longitude, Helper.ReverseGeocodeResultReceived getAddress)
        {
            string Results = "";
            try
            {
                GeocodeService.ReverseGeocodeRequest reverseGeocodeRequest = new ReverseGeocodeRequest();

                // Set the credentials using a valid Bing Maps key
                reverseGeocodeRequest.Credentials = new Credentials() { ApplicationId = bingMapKey };
                reverseGeocodeRequest.Location = new GeocodeService.Location { Latitude = latitude, Longitude = longitude };
                reverseGeocodeRequest.ExecutionOptions = new GeocodeService.ExecutionOptions { SuppressFaults = true };

                GeocodeService.GeocodeServiceClient geocodeService = new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                geocodeService.ReverseGeocodeCompleted += new EventHandler<ReverseGeocodeCompletedEventArgs>(ReverseGeocodeCompleted);
                geocodeService.ReverseGeocodeAsync(reverseGeocodeRequest);
                ReverseGeoSendHere = getAddress;
            }

            catch (Exception ex)
            {
                Results = "An exception occurred: " + ex.Message;

            }

        }

        //http://msdn.microsoft.com/en-us/library/cc879136.aspx
        private void ReverseGeocodeCompleted(object sender, ReverseGeocodeCompletedEventArgs e)
        {
           GeocodeResponse geocodeResponse = e.Result;
           try
           {
               ReverseGeoSendHere(geocodeResponse.Results[0].DisplayName);
           }
           catch
           {
               MessageBox.Show("Function does not exist ");
           }

        }*/
    }

    static class GPS
    {
        //readonly
        public static System.Device.Location.GeoCoordinate currentCoordinate
        {
            get
            {
                //check status  GeoPositionStatus.Disabled
                /*
                 *         case GeoPositionStatus.Disabled:
                            // location is unsupported on this device
                            break;
                            case GeoPositionStatus.NoData:
                            // data unavailable
                            break;
                 */
                GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
                watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
                GeoCoordinate coord = watcher.Position.Location;

                if (coord.IsUnknown != true)
                {
                    Console.WriteLine("Lat: {0}, Long: {1}",
                        coord.Latitude,
                        coord.Longitude);
                }
                else
                {
                    Console.WriteLine("Unknown latitude and longitude.");
                }
                return coord;
            }
        }

    }
}

//http://msdn.microsoft.com/en-us/library/ee681887.aspx