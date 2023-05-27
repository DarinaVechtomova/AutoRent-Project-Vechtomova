using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Presentation.Services;
using AutoRent_Presentation.Views;
using System;
using System.Linq;
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
    public sealed partial class MainPageAdmin : Page
    {
        private DataBase db;
        public MainPageAdmin()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter is DataBase)
            {
                db = (DataBase)e.Parameter;
                ListViewCars.ItemsSource = db.TruckList.Cast<Vehicle>().Concat(db.PassengerCarList);
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
                    Frame.Navigate(typeof(MainPage), db);
                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            SelectedCarData.SelectedPassengerCar = null;
            SelectedCarData.SelectedTruck = null;
            Button btn = sender as Button;
            Vehicle selectedVehicle = btn.DataContext as Vehicle;
            if (selectedVehicle != null)
            {
                if (selectedVehicle is Truck)
                {
                    SelectedCarData.SelectedTruck = selectedVehicle as Truck;
                    Frame.Navigate(typeof(CarDataEditing), db);
                }
                else if (selectedVehicle is PassengerCar)
                {
                    SelectedCarData.SelectedPassengerCar = selectedVehicle as PassengerCar;
                    Frame.Navigate(typeof(CarDataEditing), db);
                }
            }
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Vehicle selectedVehicle = btn.DataContext as Vehicle;

            if (selectedVehicle != null)
            {
                AvailableCar availableCar = new AvailableCar();
                bool isAvailable = availableCar.isAvailableCar(selectedVehicle.Id, db);
                if (!isAvailable)
                {
                    await new MessageDialog("The rented vehicle cannot be deleted").ShowAsync();
                    return;
                }
                ContentDialog deleteConfirmationDialog = new ContentDialog
                {
                    Title = "Confirm deletion",
                    Content = $"Are you sure you want to delete {selectedVehicle.VehicleName}?",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel"
                };
                ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (selectedVehicle is PassengerCar)
                    {
                        PassengerCar selectedPassengerCar = selectedVehicle as PassengerCar;
                        PassengerCarRepository passengerCarRepository = new PassengerCarRepository(db);
                        passengerCarRepository.Remove(selectedPassengerCar);
                    }
                    else if (selectedVehicle is Truck)
                    {
                        Truck selectedTruck = selectedVehicle as Truck;
                        TruckRepository truckRepository = new TruckRepository(db);
                        truckRepository.Remove(selectedTruck);
                    }
                    DataBaseManager dataBaseManager = new DataBaseManager();
                    dataBaseManager.SaveDataBase(db);
                    Frame.Navigate(typeof(MainPageAdmin), db);
                }
            }
            
        }

        private void PageMainAdmin_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }
    }
}
