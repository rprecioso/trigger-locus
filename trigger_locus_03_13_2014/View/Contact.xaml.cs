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
using System.Windows.Media;

namespace trigger_locus_03_13_2014.View
{
    public partial class Contact : PhoneApplicationPage
    {
        //ContactHelper conHelper = new ContactHelper(App.connectionString);
        ContactHelper conHelper = App.contactViewModel;
        string ID;
        public Contact()
        {
            InitializeComponent();
       
            this.Loaded += Contact_Loaded;
        }

        void Contact_Loaded(object sender, RoutedEventArgs e)
        {
            
            conHelper.loadContacts(Convert.ToInt32(ID));
            this.DataContext = conHelper;
        }


        private void Removefromlist(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image myImage = (Image)sender;
            Grid myGrid = GetParentDependencyObjectFromVisualTree(myImage, typeof(Grid)) as Grid;
            StackPanel myStackPanel = GetParentDependencyObjectFromVisualTree(myGrid, typeof(StackPanel)) as StackPanel;
            Grid myGrid2 = GetParentDependencyObjectFromVisualTree(myStackPanel, typeof(Grid)) as Grid;

            ListBoxItem myListBoxItem = GetParentDependencyObjectFromVisualTree(myGrid2, typeof(ListBoxItem)) as ListBoxItem;
            ListBox myListBox = GetParentDependencyObjectFromVisualTree(myListBoxItem, typeof(ListBox)) as ListBox;

            Microsoft.Phone.UserData.Contact myContact = (Microsoft.Phone.UserData.Contact)myStackPanel.Tag;

            conHelper.removeContact((string)myImage.Tag.ToString());
        }

        private void appBarCancelButton_Click(object sender, EventArgs e)
        {
            conHelper.conList.Clear();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("ID", out ID);

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                conHelper.resetContacts(Convert.ToInt32(ID));
                foreach (contactTemplate item in conHelper.conList)
                {
                    conHelper.addContact(item, Convert.ToInt32(ID));
                }
            }


        }

        private void Add_tolistn(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image myImage = (Image)sender;
            Grid myGrid = GetParentDependencyObjectFromVisualTree(myImage, typeof(Grid)) as Grid;
            ListBoxItem myListBoxItem = GetParentDependencyObjectFromVisualTree(myGrid, typeof(ListBoxItem)) as ListBoxItem;
            ListBox myListBox = GetParentDependencyObjectFromVisualTree(myListBoxItem, typeof(ListBox)) as ListBox;
            StackPanel myStackPanel = GetParentDependencyObjectFromVisualTree(myListBox, typeof(StackPanel)) as StackPanel;
            Microsoft.Phone.UserData.Contact myContact = (Microsoft.Phone.UserData.Contact)myStackPanel.Tag;

            contactTemplate newCon = new contactTemplate()
            {
                contact = myContact,
                NumSelected = myImage.Tag.ToString()
            };
            conHelper.addContact(newCon);
        }

        private void Add_toliste(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image myImage = (Image)sender;
            Grid myGrid = GetParentDependencyObjectFromVisualTree(myImage, typeof(Grid)) as Grid;
            ListBoxItem myListBoxItem = GetParentDependencyObjectFromVisualTree(myGrid, typeof(ListBoxItem)) as ListBoxItem;
            ListBox myListBox = GetParentDependencyObjectFromVisualTree(myListBoxItem, typeof(ListBox)) as ListBox;
            StackPanel myStackPanel = GetParentDependencyObjectFromVisualTree(myListBox, typeof(StackPanel)) as StackPanel;
            Microsoft.Phone.UserData.Contact myContact = (Microsoft.Phone.UserData.Contact)myStackPanel.Tag;
            contactTemplate newCon = new contactTemplate()
            {
                contact = myContact,
                EmaSelected = myImage.Tag.ToString()
            };
            conHelper.addContact(newCon);
        }


        public static DependencyObject GetParentDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            //Walk the visual tree to get the parent of this control
            DependencyObject parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))

                    break;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }


        private void autoCompleteContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Microsoft.Phone.UserData.Contact con = ((contactTemplate)autoCompleteContacts.SelectedItem).contact;
                conHelper.loadFilteredContacts(con.DisplayName);
                autoCompleteContacts.Text = "";
            }
            catch (Exception) { }
        }

    }
}