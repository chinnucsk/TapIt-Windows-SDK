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
    public partial class InterstitialAdPage : PhoneApplicationPage
    {
        #region DataMember

        InterstitialAdView _interstitialAdView;

        #endregion

        #region constructor

        public InterstitialAdPage()
        {
            InitializeComponent();
            // Set the event handler for when the application deactivated
            (Application.Current as TapIt_WP8_TestApp.App).App_Deactivated +=
                          new EventHandler(MainPage_AppDeactivated);

            // Set the event handler for when the application activated
            (Application.Current as TapIt_WP8_TestApp.App).App_Activated +=
                          new EventHandler(MainPage_AppActivated);

            _interstitialAdView = new InterstitialAdView();
           
        }

        #endregion

        #region Events

        ///<summary>
        ///This event is fired when the app came to foreground
        ///</summary>
        private void MainPage_AppActivated(object sender, EventArgs e)
        {
            _interstitialAdView.AppActivated();
        }

        ///<summary>
        ///This event is fired just before the app will be sent to the background.
        ///</summary>
        void MainPage_AppDeactivated(object sender, EventArgs e)
        {
            _interstitialAdView.AppDeactivated();
        }

        void interstitialAdView_navigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("interstitialAdView_navigationFailed");
        }

        void interstitialAdView_navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("interstitialAdView_navigated");
        }

        void interstitialAdView_navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("interstitialAdView_navigating");
        }

        void interstitialAdView_ErrorEvent(string strErrorMsg)
        {
            Debug.WriteLine("interstitialAdView_ErrorEvent :" + strErrorMsg);
        }

        void interstitialAdView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("interstitialAdView_LoadCompleted");
        }

        private async void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            progressring.Visibility = Visibility.Visible;
            loadinterstitialAd();
            bool display = await _interstitialAdView.LoadAndNavigate();
            if (display)
            {
             // showBtn.Visibility = Visibility.Visible;
            }
            _interstitialAdView.Visible = Visibility.Visible;
            progressring.Visibility = Visibility.Collapsed;
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            _interstitialAdView.Visible = Visibility.Visible;
        }

        #endregion

        #region Methods

        private void loadinterstitialAd()
        {
            _interstitialAdView.Visible = System.Windows.Visibility.Collapsed;
            object obj = ContentPanel.FindName("TapItAdViewControl");
            if (obj != null)
            {
                // Ad View already added.
                progressring.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
           
            _interstitialAdView.ZoneId = 2719;
            ContentPanel.Children.Add(_interstitialAdView.ViewControl);

            //attached events
            _interstitialAdView.ContentLoaded += interstitialAdView_LoadCompleted;
            _interstitialAdView.ErrorEvent += interstitialAdView_ErrorEvent;
            _interstitialAdView.Navigating += interstitialAdView_navigating;
            _interstitialAdView.Navigated += interstitialAdView_navigated;
            _interstitialAdView.NavigationFailed += interstitialAdView_navigationFailed;
        }

        #endregion
    }
}