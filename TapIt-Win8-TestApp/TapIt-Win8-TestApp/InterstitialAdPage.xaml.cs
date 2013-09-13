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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TapIt_Win8_TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InterstitialAdPage : Page
    {
        #region DataMember

        InterstitialAdView _interstitialAdView;

        #endregion

        #region Constructor

        public InterstitialAdPage()
        {
            this.InitializeComponent();

            //Initialize the view
            _interstitialAdView = new InterstitialAdView();

            _interstitialAdView.Visible = Visibility.Collapsed;
            //_interstitialAdView.ZoneId = 30647;   //zone id for TapIt
            _interstitialAdView.ZoneId = 30647;     ////zone id for local server

            LayoutRoot.Children.Add(_interstitialAdView.ViewControl);
            //SystemTray.SetIsVisible(this, false);

            // attached events
            _interstitialAdView.ControlLoaded += _interstitialAdView_ControlLoaded;
            _interstitialAdView.ContentLoaded += _interstitialAdView_ContentLoaded;
            _interstitialAdView.ErrorEvent += _interstitialAdView_ErrorEvent;
            _interstitialAdView.NavigationFailed += _interstitialAdView_NavigationFailed;
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
        void _interstitialAdView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            Debug.WriteLine(" _interstitialAdView_NavigationFailed");
            MessagePrompt("_interstitialAdView_NavigationFailed");
        }

        #endregion

        #region Events

        void _interstitialAdView_ErrorEvent(string errorMsg)
        {
            Debug.WriteLine("_interstitialAdView_ErrorEvent :" + errorMsg);
            ProgressRing1.Visibility = Visibility.Collapsed;
            MessagePrompt(errorMsg);
        }

        void _interstitialAdView_ContentLoaded(object sender, NavigationEventArgs e)
        {
            MessagePrompt("_interstitialAdView_LoadCompleted");
            ProgressRing1.Visibility = Visibility.Collapsed;
        }

        void _interstitialAdView_ControlLoaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Visible;
            Task<bool> display = _interstitialAdView.Load();
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            _interstitialAdView.Visible = Visibility.Visible;
            ProgressRing1.Visibility = Visibility.Collapsed;
        }

        private void BackBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        #endregion
    }
}
