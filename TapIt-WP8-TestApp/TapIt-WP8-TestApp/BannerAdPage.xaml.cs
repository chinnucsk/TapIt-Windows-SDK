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
using System.Threading.Tasks;

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
                          new EventHandler(BannerAdPage_AppDeactivated);

            // Set the event handler for when the application activated
            (Application.Current as TapIt_WP8_TestApp.App).App_Activated +=
                          new EventHandler(BannerAdPage_AppActivated);

            //Initialize the view
            _bannerAdView = new BannerAdView();

            _bannerAdView.Visible = Visibility.Collapsed;
            _bannerAdView.ZoneId = 25252;//2720;          //zone id for TapIt
            //_bannerAdView.ZoneId = 15087;                  //zone id for local server
            _bannerAdView.ViewControl.SetValue(Grid.RowProperty, 2);
            ContentPanel.Children.Add(_bannerAdView.ViewControl);

            //attached events
            _bannerAdView.ControlLoaded += _bannerAdView_controlLoaded;
            _bannerAdView.ContentLoaded += _bannerAdView_contentLoaded;
            _bannerAdView.ErrorEvent += _bannerAdView_errorEvent;
            _bannerAdView.Navigating += _bannerAdView_navigating;
            _bannerAdView.Navigated += _bannerAdView_navigated;
            _bannerAdView.NavigationFailed += _bannerAdView_navigationFailed;
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
            }
        }

        private void DeviceOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            _bannerAdView.DeviceOrientationChanged(e.Orientation);
        }

        #endregion

        #region Events

        ///<summary>
        ///This event is fired when the app came to foreground
        ///</summary>
        private void BannerAdPage_AppActivated(object sender, EventArgs e)
        {
            _bannerAdView.AppActivated();
        }

        ///<summary>
        ///This event is fired just before the app will be sent to the background.
        ///</summary>
        void BannerAdPage_AppDeactivated(object sender, EventArgs e)
        {
            _bannerAdView.AppDeactivated();
        }

        /// <summary>
        ///  // This event is fired when the web browser navigation fails.
        /// </summary>
        void _bannerAdView_navigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("_bannerAdView_navigationFailed");
            MessageBox.Show("_bannerAdView_navigationFailed");
        }

        /// <summary>
        ///  // This event is fired when the web browser is navigated to a url.
        /// </summary>
        void _bannerAdView_navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_bannerAdView_navigated");
        }

        /// <summary>
        ///  // The event is fired when user clicks on the web browser.
        /// </summary>
        void _bannerAdView_navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("_bannerAdView_navigating");
            MessageBox.Show("_bannerAdView_navigating");
        }

        ///<summary>
        /// //this event is fired when control is loaded
        ///</summary>
        private void _bannerAdView_controlLoaded(object sender, RoutedEventArgs e)
        {

        }

        ///<summary>
        /// //this event is fired when error occurs
        ///</summary>
        void _bannerAdView_errorEvent(string strErrorMsg)
        {
            Debug.WriteLine("_bannerAdView_ErrorEvent :" + strErrorMsg);
            progressring.Visibility = Visibility.Collapsed;
            MessageBox.Show(strErrorMsg);
        }

        ///<summary>
        /// //this event is fired when contents are loaded
        ///</summary>
        void _bannerAdView_contentLoaded(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("_bannerAdView_LoadCompleted");
            progressring.Visibility = Visibility.Collapsed;
        }

        private void hideBtn_Click(object sender, RoutedEventArgs e)
        {
            _bannerAdView.Visible = Visibility.Collapsed;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            progressring.Visibility = Visibility.Visible;
            //_bannerAdView.AnimationTimeInterval = 10;
            //_bannerAdView.AnimationDuration = 4;
            _bannerAdView.Load();
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            loadBannerAd();
        }

        private void PhoneApplicationPage_Unloaded_1(object sender, RoutedEventArgs e)
        {
            // remove the event handler for when the application deactivated
            (Application.Current as TapIt_WP8_TestApp.App).App_Deactivated -=
                          new EventHandler(BannerAdPage_AppDeactivated);

            // remove the event handler for when the application activated
            (Application.Current as TapIt_WP8_TestApp.App).App_Activated -=
                          new EventHandler(BannerAdPage_AppActivated);
        }

        #endregion


    }
}