using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using trigger_locus_03_13_2014.Model;
using Windows.Phone.Speech.Synthesis;

namespace trigger_locus_03_13_2014.Sources
{

    public class UserAppointments : IAppointment
    {
        /// <summary> 
        /// Gets the details of the appointment. 
        /// </summary> 
        public string Details
        {
            get;
            set;
        }

        /// <summary> 
        /// Gets the end date and time of the appointment. 
        /// </summary> 
        public DateTime EndDate
        {
            get;
            set;
        }

        /// <summary> 
        /// Gets the location of the appointment. 
        /// </summary> 
        public string Location
        {
            get;
            set;
        }

        /// <summary> 
        /// Gets the start date and time of the appointment. 
        /// </summary> 
        public DateTime StartDate
        {
            get;
            set;
        }

        /// <summary> 
        /// Gets the subject of the appointment. 
        /// </summary> 
        public string Subject
        {
            get;
            set;
        }

        /// <summary> 
        /// Gets a string representation of the appointment 
        /// </summary> 
        /// <returns>A string representation of the appointment</returns> 
        public override string ToString()
        {
            return this.Subject;
        }
    }

    public static class Utilities
    {
        public static void UpdateBinding(this TextBox textBox)
        {
            BindingExpression bindingExpression =
                    textBox.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }
    }

    public static class MapPushPinDependency
    {
        public static readonly DependencyProperty ItemsSourceProperty =
                DependencyProperty.RegisterAttached(
                 "ItemsSource", typeof(IEnumerable), typeof(MapPushPinDependency),
                 new PropertyMetadata(OnPushPinPropertyChanged));

        private static void OnPushPinPropertyChanged(DependencyObject d,
                DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;
            var pushpin = MapExtensions.GetChildren((Map)uie).OfType<MapItemsControl>().FirstOrDefault();
            pushpin.ItemsSource = (IEnumerable)e.NewValue;
        }

        #region Getters and Setters

        public static IEnumerable GetItemsSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(ItemsSourceProperty, value);
        }

        #endregion
    }

    public class locationServices
    {
        public void startTracker()
        {

            if (App.geolocator == null)
            {
                App.geolocator = new Windows.Devices.Geolocation.Geolocator();
                App.geolocator.DesiredAccuracy = Windows.Devices.Geolocation.PositionAccuracy.High;
                App.geolocator.MovementThreshold = 1;
                App.geolocator.PositionChanged += geolocator_PositionChanged;
            }
        }

        void geolocator_PositionChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            if (!App.RunningInBackground)
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    RelTaskLocation queryLoc = isWithinArea(new GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude));

                    if (queryLoc != null)
                    {
                        TblTask queryTask = App.taskViewModel.allTasks.Where(t => t.Task_id == queryLoc.Task_id).First();
                        //ToastPrompt inToast = new ToastPrompt();
                        ToastPrompt inToast = new ToastPrompt();
                        inToast.Title = queryTask.Title;
                        inToast.Message = "You have arrived at " + queryLoc.Address;
                        inToast.FontSize = 20;
                        inToast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
                        inToast.ImageSource = new BitmapImage(new Uri("/Assets/Images/logo_small_solid.png", UriKind.RelativeOrAbsolute));
                        inToast.Show();
                        speak(inToast.Message);
                        //inToast.Completed += inToast_Completed;
                    }
                });
            }
            else
            {
                //Code for runnning in background
                RelTaskLocation queryLoc = isWithinArea(new GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude));

                if (queryLoc != null)
                {
                    TblTask queryTask = App.taskViewModel.allTasks.Where(t => t.Task_id == queryLoc.Task_id && t.isActive == true).First();
                    Microsoft.Phone.Shell.ShellToast toast = new Microsoft.Phone.Shell.ShellToast();
                    toast.Content = "You have arrived at " + queryLoc.Address;
                    toast.Title = queryTask.Title;
                    toast.NavigationUri = new Uri("/View/EditForm.xaml?ID=" + queryTask.Task_id + "", UriKind.Relative);
                    toast.Show();
                    speak(toast.Content);
                }
            }
        }

        public async void speak(string wordsToSpeak) 
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            await speech.SpeakTextAsync(wordsToSpeak);
        }

        public RelTaskLocation isWithinArea(GeoCoordinate geocoordinate)
        {
            foreach (TblTask task in App.taskViewModel.allTasks.Where(t => t.Date_task.Date == DateTime.Now.Date))
            {
                foreach (RelTaskLocation loc in App.locationViewModel.allLocation.Where(l => l.Task_id == task.Task_id))
                {
                    GeoCoordinate tasLoc = new GeoCoordinate(loc.Latitude, loc.Longitude);
                    if (tasLoc.GetDistanceTo(geocoordinate) > 0.0 && tasLoc.GetDistanceTo(geocoordinate) < App.distanceThreshold)
                    {
                        return loc;
                    }
                }
            }
            return null;
        }
    }
}
