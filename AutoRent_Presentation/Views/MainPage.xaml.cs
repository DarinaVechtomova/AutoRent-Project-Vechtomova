using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Presentation.Services;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoRent_Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DataBase db;

        public MainPage()
        {
            this.InitializeComponent();           
            db = new DataBase();
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

        private void myPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            if (!DataBaseManager.isLoaded)
            {
                dataBaseManager.UploadDataBase(db);
                DataBaseManager.isLoaded = true;                
                Frame.Navigate(typeof(MainPage), db);
            }           
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
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
                    Frame.Navigate(typeof(TruckAutoInfo), db);
                }
                else if (selectedVehicle is PassengerCar)
                {
                    SelectedCarData.SelectedPassengerCar = selectedVehicle as PassengerCar;
                    Frame.Navigate(typeof(PassengerAutoInfo), db);
                }
            }           
        }     
    }
}
