using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using trigger_locus_03_13_2014.ViewModel;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Services;
using trigger_locus_03_13_2014.Model;
using Coding4Fun.Toolkit.Controls;

namespace trigger_locus_03_13_2014.View
{
    public partial class Location : PhoneApplicationPage
    {
        LocationHelper locHelper = new LocationHelper(App.connectionString);
        string ID;
        string Tas_loc_id;
        Double zoomLevel = 19;

        public Location()
        {
            InitializeComponent();
            this.Loaded += Location_Loaded;
        }

        void Location_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.Default };
            }
            //Returns when Location is disabled in user phone
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Location setting is not enabled in your phone. Please enable to continue using this feature.");
            }

            //Load all locations to the collections
            locHelper.loadLocation();

            //Filter locations based on the task_id, call locList for the filtered collections
            locHelper.loadLocation(Convert.ToInt32(ID));
            refreshMapObjects();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            App.locationViewModel.loadLocation();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("ID", out ID);
            NavigationContext.QueryString.TryGetValue("Focus", out Tas_loc_id);
        }

        //Get user's current position
        private async void getCurrentPosition()
        {
            Geoposition geoposition = await App.geolocator.GetGeopositionAsync();
            Geocoordinate geocoordinate = geoposition.Coordinate;

            mainMap.Center = geocoordinate.ToGeoCoordinate();
            mainMap.ZoomLevel = zoomLevel;

            MapExtensions.GetChildren(mainMap)
            .OfType<UserLocationMarker>().First()
            .GeoCoordinate = geocoordinate.ToGeoCoordinate();

            MapExtensions.GetChildren(mainMap)
            .OfType<UserLocationMarker>().First()
            .Visibility = System.Windows.Visibility.Visible;
        }

        private void refreshMapObjects()
        {
            if (locHelper.locList.Count > 0)
            {
                MapExtensions.GetChildren(mainMap)
                .OfType<MapItemsControl>().First()
                .ItemsSource = locHelper.locList;

                mainMap.Center = locHelper.locList.Where(l => l.Tas_loc_id == Convert.ToInt32(Tas_loc_id)).First().coordinate;
                mainMap.ZoomLevel = zoomLevel;
            }
            else
            {
                getCurrentPosition();
            }

        }

        #region Appbar
        private void appBarMyPositionButton_Click(object sender, EventArgs e)
        {
            getCurrentPosition();
        }

        private void appBarCancelButton_Click(object sender, EventArgs e)
        {
            locHelper.resetLocation(Convert.ToInt32(ID));
            refreshMapObjects();
        }
        #endregion

        #region Map

        private void zoomChanged(object sender, Microsoft.Phone.Maps.Controls.MapZoomLevelChangedEventArgs e)
        {
            zoomLevel = mainMap.ZoomLevel;
        }

        private void mainMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point mapPoint = e.GetPosition(mainMap);
            ReverseGeocodeQuery revGeoQuery = new ReverseGeocodeQuery();
            revGeoQuery.GeoCoordinate = mainMap.ConvertViewportPointToGeoCoordinate(mapPoint);

            revGeoQuery.QueryCompleted += revGeoQuery_QueryCompleted;
            revGeoQuery.QueryAsync();
        }

        private void revGeoQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    MapAddress geoAddress = e.Result[0].Information.Address;
                    RelTaskLocation newRelLoc = new RelTaskLocation()
                    {
                        Address = geoAddress.Street ?? "Unknown",
                        Latitude = e.Result[0].GeoCoordinate.Latitude,
                        Longitude = e.Result[0].GeoCoordinate.Longitude,
                        Altitude = e.Result[0].GeoCoordinate.Altitude,
                        Task_id = Convert.ToInt32(ID)
                    };
                    locHelper.addLocation(newRelLoc);
                    locHelper.locList.Add(newRelLoc);
                    Tas_loc_id = newRelLoc.Tas_loc_id.ToString();
                    refreshMapObjects();
                }
            }
        }

        private void Remove_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            locHelper.deleteLocation((int)(((MenuItem)sender).Tag));
        }
        #endregion

        private void Rename_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            InputPrompt iPrompt = new InputPrompt();
            iPrompt.Title = "Rename Location";
            iPrompt.Tag = (int)(((MenuItem)sender).Tag);
            iPrompt.Message = "Input New Name for this Location";
            iPrompt.Completed += iPrompt_Completed;
            iPrompt.Show();
        }

        void iPrompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            locHelper.renameLocation((int)((InputPrompt)sender).Tag, e.Result);
        }

    }
}