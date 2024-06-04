using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using trigger_locus_03_13_2014.Resources;
using trigger_locus_03_13_2014.ViewModel;
using trigger_locus_03_13_2014.Model;
using Syncfusion.WP.Controls.Notification;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media.Imaging;
using System.Device.Location;
using trigger_locus_03_13_2014.Sources;
using System.Text;

namespace trigger_locus_03_13_2014
{
    public partial class MainPage : PhoneApplicationPage
    {
        TaskHelper taskHelper = App.taskViewModel;
        dateHeader DateHeader;
        bool isEditMode = false;
        double _x = 0;
        double _x2 = 0;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            createNormalAppbar();
            refreshObjects();
        }

        private void dateHeader_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Calendar.xaml", UriKind.Relative));
        }

        private void resizeTile(object sender, System.Windows.Input.GestureEventArgs e)
        {
            taskHelper.resizeTile((int)((MenuItem)sender).Tag);
        }


        private void task_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (isEditMode == false)
            {
                NavigationService.Navigate(new Uri("/View/EditForm.xaml?ID=" + ((SfHubTile)sender).Tag.ToString(), UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            refreshObjects();

        }


        void inToast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            //throw new NotImplementedException();
        }

        protected override void OnRemovedFromJournal(JournalEntryRemovedEventArgs e)
        {
            //base.OnRemovedFromJournal(e);
            //App.geolocator.PositionChanged -= geolocator_PositionChanged;
            //App.geolocator = null;
        }

        private void refreshObjects()
        {
            taskHelper.loadTask();
            DateHeader = new dateHeader(App.dateSelected);
            MainHeader.DataContext = DateHeader;
            this.DataContext = null;
            taskHelper.loadTask(App.dateSelected);
            this.DataContext = taskHelper;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            MessageBoxResult exitConfirmation = MessageBox.Show("You will not be notified of your location-based tasks if you close this app.", "You will not be notified!", MessageBoxButton.OKCancel);
            if (exitConfirmation == MessageBoxResult.OK)
            {
                Application.Current.Terminate();
            }
            else { e.Cancel = true; }
        }

        #region Application Bar
        private void createNormalAppbar()
        {
            mainListbox.IsCheckModeActive = false;
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton addNewAppBar = new ApplicationBarIconButton(new Uri("/Assets/Images/AppBar/add.png", UriKind.Relative));
            addNewAppBar.Text = "add new";
            ApplicationBarMenuItem editMode = new ApplicationBarMenuItem("Edit Mode");
            ApplicationBarMenuItem settings = new ApplicationBarMenuItem("Settings");

            ApplicationBar.Buttons.Add(addNewAppBar);
            ApplicationBar.Opacity = 0.5;
            ApplicationBar.MenuItems.Add(editMode);
            ApplicationBar.MenuItems.Add(settings);
            ApplicationBar.IsMenuEnabled = true;
            isEditMode = false;
            addNewAppBar.Click += addNewAppBar_Click;
            editMode.Click += editMode_Click;
            settings.Click += settings_Click;
        }

        void settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/LiveTile.xaml", UriKind.Relative));
        }

        void editMode_Click(object sender, EventArgs e)
        {
            createEditAppbar();
        }

        void normalMode_Click(object sender, EventArgs e)
        {
            createNormalAppbar();
        }

        void addNewAppBar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/InputForm.xaml", UriKind.Relative));
        }

        void deleteAppBar_Click(object sender, EventArgs e)
        {
            foreach (var item in mainListbox.CheckedItems)
            {
                taskHelper.deleteTask((TblTask)item);
            }
            refreshObjects();
            createNormalAppbar();
        }

        private void createEditAppbar()
        {
            mainListbox.IsCheckModeActive = true;
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton deleteAppBar = new ApplicationBarIconButton(new Uri("/Assets/Images/AppBar/delete.png", UriKind.Relative));
            deleteAppBar.Text = "delete";

            isEditMode = true;
            ApplicationBarMenuItem normalMode = new ApplicationBarMenuItem("Normal Mode");
            ApplicationBar.Buttons.Add(deleteAppBar);
            ApplicationBar.Opacity = 0.5;
            ApplicationBar.MenuItems.Add(normalMode);
            ApplicationBar.IsMenuEnabled = true;

            deleteAppBar.Click += deleteAppBar_Click;
            normalMode.Click += normalMode_Click;
        }
        #endregion

        #region Flick Events
        private void PhoneApplicationPage_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            _x = e.ManipulationOrigin.X;
        }

        private void PhoneApplicationPage_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {

        }

        private void PhoneApplicationPage_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            _x2 = e.ManipulationOrigin.X;
            if (_x > _x2 && _x - _x2 > 100)
            {
                App.dateSelected = App.dateSelected.AddDays(1);
                DateHeader = new dateHeader(App.dateSelected);
                MainHeader.DataContext = DateHeader;
                this.DataContext = null;
                taskHelper.loadTask(App.dateSelected);
                this.DataContext = taskHelper;
            }
            else if (_x < _x2 && _x2 - _x > 100)
            {
                App.dateSelected = App.dateSelected.AddDays(-1);
                DateHeader = new dateHeader(App.dateSelected);
                MainHeader.DataContext = DateHeader;
                this.DataContext = null;
                taskHelper.loadTask(App.dateSelected);
                this.DataContext = taskHelper;
            }
        }
        #endregion

        private void readTask(object sender, System.Windows.Input.GestureEventArgs e)
        {
            locationServices loc = new locationServices();
            TblTask taskSelected = taskHelper.allTasks.Where(t => t.Task_id == (int)((MenuItem)sender).Tag).First();
            StringBuilder sb = new StringBuilder();
            sb.Append("Reading trigger,");
            sb.Append(taskSelected.Title + ", ");
            sb.Append(taskSelected.Description + ", ");
            sb.Append("located at :");

            var locQuery = from l in App.locationViewModel.allLocation
                           group l by new { id = l.Address, val = l.Task_id } into g
                           select new { id = g.Key.id, val = g.Key.val, count = g.Count() };

            foreach (var item in locQuery)
            {
                if (item.id == locQuery.Last().id)
                {
                    sb.AppendLine(" and " + item.id);
                }
                else { sb.AppendLine(item.id); }
            }

            sb.AppendLine(" with:");

            App.contactViewModel.loadContactE();
            var query = from c in App.contactViewModel.allContactE
                        group c by new { id = c.Name, val = c.Task_id } into g
                        select new { id = g.Key.id, val = g.Key.val, count = g.Count() };

            foreach (var item in query)
            {
                if (item.id == query.Last().id)
                {
                    sb.AppendLine(" and " + item.id);
                }
                else { sb.AppendLine(item.id); }
            }

            loc.speak(sb.ToString());
        }

    }

    public class dateHeader
    {
        public string dd { get; set; }
        public string MMM { get; set; }
        public string ddd { get; set; }
        public DateTime selectedDate { get; set; }
        public dateHeader() { }
        public dateHeader(DateTime date)
        {
            selectedDate = date;
            dd = date.ToString("dd");
            MMM = date.ToString("ddd");
            ddd = date.ToString("MMM");
        }
    }
}