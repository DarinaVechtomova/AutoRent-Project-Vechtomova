using AutoRent_Logic.Contexts;
using AutoRent_Logic;
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
    public sealed partial class ChangePasswordPage : Page
    {
        private DataBase db;
        public ChangePasswordPage()
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
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisteredRepository registeredRepository = new RegisteredRepository(db);
                RegisteredUser user = registeredRepository.FindByID(UserAuthentication.ID);
                string password = PasswordBoxPassword.Password;
                string passwordConfirm = PasswordBoxConfirmation.Password;
                if (password != passwordConfirm)
                {
                    await new MessageDialog("Passwords do not match").ShowAsync();
                }
                RegisteredUser updatedUser = new RegisteredUser(user.Email, user.FullName, user.DateOfBirth, user.TelephonNumber, password, user.Id);
                registeredRepository.UpData(updatedUser);
                await new MessageDialog("Password was successfully changed").ShowAsync();
                Frame.Navigate(typeof(ChangePersonalData), db);
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void PageChangePassword_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }
    }
}
