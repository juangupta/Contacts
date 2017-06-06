using Contacts.Services;
using System.Collections.ObjectModel;
using System;
using Contacts.Models;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Contacts.ViewModels
{
    public class ContactsViewModel :INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Atributes
        private ApiService apiService;
        private DialogService dialogService;
        private bool isRefreshing;
        #endregion

        #region Properties
        public ObservableCollection<ContactItemViewModel> MyContacts { get; set; }
        public bool IsRefreshing
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
            get
            {
                return isRefreshing;
            }
        }

        #endregion

        #region Constructors
        public ContactsViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            MyContacts = new ObservableCollection<ContactItemViewModel>();
            LoadContacts();
        }
        #endregion

        #region Methods
        private async void LoadContacts()
        {

            /*if (!CrossConnectivity.Current.IsConnected)
            {
                await dialogService.ShowMessage("Error", "Check you internet connection.");
                return;
            }

            var isRechable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isRechable)
            {
                await dialogService.ShowMessage("Error", "Check you internet connection.");
                return;
            }*/
            //Lo ideal es llevar las constantes al diccionario de recursos
            IsRefreshing = true;
            var response = await apiService.Get<Contact>(
                "http://contactsxamarintata.azurewebsites.net",
                "/api",
                "/Contacts");
            IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }
            var contacts = (List < Contact >) response.Result;
            ReloadContacts(contacts);
        }

        private void ReloadContacts(List<Contact> contacts)
        {
            MyContacts.Clear();
            foreach (var contact in contacts.OrderBy(c => c.FirstName).ThenBy(c => c.LastName))
            {
                MyContacts.Add(new ContactItemViewModel
                {
                    ContactId = contact.ContactId,
                    EmailAddress = contact.EmailAddress,
                    FirstName = contact.FirstName,
                    Image = contact.Image,
                    LastName = contact.LastName,
                    PhoneNumber = contact.PhoneNumber,
                });
            }
        }
        #endregion
        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(Refresh);
            }
        }

        private void Refresh()
        {
            LoadContacts();
        }
        #endregion

    }
}
