using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Quotes_and_Messages.Controls
{
    public partial class BusyIndicator : UserControl
    {
        public BusyIndicator()
        {
            InitializeComponent();
        }

        void BusyIndicator_Loaded(object sender, RoutedEventArgs e)
        {
            IsBusy = false;
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(BusyIndicator), new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var _BusyIndicator = d as BusyIndicator;
                _BusyIndicator.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception)
            {
            }
        }

    }
}
