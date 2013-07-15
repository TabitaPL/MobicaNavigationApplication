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

namespace Mobica
{
    public partial class OptionsPage : PhoneApplicationPage
    {
        private StorageSetting _sets = null;
        public OptionsPage()
        {
            InitializeComponent();
            _sets = new StorageSetting();
            SetSavedValue();
        }

        public void SetSavedValue()
        {
            if (_sets.GetValue("RouteMode") == "Drive mode")
            {
                DrivingRadioButton.IsChecked = true;
                WalkingRadioButton.IsChecked = false;
            }
            else if (_sets.GetValue("RouteMode") == "Walking mode")
            {
                DrivingRadioButton.IsChecked = false;
                WalkingRadioButton.IsChecked = true;
            }
            else
            {
                MessageBox.Show("Error occured while reading from storage memory");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WalkingRadioButton.IsChecked == true)
                _sets.UpdateValue("RouteMode", "Walking mode");
            else
                _sets.UpdateValue("RouteMode", "Drive mode");


            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}