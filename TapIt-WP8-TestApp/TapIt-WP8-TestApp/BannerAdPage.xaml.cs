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
using System.Diagnostics;

namespace TapIt_WP8_TestApp
{
    public partial class BannerAdPage : PhoneApplicationPage
    {
        #region DataMember

        BannerAdView _bannerAdView;

        #endregion

        #region Constructor

        public BannerAdPage()
        {
            InitializeComponent();
            // Set the event handler for when the application deactivated
            (Application.Current as TapIt_WP8_TestApp.App).App_Deactivated +=
                          new EventHandler(MainPage_AppDeactivated);

            // Set the event handler for when the application activated
            (Application.Current as TapIt_WP8_TestApp.App).App_Activated +=
                          new EventHandler(MainPage_AppActivated);

            _bannerAdView = new BannerAdView();
        }

        #endregion

        #region methods

        private void loadBannerAd()
        {
            progressring.Visibility = System.Windows.Visibility.Visible;
            object obj = ContentPanel.FindName("TapItAdViewControl");
            if (obj != null)
            {
                // Ad View already added.
                _bannerAdView.Visible = Visibility.Visible;
                progressring.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            _bannerAdView.Visible = System.Windows.Visibility.Collapsed;
            _bannerAdView.ZoneId = 14999;
            _bannerAdView.ViewControl.SetValue(Grid.RowProperty, 2);
            ContentPanel.Children.Add(_bannerAdView.ViewControl);

            //attached events
            _bannerAdView.ControlLoaded += _bannerAdView_loaded;
            _bannerAdView.ContentLoaded += _bannerAdView_LoadCompleted;
            _bannerAdView.ErrorEvent += _bannerAdView_ErrorEvent;
            _bannerAdView.Navigating += _bannerAdView_navigating;
            _bannerAdView.Navigated += _bannerAdView_navigated;
            _bannerAdView.NavigationFailed += _bannerAdView_navigationFailed;
        }

        #endregion

        #region Events

        ///<summary>
        ///This event is fired when the app came to foreground
        ///</summary>
        private void MainPage_AppActivated(object sender, EventArgs e)
        {
            _bannerAdView.AppActivated();
        }

        ///<summary>
        ///This event is fired just before the app will be sent to the background.
        ///</summary>
        void MainPage_AppDeactivated(object sender, EventArgs e)
        {
            _bannerAdView.AppDeactivated();
        }

        void _bannerAdView_navigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("_bannerAdView_navigationFailed");
        }

        void _bannerAdView_navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_bannerAdView_navigated");
        }

        void _bannerAdView_navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("_bannerAdView_navigating");
        }

        private async void _bannerAdView_loaded(object sender, RoutedEventArgs e)
        {
            bool display = await _bannerAdView.LoadAndNavigate();
            progressring.Visibility = Visibility.Collapsed;
            _bannerAdView.AnimationTimeInterval = 10;
            _bannerAdView.AnimationDuration = 4;
        }

        void _bannerAdView_ErrorEvent(string strErrorMsg)
        {
            Debug.WriteLine("_bannerAdView_ErrorEvent :" + strErrorMsg);
        }

        void _bannerAdView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("_bannerAdView_LoadCompleted");
            progressring.Visibility = Visibility.Collapsed;
            _bannerAdView.Visible = Visibility.Visible;
        }

        private void hideBtn_Click(object sender, RoutedEventArgs e)
        {
            _bannerAdView.Visible = System.Windows.Visibility.Collapsed;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            loadBannerAd();
        }

        #endregion
    }
}