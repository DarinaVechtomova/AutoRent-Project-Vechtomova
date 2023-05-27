using AutoRent_Logic.Contexts;
using AutoRent_Logic.Models;
using AutoRent_Logic.Services;
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
    public sealed partial class OrderPage : Page
    {
        private DataBase db;  
        private int orderId;
        public OrderPage()
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

        private void RadioButtonPayNow_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockCardNumber.Visibility = Visibility.Visible;
            TextBlockCVV.Visibility = Visibility.Visible;
            TextBlockDate.Visibility = Visibility.Visible;
            TextBoxCardNumber.Visibility = Visibility.Visible;
            TextBoxCVV.Visibility = Visibility.Visible;
            TextBoxDate.Visibility = Visibility.Visible;
        }

        private void RudioButtonPayOnDelivery_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockCardNumber.Visibility = Visibility.Collapsed;
            TextBlockCVV.Visibility = Visibility.Collapsed;
            TextBlockDate.Visibility = Visibility.Collapsed;
            TextBoxCardNumber.Visibility = Visibility.Collapsed;
            TextBoxCVV.Visibility = Visibility.Collapsed;
            TextBoxDate.Visibility = Visibility.Collapsed;
        }

        private async void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int carId;
                if (SelectedCarData.SelectedPassengerCar != null)
                {
                    carId = SelectedCarData.SelectedPassengerCar.Id;
                }
                else
                {
                    carId = SelectedCarData.SelectedTruck.Id;
                }
                DateTimeOffset dateStart = (DateTimeOffset)DateStart.Date;
                DateTimeOffset dateEnd = (DateTimeOffset)DateEnd.Date;
                DateTime dateStartDateTime = dateStart.Date;
                DateTime dateEndDateTime = dateEnd.Date;
                string adress = TextBoxAdress.Text;
                RegisteredUserAction registeredUserAction = new RegisteredUserAction(db);
                int userId = UserAuthentication.ID;
                string cardNumber = TextBoxCardNumber.Text;
                string cardExpirationDate = TextBoxDate.Text;
                string cvv = TextBoxCVV.Text;
                WaitingListRepository waitingRepository = new WaitingListRepository(db);
                WaitingForBookingConfirmation waitingCar = waitingRepository.FindByID(orderId);
                if (waitingCar == null)
                {
                    if (RadioButtonPayNow.IsChecked == true)
                    {
                        registeredUserAction.OrderNotify += registeredUserAction.DisplayMessage;
                        registeredUserAction.SuccessPayNotify += registeredUserAction.DisplayMessage;
                        bool isMakeRental = registeredUserAction.MakeCarRental(carId, dateStartDateTime, dateEndDateTime, adress, userId, out orderId);
                        bool isPaid = registeredUserAction.PayForYourOrder(orderId, cardNumber, cardExpirationDate, cvv);
                        Frame.Navigate(typeof(MainPage), db);
                    }
                    else if (RudioButtonPayOnDelivery.IsChecked == true)
                    {
                        registeredUserAction.OrderNotify += registeredUserAction.DisplayMessage;
                        bool isMakeRental = registeredUserAction.MakeCarRental(carId, dateStartDateTime, dateEndDateTime, adress, userId, out orderId);
                        Frame.Navigate(typeof(MainPage), db);
                    }
                    else
                    {
                        await new MessageDialog("Select a payment method").ShowAsync();
                    }
                }
                else
                {
                    registeredUserAction.SuccessPayNotify += registeredUserAction.DisplayMessage;
                    bool isPaid = registeredUserAction.PayForYourOrder(orderId, cardNumber, cardExpirationDate, cvv);
                    Frame.Navigate(typeof(MainPage), db);
                }                              
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void DateStart_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
            {
                int costRent = 0;
                DateTimeOffset dateStart = (DateTimeOffset)DateStart.Date;
                DateTimeOffset? dateEnd = DateEnd.Date;
                DateTime dateStartDateTime = dateStart.Date;
                DateTime dateEndDateTime = dateEnd?.Date ?? DateTime.Today.AddDays(-1);
                DateTime yesterday = DateTime.Today.AddDays(-1);
                if (dateEndDateTime != yesterday.Date && dateStartDateTime <= dateEndDateTime)
                {
                    if (SelectedCarData.SelectedTruck != null)
                    {
                        costRent = Vehicle.CalculationOfTheCostOfRenting(SelectedCarData.SelectedTruck.RentalPrice, PassengerCar.Pledge, dateStartDateTime, dateEndDateTime);
                    }
                    else if (SelectedCarData.SelectedPassengerCar != null)
                    {
                        costRent = Vehicle.CalculationOfTheCostOfRenting(SelectedCarData.SelectedPassengerCar.RentalPrice, Truck.Pledge, dateStartDateTime, dateEndDateTime);
                    }
                    TextBoxCost.Text = $"{costRent}$";
                }
                else
                {
                    TextBoxCost.Text = "";
                }
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }           
        }

        private async void DateEnd_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
            {               
                int costRent = 0;
                DateTimeOffset? dateStart = (DateTimeOffset)DateStart.Date;
                DateTimeOffset dateEnd = (DateTimeOffset)DateEnd.Date;
                DateTime dateStartDateTime = dateStart?.Date ?? DateTime.Today.AddDays(-1);
                DateTime dateEndDateTime = dateEnd.Date;
                DateTime yesterday = DateTime.Today.AddDays(-1);
                if (dateStartDateTime != yesterday.Date && dateStartDateTime <= dateEndDateTime)
                {
                    if (SelectedCarData.SelectedTruck != null)
                    {
                        costRent = Vehicle.CalculationOfTheCostOfRenting(SelectedCarData.SelectedTruck.RentalPrice, Truck.Pledge, dateStartDateTime, dateEndDateTime);
                    }
                    else if (SelectedCarData.SelectedPassengerCar != null)
                    {
                        costRent = Vehicle.CalculationOfTheCostOfRenting(SelectedCarData.SelectedPassengerCar.RentalPrice, PassengerCar.Pledge, dateStartDateTime, dateEndDateTime);
                    }
                    TextBoxCost.Text = $"{costRent}$";
                }
                else
                {
                    TextBoxCost.Text = "";
                }
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void PageOrder_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }
    }
}
