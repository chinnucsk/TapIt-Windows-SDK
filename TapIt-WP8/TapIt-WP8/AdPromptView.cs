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

        private Popup _adPrompt;
        private Border _border;
        private StackPanel _mainPanel;
        private StackPanel _stackPanel;
        private Button _closeBtn;
        private Button _okBtn;
        private TextBlock _titleBlock;
        private Color _borderColor = Colors.Black;
        private Color _mainPanelBackground = Color.FromArgb(255, 0, 110, 200);
        private Color _stackPanelColor = Colors.White;
        private Color _closeBtnColor = Colors.Black;
        private Color _okBtnColor = Colors.Black;
        private Color _closeBtnBackground = Color.FromArgb(255, 0, 110, 200);
        private Color _okBtnBackground = Color.FromArgb(255, 0, 110, 200);
        private Thickness _borderMargin = new Thickness(2.0);
        private Thickness _closeBtnMargin = new Thickness(15, 40, 15, 40);
        private Thickness _okBtnMargin = new Thickness(15, 40, 15, 40);
        private Thickness _titleMargin = new Thickness(10);
        private Color _titleForeground = Colors.White;
        private double _titleFont = 32;

        #endregion

        #region Property

        public double TitleFont
        {
            get { return _titleFont; }
            set { _titleFont = value; }
        }

        public Color TitleForeground
        {
            get { return _titleForeground; }
            set { _titleForeground = value; }
        }

        public Thickness TitleMargin
        {
            get { return _titleMargin; }
            set { _titleMargin = value; }
        }

        public Thickness OkBtnMargin
        {
            get { return _okBtnMargin; }
            set { _okBtnMargin = value; }
        }

        public Thickness CloseBtnMargin
        {
            get { return _closeBtnMargin; }
            set { _closeBtnMargin = value; }
        }

        public Thickness BorderMargin
        {
            get { return _borderMargin; }
            set { _borderMargin = value; }
        }

        public Color OkBtnBackground
        {
            get { return _okBtnBackground; }
            set { _okBtnBackground = value; }
        }

        public Color CloseBtnBackground
        {
            get { return _closeBtnBackground; }
            set { _closeBtnBackground = value; }
        }

        public Color OkBtnColor
        {
            get { return _okBtnColor; }
            set { _okBtnColor = value; }
        }

        public Color CloseBtnColor
        {
            get { return _closeBtnColor; }
            set { _closeBtnColor = value; }
        }

        public Color StackPanelColor
        {
            get { return _stackPanelColor; }
            set { _stackPanelColor = value; }
        }

        public Color MainPanelColorBackground
        {
            get { return _mainPanelBackground; }
            set { _mainPanelBackground = value; }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        //Show the AdPrompt
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
            // Create PopUp control
            CreatePopUpControl();

            // Set the AdPromt position.
            SetAdPromptPosition(CalculateYPos(), CalculateXPos());

            SetAdType();

            SetAdSize(0, 0);

            // Consider Orientation
            SetAdPromptPosition();
        }

        #endregion

        #region Abstract methods

        protected override void SetAdType()
        {
            Adtype = AdType.Ad_Prompt;
        }

        private void SetAdPromptPosition(int y, int x)
        {
            // Set the AdPromt position.
            _adPrompt.VerticalOffset = y;
            _adPrompt.HorizontalOffset = x;
        }

        protected override void SetAdSize(int height, int width)
        {
            // NO code here as AdPrompt size is adjusted as per the content.
        }

        #endregion

        #region Methods

        // PopUp X Pos
        private int CalculateXPos()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            int x = 0;
            x = (deviceData.ScreenWidth) / 4;
            return x;
        }

        // PopUp Y Pos
        private int CalculateYPos()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            int y = 0;
            y = (deviceData.ScreenHeight) / 3;
            return y;
        }

        private void CreatePopUpControl()
        {
            // Create the popup object.
            _adPrompt = new Popup();

            // events
            _adPrompt.Loaded += _adprompt_ControlLoaded;

            // Create content to show in the popup.
            _border = new Border();
            _border.BorderBrush = new SolidColorBrush(BorderColor);
            _border.BorderThickness = BorderMargin;

            _mainPanel = new StackPanel();
            _mainPanel.Background = new SolidColorBrush(MainPanelColorBackground);

            _stackPanel = new StackPanel();
            _stackPanel.Background = new SolidColorBrush(StackPanelColor);
            _stackPanel.Orientation = Orientation.Horizontal;

            _closeBtn = new Button();
            _closeBtn.BorderBrush = new SolidColorBrush(CloseBtnColor);
            // _closeBtn.Content = "close"; // temp code

            _closeBtn.Margin = CloseBtnMargin;
            _closeBtn.Background = new SolidColorBrush(CloseBtnBackground);
            _closeBtn.Click += _closeBtn_Click;

            _okBtn = new Button();
            _okBtn.BorderBrush = new SolidColorBrush(OkBtnColor);
            // _callTocationBtn.Content = "ok"; // temp code

            _okBtn.Background = new SolidColorBrush(OkBtnBackground);
            _okBtn.Margin = OkBtnMargin;
            _okBtn.Click += _callTocationBtn_Click;

            _titleBlock = new TextBlock();
            // _titleBlk.Text = "this is test app"; // temp code

            _titleBlock.Margin = TitleMargin;
            _titleBlock.FontSize = TitleFont;
            _titleBlock.Foreground = new SolidColorBrush(TitleForeground);
            _titleBlock.HorizontalAlignment = HorizontalAlignment.Center;

            _stackPanel.Children.Add(_closeBtn);
            _stackPanel.Children.Add(_okBtn);
            _mainPanel.Children.Add(_titleBlock);
            _mainPanel.Children.Add(_stackPanel);
            _border.Child = _mainPanel;

            _adPrompt.Child = _border;
        }

        private void ShowAdPrompt()
        {
            if (JsonResponse != null)
            {
                _closeBtn.Content = JsonResponse.declinestring;

                _titleBlock.Text = JsonResponse.adtitle;

                _okBtn.Content = JsonResponse.calltoaction;

                // Open the popup.
                _adPrompt.IsOpen = true;
            }
            else
            {
                OnError(TapItResource.AdPromptData);
            }

            _adPrompt.IsOpen = true; // temp code - to be removed.
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

        // for Orientation change event
        public override void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
            SetAdPromptPosition();
        }

        private void SetAdPromptPosition()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            if (PageOrientation.LandscapeRight == deviceData.PageOrientation ||
                   PageOrientation.LandscapeLeft == deviceData.PageOrientation)
            {
                SetAdPromptPosition(CalculateXPos(), CalculateYPos());
            }
            else if (PageOrientation.PortraitDown == deviceData.PageOrientation ||
               PageOrientation.PortraitUp == deviceData.PageOrientation)
            {
                SetAdPromptPosition(CalculateYPos(), CalculateXPos());
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

        // AdPrompt button click events
        void _callTocationBtn_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask browserTask = new WebBrowserTask();
            browserTask.Uri = new Uri(JsonResponse.clickUrl);
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
