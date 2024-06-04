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
using trigger_locus_03_13_2014.Model;
using trigger_locus_03_13_2014.Sources;

namespace trigger_locus_03_13_2014.View
{
    public partial class InputForm : PhoneApplicationPage
    {
        TaskHelper taskHelper = new TaskHelper(App.connectionString);
        LocationHelper locHelper = new LocationHelper(App.connectionString);
        ContactHelper conHelper = App.contactViewModel;
        public InputForm()
        {
            InitializeComponent();
            this.Loaded += InputForm_Loaded;
        }

        void InputForm_Loaded(object sender, RoutedEventArgs e)
        {
            pkrDate.MinValue = DateTime.Now;
            pkrDate.DisplayValueFormat = "MMMM dd, yyyy";
        }


        private void appBarCancelButton_Click(object sender, EventArgs e)
        {
            //Delete task on cancel 
            taskHelper.deleteTask(taskHelper.selectedTask);
        }

        #region Navigation
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            taskHelper.saveChanges();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            conHelper.conList.Clear();
            if (e.NavigationMode == NavigationMode.New)
            {
                pkrDate.Value = App.dateSelected;
                TblTask newTask = new TblTask()
                {
                    Date_task = pkrDate.Value ?? DateTime.Now,
                    TileWidth = 150,
                    isActive = true
                };
                taskHelper.addTask(newTask);

                taskHelper.loadTask(newTask.Task_id);
                this.DataContext = taskHelper.selectedTask;
            }

            if (e.NavigationMode == NavigationMode.Back)
            {
                taskHelper.loadTask(taskHelper.selectedTask.Task_id);


                locHelper = new LocationHelper(App.connectionString);

                locHelper.loadLocation();
                locHelper.loadLocation(taskHelper.selectedTask.Task_id);
                locationList.DataContext = null;
                locationList.DataContext = locHelper.locList;

                //Load Contacts
                try { contactList.Items.Clear(); }
                catch (Exception) { }
                conHelper.loadContacts(taskHelper.selectedTask.Task_id);
                contactList.DataContext = conHelper.conList;
            }
        }
        #endregion

        #region Attendee Panel Events
        private void attendee_panel_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Contact.xaml?ID=" + taskHelper.selectedTask.Task_id + "", UriKind.Relative));
        }

        private void conList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Contact.xaml?ID=" + taskHelper.selectedTask.Task_id + "", UriKind.Relative));
        }

        private void Remove_Contact_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
        #endregion

        #region Location Panel Events
        private void mapPanel_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Location.xaml?ID=" + taskHelper.selectedTask.Task_id, UriKind.Relative));
        }

        private void Remove_Location_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            locHelper.deleteLocation((int)(((MenuItem)sender).Tag));
        }

        private void locList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Location.xaml?ID=" + taskHelper.selectedTask.Task_id + "&Focus=" + ((int)((TextBlock)sender).Tag) + "", UriKind.Relative));
        }

        #endregion

        #region Force Textbox Databind

        private void TBX_description_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBX_description.UpdateBinding();
        }

        private void TBX_title_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBX_title.UpdateBinding();
        }

        #endregion

        private void RadDatePicker_ValueChanged(object sender, Telerik.Windows.Controls.ValueChangedEventArgs<object> args)
        {
            try
            {
                App.dateSelected = pkrDate.Value ?? DateTime.Now;
            }
            catch (NullReferenceException) { }
        }


    }
}