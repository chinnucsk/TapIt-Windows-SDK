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
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;

namespace TapIt_WP8_TestApp
{
    public partial class AdPromptPage : PhoneApplicationPage
    {
        #region DataMember

        AdPromptView _AdPromptView;

        #endregion

        #region Constructor

        public AdPromptPage()
        {
            InitializeComponent();

            //initialize view
            _AdPromptView = new AdPromptView();

            _AdPromptView.Visible = Visibility.Collapsed;
            //_AdPromptView.ZoneId = 15501; //zone id for local server
            _AdPromptView.ZoneId = 25254;//zone id for TapIt server

           // LayoutRoot.Children.Add(_AdPromptView.ViewControl);

            //attached events
            _AdPromptView.ControlLoaded += _AdPromptView_loaded;
            _AdPromptView.ContentLoaded += _AdPromptView_LoadCompleted;
            _AdPromptView.ErrorEvent += _AdPromptView_ErrorEvent;
        }

        #endregion

        #region Events

        //orientation change event
        private void DeviceOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            _AdPromptView.DeviceOrientationChanged(e.Orientation);
        }

        ///<summary>
        /// //this event is fired when contents are loaded
        ///</summary>
        private void _AdPromptView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("AdPrompt Load Completed");
            progressring.Visibility = Visibility.Collapsed;
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
            progressring.Visibility = Visibility.Collapsed;
            MessageBox.Show(strErrorMsg);
        }

        private void PhoneApplicationPage_BackKeyPress_1(object sender, CancelEventArgs e)
        {
            
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            progressring.Visibility = Visibility.Visible;
            _AdPromptView.Load();
        }

        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            progressring.Visibility = Visibility.Collapsed;
            _AdPromptView.Visible = Visibility.Visible;
        }

        #endregion

    }
}