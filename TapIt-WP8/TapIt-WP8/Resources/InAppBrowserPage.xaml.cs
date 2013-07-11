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

namespace TapIt_WP8.Resources
{
    public partial class InAppBrowserPage : PhoneApplicationPage
    {
        #region DataMember

        string _navigatingUri = string.Empty;

        //public delegate void InAppBrowserClosedEventHandler();
        //public event InAppBrowserClosedEventHandler InAppBrowserClosed;

        #endregion

        #region Constructor

        public InAppBrowserPage()
        {
            InitializeComponent();
           
        }

        #endregion

        #region Events

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            NavigationContext.QueryString.TryGetValue("myparameter1", out _navigatingUri);
            webBrowser.Navigate(new Uri(_navigatingUri));
            SetPanelSize(deviceData.PageOrientation);
        }

        private void PhoneApplicationPage_OrientationChanged_1(object sender, 
            OrientationChangedEventArgs e)
        {
            SetPanelSize(e.Orientation);
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.GoBack();
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.GoForward();
        }

        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            enableNavigation();
        }

        private void webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            enableNavigation();
        }

        private void webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            enableNavigation();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                webBrowser.InvokeScript("eval", "history.go()");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void webBrowser_Unloaded(object sender, RoutedEventArgs e)
        {
            //OnInAppbrowserClosed();
        }

        #endregion

        #region Methods

        //public void OnInAppbrowserClosed()
        //{
        //    // Make a temporary copy of the event to avoid possibility of 
        //    // a race condition if the last subscriber unsubscribes 
        //    // immediately after the null check and before the event is raised.
        //    InAppBrowserClosedEventHandler handler = InAppBrowserClosed;
        //    if (handler != null)
        //    {
        //        handler();
        //    }
        //}

        private void enableNavigation()
        {
            backBtn.IsEnabled = webBrowser.CanGoBack;
            nextBtn.IsEnabled = webBrowser.CanGoForward;
        }

        private void SetPanelSize(PageOrientation e)
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;

            if (e == PageOrientation.LandscapeLeft
                         || e == PageOrientation.LandscapeRight)
            {
                webBrowser.Height = deviceData.ScreenWidth - navigationGrid.Height;
                webBrowser.Width = deviceData.ScreenHeight;
                navigationGrid.Width = deviceData.ScreenHeight;
            }
            else if (e == PageOrientation.PortraitUp
                || e == PageOrientation.PortraitDown)
            {
                webBrowser.Height = deviceData.ScreenHeight - navigationGrid.Height;
                webBrowser.Width = deviceData.ScreenWidth;
                navigationGrid.Width = deviceData.ScreenWidth;
            }
        }

        #endregion

    }
}