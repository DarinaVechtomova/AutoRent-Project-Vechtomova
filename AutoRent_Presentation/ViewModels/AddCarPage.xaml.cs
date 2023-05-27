using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Presentation.Services;
using AutoRent_Presentation.Views;
using System;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoRent_Presentation.ViewModels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddCarPage : Page
    {
        private DataBase db;
        private string imgPath;
        public AddCarPage()
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

        private void RadioButtonTruck_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockTruck_Passenger.Text = "Cargo item volume";
            TextBoxPledge.Text = Truck.Pledge.ToString();
        }

        private void RadioButtonPassengerCar_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockTruck_Passenger.Text = "Number of passengers";
            TextBoxPledge.Text = PassengerCar.Pledge.ToString();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int maxIdT = 0, maxIdP = 0;
                if (db.TruckList.Count > 0)
                {
                    maxIdT = db.TruckList.Max(w => w.Id);
                }
                if (db.PassengerCarList.Count > 0)
                {
                    maxIdP = db.PassengerCarList.Max(w => w.Id);
                }
                int maxId = 0;
                if (maxIdT > maxIdP)
                {
                    maxId = maxIdT;
                }
                else if (maxIdP > maxIdT)
                {
                    maxId = maxIdP;
                }
                int id = maxId + 1;
                string name = TextBoxName.Text;
                string engine = TextBoxEngine.Text;
                string transmissionTypeString = (ComboBoxTransmission.SelectedItem as ComboBoxItem)?.Content as string;
                TransmissionType transmissionType = Enum.Parse<TransmissionType>(transmissionTypeString);
                string fuelTypeString = (ComboBoxFuel.SelectedItem as ComboBoxItem)?.Content as string;
                FuelType fuelType = Enum.Parse<FuelType>(fuelTypeString);                
                double fuelConsumption = double.Parse(TextBoxConsumption.Text);
                int rentalPrice = int.Parse(TextBoxPrice.Text);
                int pledge = int.Parse(TextBoxPledge.Text);
                
                if (RadioButtonPassengerCar.IsChecked == true)
                {
                    int numberOfPassengerSeats = int.Parse(TextBoxVolume_People.Text);
                    PassengerCar newPassengerCar = new PassengerCar(id, name, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, numberOfPassengerSeats, imgPath);
                    PassengerCarRepository repository = new PassengerCarRepository(db);
                    repository.Add(newPassengerCar);
                    await new MessageDialog("Auto added").ShowAsync();
                    Frame.Navigate(typeof(AddCarPage), db);
                }
                else if (RadioButtonTruck.IsChecked == true)
                {
                    double cargoItemVolume = double.Parse(TextBoxVolume_People.Text);
                    Truck newTruck = new Truck(id, name, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, cargoItemVolume, imgPath);
                    TruckRepository repository = new TruckRepository(db);
                    repository.Add(newTruck);
                    await new MessageDialog("Auto added").ShowAsync();
                    Frame.Navigate(typeof(AddCarPage), db);
                }
                else
                {
                    await new MessageDialog("Select car type").ShowAsync();
                }              
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            catch
            {
                await new MessageDialog("Error adding car").ShowAsync();
            }
        }

        private void AddPage_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }

        private async void ButtonPhoto_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.CommitButtonText = "OK";
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    ImageCar.Source = bitmapImage;
                }
            }
            StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder picturesFolder = await installedLocation.GetFolderAsync("Assets");
            imgPath = System.IO.Path.Combine(picturesFolder.Path, file.Name);
        }
    }
}
