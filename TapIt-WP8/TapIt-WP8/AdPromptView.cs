using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TapIt_WP8
{
    public class AdPromptView : AdViewBase
    {
        #region Datamember

        Popup _adPrompt;
        Border _border;
        StackPanel _MainPanel;
        StackPanel _stackPanel;
        Button _closeBtn;
        Button _callTocationBtn;
        TextBlock _titleBlk;

        #endregion

        #region Property

        //Show th ad prompt
        public override Visibility Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;

                if (Visibility.Visible == value)
                    ShowAdPrompt();
            }
        }

        // return control to add in UI tree
        public FrameworkElement ViewControl
        {
            get { return _adPrompt; }
        }

        #endregion

        #region Constructor

        public AdPromptView()
        {
            //create dynamic popUp control
            CreatePopUpControl();

            //set size to popup
            SetAdSize(Calculateheight(), CalculateWidth());

            SetAdType();

            //for orientation change
            SetScreenSize();
        }

        #endregion

        #region Abstract methods

        protected override void SetAdType()
        {
            Adtype = AdType.Ad_Prompt;
        }
       
        protected override void SetAdSize(int height, int width)
        {
             //Set where the popup will show up on the screen.
            _adPrompt.VerticalOffset = height;
            _adPrompt.HorizontalOffset = width;
        }

        #endregion

        #region Methods

        //popUp width
        private int CalculateWidth()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            int width = 0;
            width = (deviceData.ScreenWidth) / 4;
            return width;
        }

        //popUp height
        private int Calculateheight()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            int height = 0;
            height = (deviceData.ScreenWidth * 2) / 4;
            return height;
        }

        private void CreatePopUpControl()
        {
            // Create the popup object.
            _adPrompt = new Popup();

            // events
            _adPrompt.Loaded += _adprompt_ControlLoaded;

            // Create some content to show in the popup. Typically you would 
            // create a user control.
            _border = new Border();
            _border.BorderBrush = new SolidColorBrush(Colors.Black);
            _border.BorderThickness = new Thickness(2.0);

            _MainPanel = new StackPanel();
            _MainPanel.Background = new SolidColorBrush(Color.FromArgb(255, 0, 110, 200));

            _stackPanel = new StackPanel();
            _stackPanel.Background = new SolidColorBrush(Colors.White);
            _stackPanel.Orientation = Orientation.Horizontal;

            _closeBtn = new Button();
            _closeBtn.BorderBrush = new SolidColorBrush(Colors.Black);
          //  _closeBtn.Content = "close"; // temp code

            _closeBtn.Margin = new Thickness(15, 40, 15, 40);
            _closeBtn.Background = new SolidColorBrush(Color.FromArgb(255, 0, 110, 200));
            _closeBtn.Click += _closeBtn_Click;

            _callTocationBtn = new Button();
            _callTocationBtn.BorderBrush = new SolidColorBrush(Colors.Black);
            //_callTocationBtn.Content = "ok"; // temp code

            _callTocationBtn.Background = new SolidColorBrush(Color.FromArgb(255, 0, 110, 200));
            _callTocationBtn.Margin = new Thickness(15, 40, 15, 40);
            _callTocationBtn.Click += _callTocationBtn_Click;

            _titleBlk = new TextBlock();
           // _titleBlk.Text = "this is test app"; // temp code

            _titleBlk.Margin = new Thickness(10);
            _titleBlk.FontSize = 32;
            _titleBlk.Foreground = new SolidColorBrush(Colors.White);
            _titleBlk.HorizontalAlignment = HorizontalAlignment.Center;

            _stackPanel.Children.Add(_closeBtn);
            _stackPanel.Children.Add(_callTocationBtn);
            _MainPanel.Children.Add(_titleBlk);
            _MainPanel.Children.Add(_stackPanel);
            _border.Child = _MainPanel;

            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            _adPrompt.Child = _border;
        }

        private void ShowAdPrompt()
        {
            if (JsonResponse != null)
            {
                _closeBtn.Content = JsonResponse.declinestring;

                _titleBlk.Text = JsonResponse.adtitle;

                _callTocationBtn.Content = JsonResponse.calltoaction;

                // Open the popup.
                _adPrompt.IsOpen = true;
            }
            //_adPrompt.IsOpen = true;
        }
        
        public override async Task<bool> Load()
        {
            bool retVal = await base.Load();

            if (retVal)
            {
                Debug.WriteLine("_adPrompt_ContentLoaded");

                base.OnContentLoad(this, null);
            }

            return retVal;
        }

        //for orientation chane event
        public override async void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            
            base.DeviceOrientationChanged(pageOrientation);
            SetScreenSize();
           await Load();
        }

        private void SetScreenSize()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            if (PageOrientation.LandscapeRight == deviceData.PageOrientation ||
                   PageOrientation.LandscapeLeft == deviceData.PageOrientation)
            {
                SetAdSize(CalculateWidth(), Calculateheight());
            }
            else if (PageOrientation.PortraitDown == deviceData.PageOrientation ||
               PageOrientation.PortraitUp == deviceData.PageOrientation)
            {
                SetAdSize(Calculateheight(), CalculateWidth());
            }
        }

        public void OnBackKeypressed(bool cancle)
        {
            if (false == cancle)
                _adPrompt.IsOpen = false;
        }

        #endregion

        #region Events

        void _adprompt_ControlLoaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("_adPrompt_ControlLoaded");

            base.OnControlLoad(sender, e);
        }

        //PopUp button Clicks event
        void _callTocationBtn_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask browserTask = new WebBrowserTask();
            browserTask.Uri =new Uri(JsonResponse.clickUrl);
            browserTask.Show();
        }

        void _closeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            _adPrompt.IsOpen = false;
        }

        #endregion

    }
}
