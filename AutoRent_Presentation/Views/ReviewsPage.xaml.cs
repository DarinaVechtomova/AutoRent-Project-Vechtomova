using AutoRent_Logic.Contexts;
using AutoRent_Logic.Services;
using AutoRent_Presentation.Services;
using System;
using System.Collections.Generic;
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
    public sealed partial class ReviewsPage : Page
    {
        DataBase db;
        private ReviewsEnumerator enumerator;
        public ReviewsPage()
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

        private void PageReviews_Unloaded(object sender, RoutedEventArgs e)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.SaveDataBase(db);
        }

        private async void ButtonPublish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = TextBoxName.Text;
                string response = TextBoxReviews.Text;
                RegisteredUserAction registeredUserAction = new RegisteredUserAction(db);
                registeredUserAction.LeaveResponse(name, response);
                TextBoxName.Text = "";
                TextBoxReviews.Text = "";
                Frame.Navigate(typeof(ReviewsPage), db);
            }
            catch (ArgumentException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
    }
}
