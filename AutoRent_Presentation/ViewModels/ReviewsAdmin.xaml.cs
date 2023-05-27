using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using AutoRent_Presentation.Services;
using AutoRent_Presentation.Views;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoRent_Presentation.ViewModels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewsAdmin : Page
    {
        private DataBase db;
        private ReviewsEnumerator enumerator;
        public ReviewsAdmin()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter is DataBase)
            {
                db = (DataBase)e.Parameter;
                enumerator = new ReviewsEnumerator(db.ReviewsList);
                List<Reviews> reviewsList = new List<Reviews>();
                foreach (Reviews reviews in enumerator)
                {
                    reviewsList.Add(reviews);
                }
                reviewsListView.ItemsSource = reviewsList;
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
    }
}
