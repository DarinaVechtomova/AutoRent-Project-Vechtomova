using AutoRent_Logic;
using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using AutoRent_Presentation.Services;
using AutoRent_Presentation.Views;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoRent_Presentation.ViewModels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WaitingConfirmationAdmin : Page
    {
        private DataBase db;
        public WaitingConfirmationAdmin()
        {
            this.InitializeComponent();
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter is DataBase)
            {
                db = (DataBase)e.Parameter;
                ListViewWaiting.ItemsSource = db.WaitingForConfirmationList;
            }
        }
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            switch (args.InvokedItem)
            {
                case "HOME":
                    Frame.Navigate(typeof(MainPageAdmin), db);
                    break;
                case "RENTAL TERMS":
                    Frame.Navigate(typeof(RentalTermsAdmin), db);
                    break;
                case "ABOUT US":
                    Frame.Navigate(typeof(AboutUsAdmin), db);
                    break;
                case "REVIEWS":
                    Frame.Navigate(typeof(ReviewsAdmin), db);
                    break;
                case "ADD CAR":
                    Frame.Navigate(typeof(AddCarPage), db);
                    break;
                case "WAITING FOR CONFIRMATION":
                    Frame.Navigate(typeof(WaitingConfirmationAdmin), db);
                    break;
                case "ORDERS":
                    Frame.Navigate(typeof(OrdersAdmin), db);
                    break;
                case "EXIT":
                    AdminAuthentication.IsAuthenticated = false;
                    Frame.Navigate(typeof(LogIn), db);
                    break;
            }
        }

        private void PageWaitingConfirmation_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }

        private async void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            ManagerAction managerAction = new ManagerAction(db);
            Button btn = sender as Button;
            WaitingForBookingConfirmation selectedWaiting = btn.DataContext as WaitingForBookingConfirmation;
            if (selectedWaiting != null)
            {
                managerAction.ConfirmCarRental(selectedWaiting.OrderId);
                await new MessageDialog($"Order {selectedWaiting.OrderId} successfully approved").ShowAsync();
            }           
            Frame.Navigate(typeof(WaitingConfirmationAdmin), db);
        }

        private async void ButtonReject_Click(object sender, RoutedEventArgs e)
        {
            ManagerAction managerAction = new ManagerAction(db);
            Button btn = sender as Button;
            WaitingForBookingConfirmation selectedWaiting = btn.DataContext as WaitingForBookingConfirmation;
            if (selectedWaiting != null)
            {
                managerAction.CancelCarRental(selectedWaiting.OrderId);
                await new MessageDialog($"Order {selectedWaiting.OrderId} was rejected").ShowAsync();
            }
            Frame.Navigate(typeof(WaitingConfirmationAdmin), db);
        }
    }
}
