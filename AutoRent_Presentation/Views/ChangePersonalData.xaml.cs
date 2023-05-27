using AutoRent_Logic;
using AutoRent_Logic.Contexts;
using AutoRent_Presentation.Services;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoRent_Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangePersonalData : Page
    {
        DataBase db;
        public ChangePersonalData()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter is DataBase)
            {
                db = (DataBase)e.Parameter;
            }
        }
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            switch (args.InvokedItem)
            {
                case "HOME":
                    Frame.Navigate(typeof(MainPage), db);
                    break;
                case "RENTAL TERMS":
                    Frame.Navigate(typeof(RentalTermsPage), db);
                    break;
                case "ABOUT US":
                    Frame.Navigate(typeof(AboutUsPage), db);
                    break;
                case "REVIEWS":
                    Frame.Navigate(typeof(ReviewsPage), db);
                    break;
                case "MY ACCOUNT":
                    Frame.Navigate(typeof(PersonalAccountPage), db);
                    break;
            }
        }

        private void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PersonalAccountPage), db);
        }

        private void ButtonHistory_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PersonalOrderHistory), db);
        }

        private void ButtonPersonalData_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChangePersonalData), db);
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisteredRepository registeredRepository = new RegisteredRepository(db);
                RegisteredUser user = registeredRepository.FindByID(UserAuthentication.ID);
                string fullName = TextBoxFullName.Text;
                string telephoneNumber = TextBoxTelephoneNumber.Text;
                string email = TextBoxEmail.Text;
                RegisteredUser updatedUser = new RegisteredUser(email, fullName, user.DateOfBirth, telephoneNumber, user.Password, user.Id);
                registeredRepository.UpData(updatedUser);
                await new MessageDialog("Data was successfully changed").ShowAsync();
                Frame.Navigate(typeof (ChangePersonalData), db);
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void ButtonSingOut_Click(object sender, RoutedEventArgs e)
        {
            UserAuthentication.IsAuthentication = false;
            Frame.Navigate(typeof(MainPage), db);
        }

        private void PageChangePersonalData_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }

        private void PageChangePersonalData_Loaded(object sender, RoutedEventArgs e)
        {
            RegisteredRepository registeredRepository = new RegisteredRepository(db);
            RegisteredUser user = registeredRepository.FindByID(UserAuthentication.ID);
            TextBoxFullName.Text = user.FullName;
            TextBoxTelephoneNumber.Text = user.TelephonNumber;
            TextBoxEmail.Text = user.Email;
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChangePasswordPage), db);
        }
    }
}
