using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TapIt_WP8
{
    public class InterstitialAdView : AdView
    {
        #region constants

        public const int _intestitialAdHeight = 480, _intestitialAdWidth = 320;

        #endregion

        #region Data member

        private Button _closeBtn;

        private int _animationTimeInterval = 10;
        private int _animationDuration = 3;

        private DispatcherTimer _animationTimer = new DispatcherTimer();
        private Storyboard _storyboard = new Storyboard();
        private DoubleAnimation _doubleAnimation = new DoubleAnimation();
        private PlaneProjection _planeProjection = new PlaneProjection();


        #endregion

        #region Constructor

        public InterstitialAdView()
        {
            SetAdType();
            SetAdSize(_intestitialAdHeight, _intestitialAdWidth);
            AddCloseButton();
           // RotateinterstitialAd();
        }

        #endregion

        #region Property

        public int AnimationTimeInterval
        {
            get { return _animationTimeInterval; }
            set
            {
                // todo: need to validate value
                _animationTimeInterval = value;
                _animationTimer.Interval = new TimeSpan(0, 0, value);
            }
        }

        public int AnimationDuration
        {
            get { return _animationDuration; }
            set
            {
                // todo: need to validate value
                _animationDuration = value;
                _doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(value));
            }
        }

        public override int Width
        {
            get { return base.Width; }
            set
            {
                // in case of Interstitial Ad the width should be equal to screen width
                // ignore the "value"
                base.Width = GetOrientationWidth();
            }
        }

        public override int Height
        {
            get { return base.Height; }
            set
            {
                // in case of Interstitial Ad the height should be equal to screen height
                // ignore the "value"
                base.Height = GetOrientationHeight();
            }
        }

        #endregion

        #region Abstract Methods

        protected override void SetAdType()
        {
            Adtype = AdType.Interstitial_Ad;
        }

        protected override void SetAdSize(int height, int width)
        {
            AdHeight = height;
            AdWidth = width;
            SetSizeToScreen(true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// animation for banner ad
        /// </summary>
        private void RotateinterstitialAd()
        {
            _animationTimer.Tick += dispatcherTimer_Tick;
            _animationTimer.Interval = new TimeSpan(0, 0, AnimationTimeInterval);

            _doubleAnimation.From = -90;
            _doubleAnimation.To = 0;
            _doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration));

            WebBrowser.Projection = _planeProjection;
            Storyboard.SetTarget(_doubleAnimation, WebBrowser.Projection);
            Storyboard.SetTargetProperty(_doubleAnimation, new PropertyPath(PlaneProjection.RotationYProperty));
            _storyboard.Children.Add(_doubleAnimation);

            _animationTimer.Start();

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _storyboard.Begin();
        }
        private void AddCloseButton()
        {
            _closeBtn = new Button();
            _closeBtn.Height = 80;
            _closeBtn.Width = 80;
            _closeBtn.BorderThickness = new System.Windows.Thickness(0);
            _closeBtn.Click += _closeBtn_Click;
            _closeBtn.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Images/interstitial_close_button@2x.png", UriKind.RelativeOrAbsolute)) };
            _closeBtn.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            _closeBtn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Maingrid.Children.Add(_closeBtn);
        }

        public override async void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
            SetSizeToScreen(true);
            await Load();
        }

        #endregion

        #region Events

        void _closeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Maingrid.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
