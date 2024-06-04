using Microsoft.Phone.UserData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using trigger_locus_03_13_2014.Model;

namespace trigger_locus_03_13_2014.ViewModel
{
    public class ContactHelper : INotifyPropertyChanged
    {
        trigger_locus_DataContext triggerDB;

        public ContactHelper(string connectionString)
        {
            triggerDB = new trigger_locus_DataContext(connectionString);
        }



        private ObservableCollection<RelTaskContact> _allContactE;
        public ObservableCollection<RelTaskContact> allContactE
        {
            get { return _allContactE; }
            set
            {
                _allContactE = value;
                RaisePropertyChanged("allContactE");
            }
        }

        private ObservableCollection<contactTemplate> _allContact;
        public ObservableCollection<contactTemplate> allContact
        {
            get { return _allContact; }
            set
            {
                _allContact = value;
                RaisePropertyChanged("allContact");
            }
        }

        private ObservableCollection<contactTemplate> _conList;
        public ObservableCollection<contactTemplate> conList
        {
            get { return _conList; }
            set
            {
                _conList = value;
                RaisePropertyChanged("conList");
            }
        }

        private ObservableCollection<contactTemplate> _filteredList;
        public ObservableCollection<contactTemplate> filteredList
        {
            get { return _filteredList; }
            set
            {
                _filteredList = value;
                RaisePropertyChanged("filteredList");
            }
        }

        private contactTemplate _selectedCon;
        public contactTemplate selectedCon
        {
            get { return _selectedCon; }
            set
            {
                _selectedCon = value;
                RaisePropertyChanged("selectedCon");
            }
        }

        public void loadContacts()
        {
            Contacts cons = new Contacts();
            cons.SearchCompleted += cons_SearchCompleted;
            cons.SearchAsync("", FilterKind.None, "Contacts");
        }

        public void loadContacts(int Task_ID)
        {
            if (triggerDB.RelTaskContacts.Where(c => c.Task_id == Task_ID).Count() > 0)
            {
                conList.Clear();
                List<RelTaskContact> searchRelCon = triggerDB.RelTaskContacts.Where(c => c.Task_id == Task_ID).ToList();
                foreach (RelTaskContact item in searchRelCon)
                {
                    Contact searchCon = allContact.Where(c => c.Name == item.Name).First().contact;
                    conList.Add(new contactTemplate { contact = searchCon, NumSelected = item.Number, EmaSelected = item.Email, con_id = item.con_id });
                    //conList = new ObservableCollection<contactTemplate>(triggerDB.RelTaskContacts.Where(c => c.Task_id == Task_ID).Select(c => new contactTemplate { contact = searchCon, NumSelected = c.Number, EmaSelected = c.Email }).ToList());
                }


            }
        }

        public void loadContactE()
        {
            allContactE = new ObservableCollection<RelTaskContact>(triggerDB.RelTaskContacts.Select(c => c));
        }

        public void loadFilteredContacts(string query)
        {
            try
            {
                filteredList = new ObservableCollection<contactTemplate>(allContact.Where(c => c.Name.Contains(query)).ToList());
            }
            catch (Exception) { }

        }

        void cons_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            if (e.Results.Count() > 0)
            {
                allContact = new ObservableCollection<contactTemplate>(e.Results.Select(c => new contactTemplate { contact = c }).ToList());
            }
        }

        public void addContact(contactTemplate con)
        {
            if (conList.Where(c => c.NumSelected == con.NumSelected).Count() == 0 || conList.Where(c => c.EmaSelected == con.EmaSelected).Count() == 0)
            {
                conList.Add(con);
            }
        }

        public void removeContact(string query)
        {

            try
            {
                conList.Remove(conList.Where(c => c.NumSelected == query).First());
            }
            catch (Exception)
            {

                conList.Remove(conList.Where(c => c.EmaSelected == query).First());
            }

            //allContact.Add(new contactTemplate { contact = con });
        }

        public void deleteContact(int con_id)
        {
            conList.Remove(conList.Where(c => c.con_id == con_id).First());
            triggerDB.RelTaskContacts.DeleteOnSubmit(triggerDB.RelTaskContacts.Where(c => c.con_id == con_id).First());
            triggerDB.SubmitChanges();
        }

        public void resetContacts(int task_id)
        {
            triggerDB.RelTaskContacts.DeleteAllOnSubmit(triggerDB.RelTaskContacts.Where(c => c.Task_id == task_id));
            triggerDB.SubmitChanges();
        }

        public void addContact(contactTemplate con, int task_id)
        {
            if (triggerDB.RelTaskContacts.Where(c => c.Task_id == task_id && (c.Number == con.NumSelected || c.Email == con.EmaSelected)).Count() == 0)
            {
                //Add the new contact
                RelTaskContact newCon = new RelTaskContact()
                {
                    Address = "",
                    Name = con.Name,
                    Email = con.EmaSelected,
                    Number = con.NumSelected,
                    Task_id = task_id
                };
                triggerDB.RelTaskContacts.InsertOnSubmit(newCon);
                triggerDB.SubmitChanges();
            }
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


    public class contactTemplate
    {
        List<emaC> ema = new List<emaC>();
        List<numC> num = new List<numC>();

        private Contact _contact;
        public Contact contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        public BitmapImage Picture
        {
            get
            {
                BitmapImage img = new BitmapImage();
                try
                {
                    img.SetSource(contact.GetPicture());
                }
                catch (Exception) { }
                return img;
            }
        }

        public string Name
        {
            get { return contact.DisplayName; }
        }


        public List<numC> Number
        {
            get
            {
                num.Clear();
                foreach (ContactPhoneNumber item in contact.PhoneNumbers)
                {
                    num.Add(new numC { phoneNumber = item.PhoneNumber });
                }
                return num;
            }
        }

        public List<emaC> Email
        {
            get
            {
                ema.Clear();
                foreach (ContactEmailAddress item in contact.EmailAddresses)
                {
                    ema.Add(new emaC { emailAddress = item.EmailAddress });
                }
                return ema;
            }
        }

        private string _NumSelected;

        public string NumSelected
        {
            get { return _NumSelected; }
            set
            {
                if (value != null)
                {
                    displayContact = value;
                }
                _NumSelected = value;
            }
        }

        private string _EmaSelected;

        public string EmaSelected
        {
            get { return _EmaSelected; }
            set
            {
                if (value != null)
                {
                    displayContact = value;
                }
                _EmaSelected = value;
            }
        }

        public int con_id { get; set; }


        public string displayContact { get; set; }




    }

    public class numC
    {
        public string phoneNumber { get; set; }
    }

    public class emaC
    {
        public string emailAddress { get; set; }
    }
}
