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
    public sealed partial class LogIn : Page
    {
        private DataBase db;

        public LogIn()
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
                    Frame.Navigate(typeof(LogIn), db);
                    break;
            }
        }

        private void ButtonLogOn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Registration), db);
        }

        private async void ButtonSignIn_ClickAsync(object sender, RoutedEventArgs e)
        {
            RegisteredUserAction registeredUserAction = new RegisteredUserAction(db);
            string email = TextBoxEmail.Text;
            string password = PasswordBoxPassword.Password;
            if (registeredUserAction.Authorize(email, password))
            {
                Frame.Navigate(typeof(PersonalAccountPage), db);
            }
            else if (AdminAuthentication.IsAuthenticated)
            {
                Frame.Navigate(typeof(MainPageAdmin), db);
            }
            else
            {
                await new MessageDialog("Incorrect email or password").ShowAsync();
            }
        }
    }
}
