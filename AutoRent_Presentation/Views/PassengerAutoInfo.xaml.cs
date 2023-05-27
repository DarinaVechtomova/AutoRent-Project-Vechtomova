using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Presentation.Services;
using System;
using System.Collections.ObjectModel;
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
    public sealed partial class PassengerAutoInfo : Page
    {
        private DataBase db;
        public ObservableCollection<PassengerCar> slctdPassengerCar { get; set; }
        public PassengerAutoInfo()
        {
            this.InitializeComponent();
            slctdPassengerCar = new ObservableCollection<PassengerCar>
            {
                SelectedCarData.SelectedPassengerCar,
            };
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
        private async void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            if (UserAuthentication.IsAuthentication)
            {
                Frame.Navigate(typeof(OrderPage), db);
            }
            else
            {
                await new MessageDialog("To rent a car you need to log in").ShowAsync();
            }
        }
    }
}
