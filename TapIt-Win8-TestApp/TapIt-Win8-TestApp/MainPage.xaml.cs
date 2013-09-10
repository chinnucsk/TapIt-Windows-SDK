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

namespace TapIt_W8_TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void bannerAdBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BannerAdPage));
        }

        private void adPromptBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdPromptPage));
        }

        ///<summary>
        /// //this event is fired when contents are loaded
        ///</summary>
        private void _AdPromptView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            MessagePrompt("AdPrompt Load Completed");
            ProgressRing1.Visibility = Visibility.Collapsed;
        }

        ///<summary>
        /// //this event is fired when control is loaded
        ///</summary>
        private void _AdPromptView_loaded(object sender, RoutedEventArgs e)
        {
        }

        ///<summary>
        /// //this event is fired when error occurs
        ///</summary>
        void _AdPromptView_ErrorEvent(string strErrorMsg)
        {
            ProgressRing1.Visibility = Visibility.Collapsed;
            MessagePrompt(strErrorMsg);
        }


        private async void MessagePrompt(string message)
        {
            // Create the message dialog and set its content
            MessageDialog messageDialog = new MessageDialog(message);

            messageDialog.Commands.Add(new UICommand("Ok",
                new UICommandInvokedHandler(this.OkCommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private void OkCommandInvokedHandler(IUICommand command)
        {

        }

    }
}
