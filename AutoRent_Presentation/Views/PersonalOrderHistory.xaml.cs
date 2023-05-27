using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using AutoRent_Presentation.Services;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoRent_Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalOrderHistory : Page
    {
        DataBase db;
        public PersonalOrderHistory()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter is DataBase)
            {
                db = (DataBase)e.Parameter;
                int userId = UserAuthentication.ID;
                RegisteredUserAction userAction = new RegisteredUserAction(db);
                List<Orders> orderHistory = userAction.GetOrderHistory(userId);
                ListViewOrderHistory.ItemsSource = orderHistory;
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

        private void ButtonPersonalData_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChangePersonalData), db);
        }

        private void ButtonSignOut_Click(object sender, RoutedEventArgs e)
        {
            UserAuthentication.IsAuthentication = false;
            Frame.Navigate(typeof(MainPage), db);
        }
    }
}
