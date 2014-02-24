using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Quotes_and_Messages.Helper;
using Microsoft.Phone.Tasks;

namespace Quotes_and_Messages
{
    public partial class ListOfQuotes : PhoneApplicationPage
    {
        private ApplicationBarIconButton nextPage;
        private ApplicationBarIconButton shareViaSMS;
        private ApplicationBarIconButton shareViaShareLink;
        private ApplicationBarIconButton copy;
        public ListOfQuotes()
        {
            InitializeComponent();
            initAppBar();
        }

        private void initAppBar()
        {
            ApplicationBar appBar = new ApplicationBar();

            nextPage = new ApplicationBarIconButton(new Uri("/Assets/AppBar/WP_appBar_NextIcon.png", UriKind.Relative));
            nextPage.Click += btnNextQuotes_click;
            nextPage.Text = "next page";
            appBar.Buttons.Add(nextPage);

            shareViaSMS = new ApplicationBarIconButton(new Uri("/Assets/AppBar/WP_appBar_SMSIcon.png", UriKind.Relative));
            shareViaSMS.Click += shareViaSMS_Click;
            shareViaSMS.Text = "Send SMS";
            appBar.Buttons.Add(shareViaSMS);

            shareViaShareLink = new ApplicationBarIconButton(new Uri("/Assets/AppBar/WP_appBar_ShareIcon.png", UriKind.Relative));
            shareViaShareLink.Click += shareViaShareLink_Click;
            shareViaShareLink.Text = "Share";
            appBar.Buttons.Add(shareViaShareLink);

            copy = new ApplicationBarIconButton(new Uri("/Assets/AppBar/WP_appBar_CopyIcon.png", UriKind.Relative));
            copy.Click += copy_Click;
            copy.Text = "Copy";
            appBar.Buttons.Add(copy);

            ApplicationBar = appBar;
        }

        void copy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedQuote))
            {
                Clipboard.SetText(selectedQuote);
                MessageBox.Show("Quote Copied to clipboard.", "Quotes & Messages", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Select a quote first.", "Quotes & Messages", MessageBoxButton.OK);
            }
        }

        void shareViaShareLink_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedQuote))
            {
                ShareStatusTask shareTask = new ShareStatusTask()
                {
                    Status = selectedQuote
                };
                shareTask.Show();
            }
            else
            {
                MessageBox.Show("Select any Quote first.", "Quotes & Messages", MessageBoxButton.OK);
            }
            
            //throw new NotImplementedException();
            //ShareLinkTask shareLinkTask = new ShareLinkTask()
            //{
            //    Title = "Code Samples",
            //    //LinkUri = new Uri("http://msdn.microsoft.com/en-us/library/windowsphone/develop/ff431744(v=vs.92).aspx", UriKind.Absolute),
            //    Message = "Here are some great code samples for Windows Phone."
            //};
            //shareLinkTask.Show();
        }

        void shareViaSMS_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedQuote))
            {
                SmsComposeTask sms = new SmsComposeTask()
                {
                    Body = selectedQuote
                };
                sms.Show();
            }
            else
            {
                MessageBox.Show("Select any Quote first.", "Quotes & Messages", MessageBoxButton.OK);
            }
        }

        
        
        public List<string> lstQuotes;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);

            lstQuotes = (List<string>)NavigationService.GetNavigationData();
            lbxQuotes.ItemsSource = lstQuotes;

            tBlockHeader.Text = MainPage.selectedCategory;
        }

        private void btnNextQuotes_click(object sender, EventArgs e)
        {
            ucBusy.IsBusy = true;
            //AppBarBtn_nextPage.IsEnabled = false;
            nextPage.IsEnabled = false;

            count = 0;
            lstQuotes = null;
            lstQuotes = new List<string>();

            getNext10Quotes();
        }

        private void getNext10Quotes()
        {
            try
            {
                var webClient = new WebClient();

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_FetchNextQuotesCompleted);
                webClient.DownloadStringAsync(new Uri("http://www.iheartquotes.com/api/v1/random?source=" + MainPage.selectedCategory));
            }
            catch (Exception ex)
            {
                ucBusy.IsBusy = false;
                nextPage.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
        }

        public string RandomSingleQuote;
        public int count = 0;
        private void webClient_FetchNextQuotesCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
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
                    getNext10Quotes();
                }
                else if (count == 10)
                {
                    ucBusy.IsBusy = false;
                    //AppBarBtn_nextPage.IsEnabled = true;
                    nextPage.IsEnabled = true;

                    lbxQuotes.ItemsSource = lstQuotes;
                }
                else
                {
                    ucBusy.IsBusy = false;
                    //AppBarBtn_nextPage.IsEnabled = true;
                    nextPage.IsEnabled = true;
                    MessageBox.Show("Something went wrong.");
                }
            }
            else
            {
                if (App.IsInternetAvailable_2nd)
                {
                    ucBusy.IsBusy = false;
                    nextPage.IsEnabled = true;

                    MessageBox.Show(e.Error.ToString());
                }
                else
                {
                    ucBusy.IsBusy = false;
                    nextPage.IsEnabled = true;

                    MessageBox.Show("No Internet", "Quotes & Messages", MessageBoxButton.OK);
                }
            }
        }


        public string selectedQuote;
        private void lbxQuotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedQuote = (string)lbxQuotes.SelectedItem;
        }
    }
}