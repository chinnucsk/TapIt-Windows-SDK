using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Windows.Media;

namespace TapIt_WP8.Resources
{
    partial class InAppBrowserPage : PhoneApplicationPage
    {
        #region DataMember

        public static AdViewBase _adViewBase = null;
        private string _uriString = String.Empty;

        #endregion

        #region Constructor

        public InAppBrowserPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (webBrowser.Source != null)
            {
                _uriString = webBrowser.Source.AbsoluteUri;
            }

            if (_adViewBase != null)
            {
                _adViewBase.OnInAppBrowserClosed(this);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string uri = String.Empty;
            if (string.IsNullOrEmpty(_uriString))
            {
                string navigateToUri = String.Empty;
                NavigationContext.QueryString.TryGetValue("navigatingUri", out navigateToUri);
                uri = navigateToUri;
            }
            else
            {
                uri = _uriString;
            }

            webBrowser.Navigate(new Uri(uri));

            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            SetPanelSize(deviceData.DeviceOrientation);
        }

        // handle to orientation change event
        private void PhoneApplicationPage_OrientationChanged_1(object sender,
            OrientationChangedEventArgs e)
        {
            SetPanelSize(e.Orientation);
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            webBrowser.GoBack();
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            webBrowser.GoForward();
        }

        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            this.NavigationService.GoBack();
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            EnableNavigation();
        }

        private void webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            EnableNavigation();
        }

        private void webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            EnableNavigation();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            try
            {
                webBrowser.InvokeScript("eval", "history.go()");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Methods

        private void EnableNavigation()
        {
            backBtn.IsEnabled = webBrowser.CanGoBack;
            nextBtn.IsEnabled = webBrowser.CanGoForward;
        }

        //set browser size depending on orientation.
        private void SetPanelSize(PageOrientation e)
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;

            if (e == PageOrientation.LandscapeLeft ||
                e == PageOrientation.LandscapeRight)
            {
                webBrowser.Height = deviceData.ScreenWidth - navigationGrid.Height;
                webBrowser.Width = deviceData.ScreenHeight;
                navigationGrid.Width = deviceData.ScreenHeight;
            }
            else if (e == PageOrientation.PortraitUp ||
                e == PageOrientation.PortraitDown)
            {
                webBrowser.Height = deviceData.ScreenHeight - navigationGrid.Height;
                webBrowser.Width = deviceData.ScreenWidth;
                navigationGrid.Width = deviceData.ScreenWidth;
            }
        }

        #endregion

    }
}