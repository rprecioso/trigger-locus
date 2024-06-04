using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace trigger_locus_03_13_2014.Sources.LiveTile
{
    public partial class WideTileFront : UserControl
    {
        public void SetProperties(ImageSource imageSource, string temperature)
        {
            this.frontImage.Source = imageSource;
            this.temperature.Text = temperature;
        }

        public void SetTemperatureRange(string temperatureRange)
        {
            this.temperatureRange.Text = temperatureRange;
        }

        public void SetCityName(string cityName)
        {
            this.cityName.Text = cityName;
        }

        public void SetScale(string scale)
        {
            this.scale.Text = scale;
        }

        public WideTileFront()
        {
            InitializeComponent();
            this.SetScale("C");
        }
    }
}
