using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TapIt_Win8;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TapIt_Win8_TestApp
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BannerAdPage : Page
    {
        #region DataMember

        BannerAdView _bannerAdView;

        #endregion

        #region Constructor

        public BannerAdPage()
        {
            this.InitializeComponent();

            //// Set the event handler for when the application deactivated
            //(Application.Current as TapIt_W8_TestApp.App).App_Deactivated +=
            //              new EventHandler(BannerAdPage_AppDeactivated);

            //// Set the event handler for when the application activated
            //(Application.Current as TapIt_W8_TestApp.App).App_Activated +=
            //              new EventHandler(BannerAdPage_AppActivated);

            //Initialize the view
            _bannerAdView = new BannerAdView();

            //_bannerAdView.ZoneId = 25252;//2720;          //zone id for TapIt
            _bannerAdView.ZoneId = 14702;                  //zone id for local server
            //_bannerAdView.ZoneId = 29318;
            _bannerAdView.Visible = Visibility.Collapsed;
            _bannerAdView.ViewControl.SetValue(Grid.RowProperty, 2);
            _bannerAdView.Mode = ModeType.live;
            
            ContentPanel.Children.Add(_bannerAdView.ViewControl);

            //_bannerAdView.UrlAdditionalParameters["param1"] = "value1";
            //_bannerAdView.UrlAdditionalParameters["param2"] = "value2";
            //_bannerAdView.UrlAdditionalParameters["param3"] = "value3";

            //attached events
            _bannerAdView.ControlLoaded += _bannerAdView_controlLoaded;
            _bannerAdView.ContentLoaded += _bannerAdView_contentLoaded;
            _bannerAdView.ErrorEvent += _bannerAdView_errorEvent;
            _bannerAdView.NavigationFailed += _bannerAdView_NavigationFailed;
          
            //_bannerAdView.SetBannerAdSize(BannerAdView.BannerAdtype.Banners);
        }

        #endregion

        #region Methods

        private async void MessagePrompt(string message)
        {
            MessageDialog messageDialog = new MessageDialog(message);

            messageDialog.Commands.Add(new UICommand("Ok",
                new UICommandInvokedHandler(this.OkCommandInvokedHandler)));

            messageDialog.DefaultCommandIndex = 0;

            messageDialog.CancelCommandIndex = 1;

            await messageDialog.ShowAsync();
        }

        private void OkCommandInvokedHandler(IUICommand command)
        {

        }

        #endregion

        #region Events

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        ///<summary>
        /// //this event is fired when control is loaded
        ///</summary>
        private void _bannerAdView_controlLoaded(object sender, RoutedEventArgs e)
        {

        }

        void _bannerAdView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            Debug.WriteLine("_bannerAdView_navigationFailed");
            MessagePrompt("_bannerAdView_navigationFailed");
        }

        ///<summary>
        /// //this event is fired when error occurs
        ///</summary>
        void _bannerAdView_errorEvent(string strErrorMsg)
        {
            Debug.WriteLine("_bannerAdView_ErrorEvent :" + strErrorMsg);
            ProgressRing1.Visibility = Visibility.Collapsed;
            MessagePrompt(strErrorMsg);
        }

        ///<summary>
        /// //this event is fired when contents are loaded
        ///</summary>
        void _bannerAdView_contentLoaded(object sender, NavigationEventArgs e)
        {
            MessagePrompt("_bannerAdView_LoadCompleted");
            ProgressRing1.Visibility = Visibility.Collapsed;
        }

        private void BackBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Visible;
            _bannerAdView.SetBannerAdSize(BannerAdView.BannerAdtype.LeaderBoard);
            Task<bool> disply = _bannerAdView.Load();
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Collapsed;
            _bannerAdView.Visible = Visibility.Visible;
        }

        private void hideBtn_Click(object sender, RoutedEventArgs e)
        {
            _bannerAdView.Visible = Visibility.Collapsed;
        }

        private void installBtn_Click(object sender, RoutedEventArgs e)
        {
            Tracker tracker = Tracker.Instance;
            Task<bool> isInstalled = tracker.ReportInstall("offer_txt");
        }

        private void midRectBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Visible;
            _bannerAdView.SetBannerAdSize(BannerAdView.BannerAdtype.MediumRect);
            Task<bool> disply = _bannerAdView.Load();
        }

        private void bannerBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Visible;
            _bannerAdView.SetBannerAdSize(BannerAdView.BannerAdtype.Banners);
            Task<bool> disply = _bannerAdView.Load();
        }

        #endregion
    }
}
