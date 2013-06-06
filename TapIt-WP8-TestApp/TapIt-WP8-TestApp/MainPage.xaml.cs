using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TapIt_WP8_TestApp.Resources;
using TapIt_WP8;
using Microsoft.Phone.Tasks;
using System.Diagnostics;

namespace TapIt_WP8_TestApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region DataMember

        AdView tapItAdView;

        #endregion

        #region Constructor

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Set the event handler for when the application deactivated
            (Application.Current as TapIt_WP8_TestApp.App).App_Deactivated +=
                          new EventHandler(MainPage_AppDeactivated);

            // Set the event handler for when the application activated
            (Application.Current as TapIt_WP8_TestApp.App).App_Activated +=
                          new EventHandler(MainPage_AppActivated);

            tapItAdView = new BannerAdView();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        #endregion

        #region Events

        ///<summary>
        ///This event is fired when the app came to foreground
        ///</summary>
        private void MainPage_AppActivated(object sender, EventArgs e)
        {
            tapItAdView.AppActivated();
        }

        ///<summary>
        ///This event is fired just before the app will be sent to the background.
        ///</summary>
        void MainPage_AppDeactivated(object sender, EventArgs e)
        {
            tapItAdView.AppDeactivated();
        }

        ///<summary>
        ///dispaly ad
        ///</summary>
        private void Banner_Ad_Click(object sender, RoutedEventArgs e)
        {
            progressring.Visibility = System.Windows.Visibility.Visible;
            object obj = ContentPanel.FindName("TapItAdViewControl");
            if (obj != null)
            {
                // Ad View already added.
                tapItAdView.Visible = Visibility.Visible;
                progressring.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            //tapItAdView.BaseURL = "http://ec2-107-20-3-62.compute-1.amazonaws.com/~chetanch/adrequest.php";
            tapItAdView.Visible = System.Windows.Visibility.Collapsed;
            tapItAdView.Height = 80;
            tapItAdView.Width = 480;
            tapItAdView.Margin = new Thickness(0, 400, 0, 0);
            ContentPanel.Children.Add(tapItAdView.ViewControl);

            //attached events
            tapItAdView.ControlLoaded += tapItAdView_loaded;
            tapItAdView.ContentLoaded += tapItAdView_LoadCompleted;
            tapItAdView.ErrorEvent += tapItAdView_ErrorEvent;
            tapItAdView.Navigating += tapItAdView_navigating;
            tapItAdView.Navigated += tapItAdView_navigated;
            tapItAdView.NavigationFailed += tapItAdView_navigationFailed;
        }

        void tapItAdView_navigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("tapItAdView_navigationFailed");
        }

        void tapItAdView_navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("tapItAdView_navigated");
        }

        void tapItAdView_navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("tapItAdView_navigating");
        }

        private async void tapItAdView_loaded(object sender, RoutedEventArgs e)
        {
            bool display = await tapItAdView.Load();
        }

        void tapItAdView_ErrorEvent(string strErrorMsg)
        {
            Debug.WriteLine("tapItAdView_ErrorEvent :" + strErrorMsg);
        }

        void tapItAdView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("tapItAdView_LoadCompleted");
            progressring.Visibility = Visibility.Collapsed;
            tapItAdView.Visible = Visibility.Visible;
        }

        #endregion

        private void HideBtn_Click(object sender, RoutedEventArgs e)
        {
            tapItAdView.Visible = Visibility.Collapsed;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}