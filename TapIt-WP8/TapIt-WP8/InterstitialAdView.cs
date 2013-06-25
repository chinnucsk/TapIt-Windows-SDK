using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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
        private bool _isSystemTray = false;

        #endregion

        #region Constructor

        public InterstitialAdView()
        {
            SetAdType();
            SetAdSize(_intestitialAdHeight, _intestitialAdWidth);
            AddCloseButton();
            RotateinterstitialAd();
        }

        #endregion

        #region Property

        public override Visibility Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                if (Visibility.Visible == value)
                {
                    RotateinterstitialAd();
                    //_isSystemTray = SystemTray.IsVisible;
                    //SystemTray.IsVisible = false;
                }
                else
                {
                    //SystemTray.IsVisible = _isSystemTray;
                }
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
            Storyboard _storyBourd = new Storyboard();
            DoubleAnimation _doubleAnimation = new DoubleAnimation();
            _doubleAnimation.From = 90;
            _doubleAnimation.To = 0;
            PlaneProjection projection1 = new PlaneProjection();
            Maingrid.Projection = projection1;
            _doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.25));
            Storyboard.SetTarget(_doubleAnimation, Maingrid.Projection);
            Storyboard.SetTargetProperty(_doubleAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
            _storyBourd.Children.Add(_doubleAnimation);
            _storyBourd.Seek(TimeSpan.FromSeconds(0.5));
            _storyBourd.Begin();
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

        public override void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
            SetSizeToScreen(true);
        }

        #endregion

        #region Events

        void _closeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Visible = Visibility.Collapsed;
        }

        #endregion
    }
}
