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
    public partial class WideTileBack : UserControl
    {
        public WideTileBack()
        {
            InitializeComponent();
        }

        public void SetProperties(ImageSource imageSource, string text)
        {
            this.backImage.Source = imageSource;
            this.backText.Text = text;
        }
    }
}
