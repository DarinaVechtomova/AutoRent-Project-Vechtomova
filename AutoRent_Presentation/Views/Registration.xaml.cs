using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using AutoRent_Presentation.Services;
using AutoRent_Presentation.ViewModels;
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
    public sealed partial class Registration : Page
    {
        DataBase db;
        public Registration()
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
                    if (UserAuthentication.IsAuthentication)
                        Frame.Navigate(typeof(PersonalAccountPage), db);
                    else
                        Frame.Navigate(typeof(LogIn), db);
                    break;
            }
        }

        private async void ButtonSignUp_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {

                Guest guest = new Guest(db);
                string fullName = TextBoxName.Text;
                string telephone = TextBoxTelephone.Text;
                string email = TextBoxEmail.Text;
                DateTimeOffset dateBirth = (DateTimeOffset)CalendarBirth.Date;
                DateTime dateBirthDateTime = dateBirth.Date;
                string password = PasswordBoxPassword.Password;
                string passwordRepeat = PasswordBoxRepeatPassword.Password;
                if (password != passwordRepeat)
                {
                    await new MessageDialog("Passwords do not match").ShowAsync();
                    PasswordBoxRepeatPassword.Password = "";
                }
                else
                {
                    guest.SignUp(email, fullName, dateBirthDateTime, telephone, password);
                    if (UserAuthentication.IsAuthentication)
                        Frame.Navigate(typeof(PersonalAccountPage), db);
                    else if (AdminAuthentication.IsAuthenticated)
                        Frame.Navigate(typeof(MainPageAdmin), db);
                }
            } 
            catch (ArgumentException ex) 
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void PageRegistration_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }
    }
}
