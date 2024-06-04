using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trigger_locus_03_13_2014.Model;

namespace trigger_locus_03_13_2014.ViewModel
{
    public class LocationHelper : INotifyPropertyChanged
    {
        trigger_locus_DataContext triggerDB;

        public LocationHelper(string connectionString)
        {
            triggerDB = new trigger_locus_DataContext(connectionString);
        }

        private ObservableCollection<RelTaskLocation> _allLocation;
        public ObservableCollection<RelTaskLocation> allLocation
        {
            get { return _allLocation; }
            set
            {
                _allLocation = value;
                RaisePropertyChanged("allLocation");
            }
        }

        private ObservableCollection<RelTaskLocation> _locList;
        public ObservableCollection<RelTaskLocation> locList
        {
            get { return _locList; }
            set
            {
                _locList = value;
                RaisePropertyChanged("locList");
            }
        }

        private RelTaskLocation _selectedLoc;
        public RelTaskLocation selectedLoc
        {
            get { return _selectedLoc; }
            set
            {
                _selectedLoc = value;
                RaisePropertyChanged("selectedLoc");
            }
        }

        public void addLocation(RelTaskLocation loc)
        {
            triggerDB.RelTaskLocations.InsertOnSubmit(loc);
            triggerDB.SubmitChanges();
        }

        public void updateLocation(RelTaskLocation loc)
        {
            RelTaskLocation locQuery = (from lo in triggerDB.RelTaskLocations
                                        where lo.Tas_loc_id == loc.Tas_loc_id
                                        select lo).First();
            locQuery.Altitude = loc.Altitude;
            locQuery.Latitude = loc.Latitude;
            locQuery.Longitude = loc.Longitude;
            locQuery.Address = loc.Address;
            locQuery.Task_id = loc.Task_id;
            triggerDB.SubmitChanges();
        }

        public void deleteLocation(int _Tas_loc_id)
        {
            locList.Remove(locList.Where(l => l.Tas_loc_id == _Tas_loc_id).First());
            triggerDB.RelTaskLocations.DeleteOnSubmit(triggerDB.RelTaskLocations.Where(r => r.Tas_loc_id == _Tas_loc_id).First());
            triggerDB.SubmitChanges();
        }

        public void renameLocation(int _Tas_loc_id, string newAddress)
        {
            locList.Where(l => l.Tas_loc_id == _Tas_loc_id).First().Address = newAddress;
            triggerDB.RelTaskLocations.Where(l => l.Tas_loc_id == _Tas_loc_id).First().Address = newAddress;
            triggerDB.SubmitChanges();
        }

        public void resetLocation(int _task_id)
        {
            locList.Clear();
            triggerDB.RelTaskLocations.DeleteAllOnSubmit(triggerDB.RelTaskLocations.Where(r => r.Task_id == _task_id));
            triggerDB.SubmitChanges();
        }

        public void loadLocation()
        {
            var location = from loc in triggerDB.RelTaskLocations
                           select loc;
            allLocation = new ObservableCollection<RelTaskLocation>(location);
        }

        public void loadLocation(int _task_id)
        {
            var location = from loc in allLocation
                           where loc.Task_id == _task_id
                           select loc;
            locList = new ObservableCollection<RelTaskLocation>(location);
        }

        public void loadLocation(int _loc_Task_id, int _task_id)
        {
            var location = from loc in allLocation
                           where loc.Tas_loc_id == _loc_Task_id
                           select loc;
            selectedLoc = location.First();
        }

        public void saveChanges()
        {
            triggerDB.SubmitChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
