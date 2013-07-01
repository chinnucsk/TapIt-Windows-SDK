using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private Grid _maingrid;
        private Grid _popUpGrid;
        private int _width = 0;
        private int _height = 0;

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
            set
            {
                _titleFont = value;
                _titleBlock.FontSize = value;
            }
        }

        public Color TitleForeground
        {
            get { return _titleForeground; }
            set
            {
                _titleForeground = value;
                _titleBlock.Foreground = new SolidColorBrush(value);
            }
        }

        public Thickness TitleMargin
        {
            get { return _titleMargin; }
            set
            {
                _titleMargin = value;
                _titleBlock.Margin = value;
            }
        }

        public Thickness OkBtnMargin
        {
            get { return _okBtnMargin; }
            set
            {
                _okBtnMargin = value;
                _okBtn.Margin = value;
            }
        }

        public Thickness CloseBtnMargin
        {
            get { return _closeBtnMargin; }
            set
            {
                _closeBtnMargin = value;
                _closeBtn.Margin = value;
            }
        }

        public Thickness BorderMargin
        {
            get { return _borderMargin; }
            set
            {
                _borderMargin = value;
                _border.BorderThickness = value;
            }
        }

        public Color OkBtnBackground
        {
            get { return _okBtnBackground; }
            set
            {
                _okBtnBackground = value;
                _okBtn.Background = new SolidColorBrush(value);
            }
        }

        public Color CloseBtnBackground
        {
            get { return _closeBtnBackground; }
            set
            {
                _closeBtnBackground = value;
                _closeBtn.Background = new SolidColorBrush(value);
            }
        }

        public Color OkBtnColor
        {
            get { return _okBtnColor; }
            set
            {
                _okBtnColor = value;
                _okBtn.BorderBrush = new SolidColorBrush(value);
            }
        }

        public Color CloseBtnColor
        {
            get { return _closeBtnColor; }
            set
            {
                _closeBtnColor = value;
                _closeBtn.BorderBrush = new SolidColorBrush(value);
            }
        }

        public Color StackPanelColor
        {
            get { return _stackPanelColor; }
            set
            {
                _stackPanelColor = value;
                _stackPanel.Background = new SolidColorBrush(value);
            }
        }

        public Color MainPanelColorBackground
        {
            get { return _mainPanelBackground; }
            set
            {
                _mainPanelBackground = value;
                _mainPanel.Background = new SolidColorBrush(value);
            }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                _border.BorderBrush = new SolidColorBrush(value);
            }
        }

        //Show the AdPrompt
        public override Visibility Visible
        {
            get { return _visible; }
            set
            {
                if (value == Visibility.Visible)
                {
                    if (IsAdDisplayed)
                    {
                        OnError(TapItResource.LoadNewAd);
                        return;
                    }

                    if (!IsAdLoaded)
                    {
                        IsInternalLoad = true;
                        Load();
                    }
                    else
                    {
                        _visible = value;
                        ShowAdPrompt();
                    }
                }
                else
                {
                    _visible = value;
                    _adPrompt.IsOpen = false;
                }
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

            SetAdType();

            SetAdSize(0, 0);
        }

        #endregion

        #region Abstract methods

        protected override void SetAdType()
        {
            Adtype = AdType.Ad_Prompt;
        }

        //private void SetAdPromptPosition(int y, int x)
        //{
        //    // Set the AdPromt position.
        //    //_adPrompt.VerticalOffset = y;
        //    //_adPrompt.HorizontalOffset = x;
        //}

        protected override void SetAdSize(int height, int width)
        {
            // No code here as AdPrompt size is adjusted as per the content.
        }

        #endregion

        #region Methods

        //// PopUp X Pos
        //private int CalculateXPos(int deviceWidth)
        //{
        //    int x = 0;
        //    x = (deviceWidth - _width) / 2;
        //    return x;
        //}

        //// PopUp Y Pos
        //private int CalculateYPos(int deviceHeight)
        //{
        //    int y = 0;
        //    y = (deviceHeight - _height) / 2;
        //    return y;
        //}

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
            //_okBtn.Content = "ok"; // temp code

            _okBtn.Background = new SolidColorBrush(OkBtnBackground);
            _okBtn.Margin = OkBtnMargin;
            _okBtn.Click += _okBtn_Click;

            _titleBlock = new TextBlock();
            // _titleBlk.Text = "this is test app"; // temp code

            DeviceDataMgr deviceData = DeviceDataMgr.Instance;

            _titleBlock.Margin = TitleMargin;
            _titleBlock.TextWrapping = TextWrapping.Wrap;
            _titleBlock.MaxWidth = (int)deviceData.ScreenWidth * 0.8;
            _titleBlock.FontSize = TitleFont;
            _titleBlock.Foreground = new SolidColorBrush(TitleForeground);
            _titleBlock.HorizontalAlignment = HorizontalAlignment.Center;

            _stackPanel.Children.Add(_closeBtn);
            _stackPanel.Children.Add(_okBtn);
            _mainPanel.Children.Add(_titleBlock);
            _mainPanel.Children.Add(_stackPanel);
            _border.Child = _mainPanel;

            _popUpGrid = new Grid();
            _popUpGrid.SizeChanged += _popUpGrid_SizeChanged;
            _popUpGrid.VerticalAlignment = VerticalAlignment.Center;
            _popUpGrid.HorizontalAlignment = HorizontalAlignment.Center;
            _popUpGrid.Children.Add(_border);

            _maingrid = new Grid();
            _maingrid.Background = new SolidColorBrush(Colors.Transparent);
            _maingrid.Children.Add(_popUpGrid);

            _adPrompt.Child = _maingrid;
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
                IsAdDisplayed = true;
            }
            else
            {
                OnError(TapItResource.AdPromptData);
            }
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
                //SetAdPromptPosition(CalculateYPos(deviceData.ScreenWidth), CalculateXPos(deviceData.ScreenHeight));

                _maingrid.Width = deviceData.ScreenHeight - (SystemTray.IsVisible ? SystemTrayWidthLandscape : 0);
                _maingrid.Height = deviceData.ScreenWidth;
            }
            else if (PageOrientation.PortraitDown == deviceData.PageOrientation ||
               PageOrientation.PortraitUp == deviceData.PageOrientation)
            {
                //SetAdPromptPosition(CalculateYPos(deviceData.ScreenHeight), CalculateXPos(deviceData.ScreenWidth));
                _maingrid.Width = deviceData.ScreenWidth;
                _maingrid.Height = deviceData.ScreenHeight - (SystemTray.IsVisible ? SystemTrayHeightPortrait : 0);
            }
        }

        public void OnBackKeypressed(CancelEventArgs e)
        {
            // Check if the PopUp window is open
            if (_adPrompt.IsOpen)
            {
                // Close the PopUp Window
                _adPrompt.IsOpen = false;
                e.Cancel = true;
            }
            else
            {
                // Popup is already closed 
                // navigating back from the current page
            }
        }

        #endregion

        #region Events

        void _popUpGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _width = (int)_popUpGrid.ActualWidth;
            _height = (int)_popUpGrid.ActualHeight;

            // Set the AdPromt position.
            SetAdPromptPosition();
            //_maingrid.Height = 800;
            //_maingrid.Width = 480;
        }

        void _adprompt_ControlLoaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("_adPrompt_ControlLoaded");

            base.OnControlLoad(sender, e);
        }

        // AdPrompt button click events
        void _okBtn_Click(object sender, RoutedEventArgs e)
        {
            _adPrompt.IsOpen = false;
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
