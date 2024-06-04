using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using trigger_locus_03_13_2014.Sources;
using trigger_locus_03_13_2014.ViewModel;
using Telerik.Windows.Controls;
using System.Text;
using trigger_locus_03_13_2014.Model;

namespace trigger_locus_03_13_2014.View
{
    public partial class EditForm : PhoneApplicationPage
    {
        TaskHelper taskHelper = App.taskViewModel;
        ContactHelper conHelper = App.contactViewModel;
        LocationHelper locHelper = new LocationHelper(App.connectionString);
        string ID;

        public EditForm()
        {
            InitializeComponent();
            this.Loaded += EditForm_Loaded;
        }

        void EditForm_Loaded(object sender, RoutedEventArgs e)
        {
            taskHelper.loadTask();
            taskHelper.loadTask(Convert.ToInt32(ID));
            this.DataContext = taskHelper.selectedTask;
            pkrDate.DisplayValueFormat = "MMMM dd, yyyy";
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
            NavigationContext.QueryString.TryGetValue("ID", out ID);

            //Load locations
            locHelper = new LocationHelper(App.connectionString);
   
            locHelper.loadLocation();
            locHelper.loadLocation(Convert.ToInt32(ID));
            locationList.DataContext = null;
            locationList.DataContext = locHelper.locList;

            //Load Contacts

            conHelper.conList.Clear();


            conHelper.loadContacts(Convert.ToInt32(ID));
            contactList.DataContext = conHelper.conList;
        }
        #endregion

        private void appBarDeleteButton_Click(object sender, EventArgs e)
        {
            //locHelper.resetLocation(taskHelper.selectedTask.Task_id);
            taskHelper.deleteTask(taskHelper.selectedTask);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void appBarSetDoneButton_Click(object sender, EventArgs e)
        {
            taskHelper.setAsDone(Convert.ToInt32(ID), false);
        }


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
            conHelper.deleteContact(Convert.ToInt32(((MenuItem)sender).Tag));
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

        private void appBarPinButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (RelTaskLocation item in locHelper.locList.Where(l => l.Task_id == taskHelper.selectedTask.Task_id))
            {
                sb.AppendLine(item.Address);
            }
            RadFlipTileData flipData = new RadFlipTileData()
            {
                //VisualElement = this.LayoutRoot,
                //WideBackVisualElement = this.LayoutRoot,
                
                WideBackContent = sb.ToString(),
                Title = taskHelper.selectedTask.Title,
                BackTitle = taskHelper.selectedTask.Date_task.ToString("MMMM dd, yyyy"),
                BackContent = taskHelper.selectedTask.Description,
            };

            //RadExtendedTileData extendedData = new RadExtendedTileData()
            //{
            //    VisualElement = this.LayoutRoot,
            //    Title = taskHelper.selectedTask.Title,
            //    BackContent = taskHelper.selectedTask.Description
                
            //};
            //

            //this will create a tile looking exactly as your page if it is placed inside a layout panel named LayoutRoot
            LiveTileHelper.CreateOrUpdateTile(flipData, new Uri("/View/EditForm.xaml?ID=" + taskHelper.selectedTask.Task_id + "", UriKind.RelativeOrAbsolute),true);
        }
    }
}