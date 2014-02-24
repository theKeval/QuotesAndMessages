using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Quotes_and_Messages.Resources;
using Quotes_and_Messages.Helper;

namespace Quotes_and_Messages
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        public static List<string> lstCategories = new List<string>()
        {
            "anonymous", "Why Why Why", "Wisdom", "Art", "Work", "Friends", "Computation", "Education", "Science",
            "Technology", "Computers", "Macintosh", "Love", "Songs Poems", "Osho", "Albert Einstein", "Shakespeare",
            "People", "Men Women", "Kids", "Pets", "Politics", "Sports", "Math", "Medicine", "Holygrail", "News",
            "Hogfather", "Law Liberty", "Literature", "Lords", "Definitions", "Dictionary", "Military", "Food",
            "Fortunes", "Humorix Stories", "Joel on Software", "Cryptonomicon", "Children of Dune", "Startrek",
            "Starwars", "Codehappy", "Zippy", "Discworld", "Dune", "John heywood", "Sex", "Vulgar", "Dictionary of the Vulgar tongue"
        };

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.Back)
            {
                count = 0;
                lstQuotes = null;
                lstQuotes = new List<string>();
            }

            lbxCategories.ItemsSource = lstCategories;

        }

        public static string selectedCategory;
        private void lbxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.IsInternetAvailable)
            {
                if (lbxCategories.SelectedItem != null)
                {
                    switch (((string)lbxCategories.SelectedItem).ToLower())
                    {
                        case "dictionary of the vulgar tongue":
                            selectedCategory = "1811 dictionary of the vulgar tongue ";
                            break;

                        case "songs poems":
                            selectedCategory = "songs_poems";
                            break;

                        case "men women":
                            selectedCategory = "men_women";
                            break;

                        case "humorix stories":
                            selectedCategory = "humorix_stories";
                            break;

                        case "joel on software":
                            selectedCategory = "joel_on_software";
                            break;

                        case "john heywood":
                            selectedCategory = "john_heywood";
                            break;

                            

                        default:
                            selectedCategory = ((string)lbxCategories.SelectedItem).ToLower();
                            break;
                    }

                    ucBusy.IsBusy = true;
                    get10Quotes(selectedCategory);
                }
            }
            else
            {
                MessageBox.Show("Looks like you are not connected with Internet.\nCheck your internet connection and try again.");
            }
        }

        public string RandomSingleQuote;
        public List<string> lstQuotes = new List<string>();
        public int count = 0;
        private void webClient_TipsCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();

            if (e.Error == null)
            {
                var MainQuote = e.Result;
                string[] splitedString = MainQuote.Split('[');
                RandomSingleQuote = splitedString[0];

                if (RandomSingleQuote.Contains("&quot;"))
                {
                    RandomSingleQuote = RandomSingleQuote.Replace("&quot;", " \" ");
                }

                lstQuotes.Add(RandomSingleQuote);
                count++;

                if (count < 10)
                {
                    get10Quotes(selectedCategory);
                }
                else if (count == 10)
                {
                    ucBusy.IsBusy = false;

                    NavigationService.Navigate(new Uri("/ListOfQuotes.xaml", UriKind.Relative), lstQuotes);
                }
                else
                {
                    ucBusy.IsBusy = false;
                    MessageBox.Show("Something went wrong.");
                }
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        public List<string> get10Quotes(string Category)
        {
            //for (int i = 0; i < 10; i++)
            //{
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_TipsCompleted);
            webClient.DownloadStringAsync(new Uri("http://www.iheartquotes.com/api/v1/random?source=" + Category));
            //}
            return lstQuotes;
        }


        public void RandomQuote()
        {
            try
            {
                if (App.IsInternetAvailable)
                {
                    ucBusy.IsBusy = true;

                    var webClient = new WebClient();

                    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_RandomQuoteGenerated);
                    webClient.DownloadStringAsync(new Uri("http://iheartquotes.com/api/v1/random?max_lines=4"));
                }
                else
                {
                    MessageBox.Show("Looks like you are not connected to Internet.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void webClient_RandomQuoteGenerated(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var RandomQuote = e.Result;
                string[] splitedString = RandomQuote.Split('[');
                var QuoteToDisplay = splitedString[0];

                if (QuoteToDisplay.Contains("&quot;"))
                {
                    QuoteToDisplay = QuoteToDisplay.Replace("&quot;", " \" ");
                }

                tBlockRandomQuote.Text = QuoteToDisplay;

                ucBusy.IsBusy = false;
            }
            else
            {
                ucBusy.IsBusy = false;
                MessageBox.Show(e.Error.ToString());
            }
        }

        private void BtnGenerateRandomQuote_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            RandomQuote();
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}