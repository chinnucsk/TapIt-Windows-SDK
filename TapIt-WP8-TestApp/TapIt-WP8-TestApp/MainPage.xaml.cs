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

            tapItAdView = new AdView();
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
        private async void Banner_Ad_Click(object sender, RoutedEventArgs e)
        {
            object obj = ContentPanel.FindName("TapItAdViewControl");
            if (obj != null)
                return; // Ad View already added.

            //tapItAdView.BaseURL = "http://ec2-107-20-3-62.compute-1.amazonaws.com/~chetanch/adrequest.php";
            tapItAdView.Height = 80;
            int tempWidth = tapItAdView.Width;
            tapItAdView.Margin = new Thickness(0, 400, 0, 0);
            ContentPanel.Children.Add(tapItAdView.ViewControl);
            tapItAdView.PageLoaded += tapItAdView_PageLoaded;
            bool display = await tapItAdView.Show();
        }

        void tapItAdView_PageLoaded(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("tapItAdView_PageLoaded");
        }

        #endregion

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