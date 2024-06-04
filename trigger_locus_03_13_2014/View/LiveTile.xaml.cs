using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;

namespace trigger_locus_03_13_2014.View
{
    public partial class LiveTile : PhoneApplicationPage
    {
        public LiveTile()
        {
            InitializeComponent();
        }

        private void PinToStart_Click(object sender, RoutedEventArgs e)
        {
            RadIconicTileData iconicTileData = new RadIconicTileData();
            iconicTileData.Count = 5;
            iconicTileData.Title = "my app";
            //iconicTileData.IconVisualElement = this.VisualElement;          
            Uri uri = new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute);
            LiveTileHelper.CreateOrUpdateTile(iconicTileData, uri, true);
        }
    }
}