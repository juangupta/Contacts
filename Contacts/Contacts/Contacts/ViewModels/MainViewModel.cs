using Contacts.Services;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Contacts.ViewModels
{
    public class MainViewModel
    {
        #region Atributes
        public ContactsViewModel Contacts { get; set; }
        private NavigationService navigationService;
        #endregion

        #region Properties
        public NewContactViewModel NewContact { get; set; } 
        #endregion

        #region Constructors
        public MainViewModel()
        {
            Contacts = new ContactsViewModel();
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand AddContactCommand { get { return new RelayCommand(AddContact); } }
        private async void AddContact()
        {
            NewContact = new NewContactViewModel();
            await navigationService.Navigate("NewContactPage");
        }
        #endregion
    }
}