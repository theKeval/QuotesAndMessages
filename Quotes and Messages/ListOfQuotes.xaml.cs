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

namespace Quotes_and_Messages
{
    public partial class ListOfQuotes : PhoneApplicationPage
    {
        public ListOfQuotes()
        {
            InitializeComponent();
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

        }
    }
}