using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trigger_locus_03_13_2014.Model
{
    public class trigger_locus_DataContext : DataContext
    {
        public trigger_locus_DataContext(string connectionString)
            : base(connectionString)
        { }

        public Table<TblTask> TblTasks;

        public Table<RelTaskLocation> RelTaskLocations;

        public Table<RelTaskContact> RelTaskContacts;

        //public Table<RelConNumber> RelConNumbers;

        //public Table<RelConEmail> RelConEmails;
    }

    [Table]
    public class TblTask : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _Task_id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Task_id
        {
            get
            {
                return _Task_id;
            }
            set
            {
                if (_Task_id != value)
                {
                    NotifyPropertyChanging("Task_id");
                    _Task_id = value;
                    NotifyPropertyChanged("Task_id");
                }
            }
        }

        private string _Title;
        [Column]
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    NotifyPropertyChanging("Title");
                    _Title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private DateTime _Date_task;
        [Column]
        public DateTime Date_task
        {
            get { return _Date_task; }
            set
            {
                if (_Date_task != value)
                {
                    NotifyPropertyChanging("Date_task");
                    _Date_task = value;
                    NotifyPropertyChanged("Date_task");
                }
            }
        }

        private String _Description;
        [Column]
        public String Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    NotifyPropertyChanging("Description");
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private double _TileWidth;
        [Column]
        public double TileWidth
        {
            get { return _TileWidth; }
            set
            {
                if (_TileWidth != value)
                {
                    NotifyPropertyChanging("TileWidth");
                    _TileWidth = value;
                    NotifyPropertyChanged("TileWidth");
                }
            }
        }

        private DateTime? _Time_task;
        [Column]
        public DateTime? Time_task
        {
            get { return _Time_task; }
            set
            {
                if (_Time_task != value)
                {
                    NotifyPropertyChanging("Time_task");
                    _Time_task = value;
                    NotifyPropertyChanged("Time_task");
                }
            }
        }

        private int _dur_id;
        [Column]
        public int dur_id
        {
            get { return _dur_id; }
            set
            {
                if (_dur_id != value)
                {
                    NotifyPropertyChanging("dur_id");
                    _dur_id = value;
                    NotifyPropertyChanged("dur_id");
                }
            }
        }

        private int _occ_id;
        [Column]
        public int occ_id
        {
            get { return _occ_id; }
            set
            {
                if (_occ_id != value)
                {
                    NotifyPropertyChanging("occ_id");
                    _occ_id = value;
                    NotifyPropertyChanged("occ_id");
                }
            }
        }

        private bool _isActive;
        [Column]
        public bool isActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    NotifyPropertyChanging("isActive");
                    _isActive = value;
                    NotifyPropertyChanged("isActive");
                }
            }
        }


        [Column(IsVersion = true)]
        private Binary _version;


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table]
    public class RelTaskLocation : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _Tas_loc_id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Tas_loc_id
        {
            get { return _Tas_loc_id; }
            set
            {
                if (_Tas_loc_id != value)
                {
                    NotifyPropertyChanging("Tas_loc_id");
                    _Tas_loc_id = value;
                    NotifyPropertyChanged("Tas_loc_id");
                }
            }
        }

        private double _Latitude;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public double Latitude
        {
            get { return _Latitude; }
            set
            {
                if (_Latitude != value)
                {
                    NotifyPropertyChanging("Latitude");
                    _Latitude = value;
                    coordinate = new GeoCoordinate(Latitude, Longitude);
                    NotifyPropertyChanged("Latitude");
                }
            }
        }

        private double _Longitude;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public double Longitude
        {
            get { return _Longitude; }
            set
            {
                if (_Longitude != value)
                {
                    NotifyPropertyChanging("Longitude");
                    _Longitude = value;
                    coordinate = new GeoCoordinate(Latitude, Longitude);
                    NotifyPropertyChanged("Longitude");
                }
            }
        }

        private GeoCoordinate _coordinate;
        [TypeConverter(typeof(GeoCoordinateConverter))]
        public GeoCoordinate coordinate
        {
            get { return _coordinate; }
            set
            {
                NotifyPropertyChanging("coordinate");
                _coordinate = value;
                NotifyPropertyChanged("coordinate");
            }
        }

        private double _Altitude;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public double Altitude
        {
            get { return _Altitude; }
            set
            {
                if (_Altitude != value)
                {
                    NotifyPropertyChanging("Altitude");
                    _Altitude = value;
                    NotifyPropertyChanged("Altitude");
                }
            }
        }

        private string _Address;
        [Column]
        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    NotifyPropertyChanging("Address");
                    _Address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }

        private int _Task_id;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Task_id
        {
            get
            {
                return _Task_id;
            }
            set
            {
                if (_Task_id != value)
                {
                    NotifyPropertyChanging("Task_id");
                    _Task_id = value;
                    NotifyPropertyChanged("Task_id");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table]
    public class RelTaskContact : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _con_id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int con_id
        {
            get { return _con_id; }
            set
            {
                if (_con_id != value)
                {
                    NotifyPropertyChanging("con_id");
                    _con_id = value;
                    NotifyPropertyChanged("con_id");
                }
            }
        }


        private string _Name;
        [Column]
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    NotifyPropertyChanging("Name");
                    _Name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _Address;
        [Column]
        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    NotifyPropertyChanging("Address");
                    _Address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }

        private int _Task_id;
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Task_id
        {
            get
            {
                return _Task_id;
            }
            set
            {
                if (_Task_id != value)
                {
                    NotifyPropertyChanging("Task_id");
                    _Task_id = value;
                    NotifyPropertyChanged("Task_id");
                }
            }
        }

        private string _Number;
        [Column]
        public string Number
        {
            get { return _Number; }
            set
            {
                if (_Number != value)
                {
                    NotifyPropertyChanging("Number");
                    _Number = value;
                    NotifyPropertyChanged("Number");
                }
            }
        }

        private string _Email;
        [Column]
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    NotifyPropertyChanging("Email");
                    _Email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    //[Table]
    //public class RelConNumber : INotifyPropertyChanged, INotifyPropertyChanging
    //{
    //    private int _con_num_id;
    //    [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
    //    public int con_num_id
    //    {
    //        get { return _con_num_id; }
    //        set
    //        {
    //            if (_con_num_id != value)
    //            {
    //                NotifyPropertyChanging("con_num_id");
    //                _con_num_id = value;
    //                NotifyPropertyChanged("con_num_id");
    //            }
    //        }
    //    }

    //    private string _Number;
    //    [Column]
    //    public string Number
    //    {
    //        get { return _Number; }
    //        set
    //        {
    //            if (_Number != value)
    //            {
    //                NotifyPropertyChanging("Number");
    //                _Number = value;
    //                NotifyPropertyChanged("Number");
    //            }
    //        }
    //    }


    //    private int _con_id;
    //    [Column(UpdateCheck = UpdateCheck.Never)]
    //    public int con_id
    //    {
    //        get
    //        {
    //            return _con_id;
    //        }
    //        set
    //        {
    //            if (_con_id != value)
    //            {
    //                NotifyPropertyChanging("con_id");
    //                _con_id = value;
    //                NotifyPropertyChanged("con_id");
    //            }
    //        }
    //    }

    //    #region INotifyPropertyChanged Members

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    // Used to notify the page that a data context property changed
    //    private void NotifyPropertyChanged(string propertyName)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion

    //    #region INotifyPropertyChanging Members

    //    public event PropertyChangingEventHandler PropertyChanging;

    //    // Used to notify the data context that a data context property is about to change
    //    private void NotifyPropertyChanging(string propertyName)
    //    {
    //        if (PropertyChanging != null)
    //        {
    //            PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
    //        }
    //    }

    //    #endregion
    //}

    //[Table]
    //public class RelConEmail : INotifyPropertyChanged, INotifyPropertyChanging
    //{
    //    private int _con_ema_id;
    //    [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
    //    public int con_ema_id
    //    {
    //        get { return _con_ema_id; }
    //        set
    //        {
    //            if (_con_ema_id != value)
    //            {
    //                NotifyPropertyChanging("con_ema_id");
    //                _con_ema_id = value;
    //                NotifyPropertyChanged("con_ema_id");
    //            }
    //        }
    //    }

    //    private string _Email;
    //    [Column]
    //    public string Email
    //    {
    //        get { return _Email; }
    //        set
    //        {
    //            if (_Email != value)
    //            {
    //                NotifyPropertyChanging("Email");
    //                _Email = value;
    //                NotifyPropertyChanged("Email");
    //            }
    //        }
    //    }


    //    private int _con_id;
    //    [Column(UpdateCheck = UpdateCheck.Never)]
    //    public int con_id
    //    {
    //        get
    //        {
    //            return _con_id;
    //        }
    //        set
    //        {
    //            if (_con_id != value)
    //            {
    //                NotifyPropertyChanging("con_id");
    //                _con_id = value;
    //                NotifyPropertyChanged("con_id");
    //            }
    //        }
    //    }

    //    #region INotifyPropertyChanged Members

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    // Used to notify the page that a data context property changed
    //    private void NotifyPropertyChanged(string propertyName)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion

    //    #region INotifyPropertyChanging Members

    //    public event PropertyChangingEventHandler PropertyChanging;

    //    // Used to notify the data context that a data context property is about to change
    //    private void NotifyPropertyChanging(string propertyName)
    //    {
    //        if (PropertyChanging != null)
    //        {
    //            PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
    //        }
    //    }

    //    #endregion
    //}
}
