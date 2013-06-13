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
        #region Constructor

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        ///   //banner ad click
        /// </summary>
        private void BannerAd_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/BannerAdPage.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        ///   //interstitial ad click
        /// </summary>
        private void InterstitialAd_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/InterstitialAdPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AlertAd_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AlertAdPage.xaml", UriKind.RelativeOrAbsolute));
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