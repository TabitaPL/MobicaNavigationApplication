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

namespace Mobica
{
    static public class Helper
    {
        public delegate void GeocodeResultReceived(Mobica.GeocodeService.GeocodeResult coordinates);
        public delegate void ReverseGeocodeResultReceived(String address);
        public delegate void MapResultReceived(Microsoft.Phone.Controls.Maps.Map mapWithRoute);
    }
}