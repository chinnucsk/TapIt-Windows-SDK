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

            //Initialize the view
            _interstitialAdView = new InterstitialAdView();

            _interstitialAdView.Visible = Visibility.Collapsed;
            _interstitialAdView.ZoneId = 25253;   //zone id for TapIt
           // _interstitialAdView.ZoneId = 15093;     ////zone id for local server

            LayoutRoot.Children.Add(_interstitialAdView.ViewControl);

            //attached events
            _interstitialAdView.ControlLoaded += _interstitialAdView_ControlLoaded;
            _interstitialAdView.ContentLoaded += interstitialAdView_LoadCompleted;
            _interstitialAdView.ErrorEvent += interstitialAdView_ErrorEvent;
            _interstitialAdView.Navigating += interstitialAdView_navigating;
            _interstitialAdView.Navigated += interstitialAdView_navigated;
            _interstitialAdView.NavigationFailed += interstitialAdView_navigationFailed;
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

        ///<summary>
        /// //this event is fired when control is loaded
        ///</summary>
        void _interstitialAdView_ControlLoaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("_interstitialAdView_ControlLoaded");
        }

        /// <summary>
        ///  // This event is fired when the web browser navigation fails.
        /// </summary>
        void interstitialAdView_navigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("interstitialAdView_navigationFailed");
            MessageBox.Show("interstitialAdView_navigationFailed");
        }

        /// <summary>
        ///  // This event is fired when the web browser is navigated to a url.
        /// </summary>
        void interstitialAdView_navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("interstitialAdView_navigated");
        }

        /// <summary>
        ///  // The event is fired when user clicks on the web browser.
        /// </summary>
        void interstitialAdView_navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("interstitialAdView_navigating");
        }

        ///<summary>
        /// //this event is fired when error occurs
        ///</summary>
        void interstitialAdView_ErrorEvent(string strErrorMsg)
        {
            Debug.WriteLine("interstitialAdView_ErrorEvent :" + strErrorMsg);
            MessageBox.Show(strErrorMsg);
        }

        ///<summary>
        /// //this event is fired when contents are loaded
        ///</summary>
        void interstitialAdView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("interstitialAdView_LoadCompleted");
        }

        private async void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            progressring.Visibility = Visibility.Visible;
            bool display = await _interstitialAdView.Load();
            progressring.Visibility = Visibility.Collapsed;
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            loadinterstitialAd();
        }

        #endregion

        #region Methods

        private void loadinterstitialAd()
        {
            _interstitialAdView.Visible = Visibility.Visible;
            object obj = ContentPanel.FindName("TapItAdViewControl");
            if (obj != null)
            {
                // Ad View already added.
                progressring.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }


        }

        private void DeviceOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            _interstitialAdView.DeviceOrientationChanged(e.Orientation);
        }

        #endregion

      
    }
}