using AutoRent_Presentation.Services;
using System;
using System.Linq;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using AutoRent_Logic.Contexts;
using AutoRent_Presentation.Views;
using AutoRent_Logic.Models;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoRent_Presentation.ViewModels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CarDataEditing : Page
    {
        private DataBase db;
        private string imgPath;
        public CarDataEditing()
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
        private void PageEditCar_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedCarData.SelectedTruck != null)
            {
                TextBlockTruck_Passenger.Text = "Cargo item volume";
                TextBoxName.Text = SelectedCarData.SelectedTruck.VehicleName;
                TextBoxEngine.Text = SelectedCarData.SelectedTruck.Engine;
                string transmissionType = Enum.GetName(typeof(TransmissionType), SelectedCarData.SelectedTruck.TransmissionType);
                ComboBoxTransmission.SelectedItem = ComboBoxTransmission.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == transmissionType);
                string fuelType = Enum.GetName(typeof(FuelType), SelectedCarData.SelectedTruck.TransmissionType);
                ComboBoxFuel.SelectedItem = ComboBoxFuel.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == fuelType);
                ComboBoxFuel.SelectedItem = SelectedCarData.SelectedTruck.FuelType;
                TextBoxConsumption.Text = SelectedCarData.SelectedTruck.FuelConsumption.ToString();
                TextBoxPrice.Text = SelectedCarData.SelectedTruck.RentalPrice.ToString();
                TextBoxPledge.Text = Truck.Pledge.ToString();
                TextBoxVolume_People.Text = SelectedCarData.SelectedTruck.CargoItemVolume.ToString();
                ImageCar.Source = new BitmapImage(new Uri(SelectedCarData.SelectedTruck.Img));
            }
            else if (SelectedCarData.SelectedPassengerCar != null)
            {
                TextBlockTruck_Passenger.Text = "Number of passengers";
                TextBoxName.Text = SelectedCarData.SelectedPassengerCar.VehicleName;
                TextBoxEngine.Text = SelectedCarData.SelectedPassengerCar.Engine;
                string transmissionType = Enum.GetName(typeof(TransmissionType), SelectedCarData.SelectedPassengerCar.TransmissionType);
                ComboBoxTransmission.SelectedItem = ComboBoxTransmission.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == transmissionType);
                string fuelType = Enum.GetName(typeof(FuelType), SelectedCarData.SelectedPassengerCar.TransmissionType);
                ComboBoxFuel.SelectedItem = ComboBoxFuel.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == fuelType);
                TextBoxConsumption.Text = SelectedCarData.SelectedPassengerCar.FuelConsumption.ToString();
                TextBoxPrice.Text = SelectedCarData.SelectedPassengerCar.RentalPrice.ToString();
                TextBoxPledge.Text = PassengerCar.Pledge.ToString();
                TextBoxVolume_People.Text = SelectedCarData.SelectedPassengerCar.NumberOfPassengerSeats.ToString();
                ImageCar.Source = new BitmapImage(new Uri(SelectedCarData.SelectedPassengerCar.Img));
            }          
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

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = TextBoxName.Text;
                string engine = TextBoxEngine.Text;
                string transmissionTypeString = (ComboBoxTransmission.SelectedItem as ComboBoxItem)?.Content as string;
                TransmissionType transmissionType = Enum.Parse<TransmissionType>(transmissionTypeString);
                string fuelTypeString = (ComboBoxFuel.SelectedItem as ComboBoxItem)?.Content as string;
                FuelType fuelType = Enum.Parse<FuelType>(fuelTypeString);
                double fuelConsumption = double.Parse(TextBoxConsumption.Text);
                int rentalPrice = int.Parse(TextBoxPrice.Text);
                int pledge = int.Parse(TextBoxPledge.Text);

                if (SelectedCarData.SelectedPassengerCar != null)
                {
                    int id = SelectedCarData.SelectedPassengerCar.Id;
                    int numberOfPassengerSeats = int.Parse(TextBoxVolume_People.Text);
                    if (imgPath == null)
                    {
                        imgPath = SelectedCarData.SelectedPassengerCar.Img;
                    }
                    PassengerCar newPassengerCar = new PassengerCar(id, name, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, numberOfPassengerSeats, imgPath);
                    PassengerCarRepository repository = new PassengerCarRepository(db);
                    repository.UpData(newPassengerCar);
                    await new MessageDialog("Car updated").ShowAsync();
                }
                else if (SelectedCarData.SelectedTruck != null)
                {
                    int id = SelectedCarData.SelectedTruck.Id;
                    double cargoItemVolume = double.Parse(TextBoxVolume_People.Text);
                    if (imgPath == null)
                    {
                        imgPath = SelectedCarData.SelectedTruck.Img;
                    }
                    Truck newTruck = new Truck(id, name, engine, transmissionType, fuelType, fuelConsumption, rentalPrice, pledge, cargoItemVolume, imgPath);
                    TruckRepository repository = new TruckRepository(db);
                    repository.UpData(newTruck);
                    await new MessageDialog("Car updated").ShowAsync();
                }
                Frame.Navigate(typeof(MainPageAdmin), db);
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            catch
            {
                await new MessageDialog("Error updating car").ShowAsync();
            }
        }

        private void PageEditCar_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }
    }
}
