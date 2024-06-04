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
using Telerik.Windows.Controls;
using trigger_locus_03_13_2014.Sources;
using trigger_locus_03_13_2014.Model;

namespace trigger_locus_03_13_2014.View
{
    public partial class Calendar : PhoneApplicationPage
    {
        bool firstLoad = true;
        public Calendar()
        {
            InitializeComponent();         
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mainCalendar.DisplayDate = App.dateSelected;
            mainCalendar.SelectedValue = App.dateSelected;
            firstLoad = false;
        }

        private void mainCalendar_SelectedValueChanged(object sender, ValueChangedEventArgs<object> e)
        {
            if (firstLoad == false)
            {
                App.dateSelected = mainCalendar.SelectedValue ?? DateTime.Now;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }


    }

    public class AppointmentSources : AppointmentSource
    {
        TaskHelper taskHelper = new TaskHelper(App.connectionString);
        public AppointmentSources()
        {
      
        }

        public override void FetchData(DateTime startDate, DateTime endDate)
        {
            taskHelper.loadTask();
            this.AllAppointments.Clear();
            foreach (TblTask item in taskHelper.allTasks)
            {
                this.AllAppointments.Add(new UserAppointments()
                {
                    StartDate = item.Date_task,
                    EndDate = item.Date_task.AddHours(1),
                    Subject = item.Title,
                    Details = item.Description,
                    Location = ""
                });
            }

            this.OnDataLoaded(); // notify that there is new data to display
        }
    }
}