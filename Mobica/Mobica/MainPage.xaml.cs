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
using Microsoft.Phone.Tasks;
using System.Device.Location;
using System.Net.NetworkInformation;

namespace Mobica
{
    //pictures are from www.icondrawer.com -> this link should be placed in about subpage
    public partial class MainPage : PhoneApplicationPage
    {
        public String CurrentlySelectedCity;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (MobicaAddresses.mobicaAddressList.Count < 1)
                AddCityToList();
            AddressList.ItemsSource = MobicaAddresses.mobicaAddressList;
        }

        //po kliknięciu w odpowiednie miasto przekierowuje na mapę i drogę z obecnego punktu do biura Mobica
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ListBoxItem selectedItem = this.AddressList.ItemContainerGenerator.ContainerFromItem(this.AddressList.SelectedItem) as ListBoxItem;
                Address data = (sender as Button).DataContext as Address;
                NavigationService.Navigate(new Uri("/Map.xaml?city=" + data.City, UriKind.RelativeOrAbsolute));
            }
            else
            {
                MessageBox.Show("You must have Internet connection to use this service");
            }

        }

        private void AddCityToList()
        {
            MobicaAddresses.AddAddress(new Address
                        ("Poland", "Szczecin", "Plac Hołdu Pruskiego 9", "70-550", "Mobica Limited Sp. z o.o."));
            MobicaAddresses.AddAddress(new Address
                        ("Poland", "Warszawa", "ul. Wałbrzyska 11", "02-741", "Mobica Limited Sp. z o.o."));
            MobicaAddresses.AddAddress(new Address
                        ("Poland", "Łódź", "ul. Wólczańska 178", "90-530", "Mobica Limited Sp. z o.o."));
            MobicaAddresses.AddAddress(new Address
                        ("Poland", "Bydgoszcz", "ul. Fordońska 353", "85-766", "Mobica Limited Sp. z o.o."));
            MobicaAddresses.AddAddress(new Address
                        ("USA", "San Jose", "2570 N. First Street", "California 95131", "Mobica US Inc."));
            MobicaAddresses.AddAddress(new Address
                        ("UK", "Wilmslow", "Manchester Road", "Wilmslow SK9 1BH", "Crown House"));
        }

        private void About_click(object sender, EventArgs e)
        {
            MessageBox.Show("Application was created for Mobica competition by Agnieszka Panek. All rights reserved. Flag icons are from www.icondrawer.com");
        }

        private void Options_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/OptionsPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }

}