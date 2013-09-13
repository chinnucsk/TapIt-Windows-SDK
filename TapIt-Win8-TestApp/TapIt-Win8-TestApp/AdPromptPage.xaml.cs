using System;
using System.Collections.Generic;
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
    public sealed partial class AdPromptPage : Page
    {
        #region DataMember

        AdPromptView _AdPromptView;

        #endregion

        #region Constructor

        public AdPromptPage()
        {
            this.InitializeComponent();


            //initialize view
            _AdPromptView = new AdPromptView();

            _AdPromptView.Visible = Visibility.Collapsed;

            _AdPromptView.ZoneId = 15025; //zone id for local server

            _AdPromptView.ControlLoaded += _AdPromptView_loaded;
            _AdPromptView.ContentLoaded += _AdPromptView_LoadCompleted;
            _AdPromptView.ErrorEvent += _AdPromptView_ErrorEvent;
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

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Visible;
            Task<bool> display = _AdPromptView.Load();
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Collapsed;
            _AdPromptView.Visible = Visibility.Visible;
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

        private void BackBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        #endregion

        #region Methods

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

        #endregion
    }
}
