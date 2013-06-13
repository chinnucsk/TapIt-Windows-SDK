using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TapIt_WP8;

namespace TapIt_WP8_TestApp
{
    public partial class AlertAdPage : PhoneApplicationPage
    {
        #region DataMember

        AdView tapItAdView;

        #endregion

        public AlertAdPage()
        {
            InitializeComponent();
            tapItAdView = new AlertAdView();
        }


        private void tapItAdView_LoadCompleted(object sender, NavigationEventArgs e)
        {
        }

        private  void tapItAdView_loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private async void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            progressring.Visibility = Visibility.Visible;
            tapItAdView.Visible = System.Windows.Visibility.Collapsed;
            tapItAdView.ZoneId = 15501;

            bool display = await tapItAdView.Load();
            //tapItAdView.ViewControl.SetValue(Grid.RowProperty, 2);
            //ContentPanel.Children.Add(tapItAdView.ViewControl);

            //attached events
            tapItAdView.ControlLoaded += tapItAdView_loaded;
            tapItAdView.ContentLoaded += tapItAdView_LoadCompleted;

            if (display)
            {
                progressring.Visibility = Visibility.Collapsed;
                tapItAdView.Visible = Visibility.Visible;
            }
            else
            {
                progressring.Visibility = Visibility.Collapsed;
            }
        }
    }
}