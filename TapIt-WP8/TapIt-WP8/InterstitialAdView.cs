using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows;
using System.Windows.Navigation;
#elif WIN8
using TapIt_Win8;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;

#endif

#if WINDOWS_PHONE
namespace TapIt_WP8
#elif WIN8
namespace TapIt_Win8
#endif
{
    public class InterstitialAdView : AdView
    {
        #region constants
#if WINDOWS_PHONE
        public const int _intestitialAdHeight = 480, _intestitialAdWidth = 320;
#elif WIN8
        public const int _intestitialAdHeight = 1024, _intestitialAdWidth = 728;
#endif

        #endregion

        #region Data member

        //private Button _closeBtn;
        private Image _closeBtn;

        #endregion

        #region Constructor

        public InterstitialAdView()
        {
            SetAdSize(_intestitialAdWidth, _intestitialAdHeight);
            AddCloseButton();
        }

        #endregion

        #region Property

        protected override AdType Adtype
        {
            get
            {
                return AdType.Interstitial_Ad;
            }
        }

        public override Visibility Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;

                if (Visibility.Visible == value)
                {
                    //RotateInterstitialAd();
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

        protected void SetAdSize(int width, int height)
        {
            AdHeight = height;
            AdWidth = width;
            SetSizeToScreen(true);
        }

        #endregion

        #region Methods

        public override async Task<bool> Load(bool bRaiseError = true)
        {
            bool retVal = false;
            try
            {
                retVal = await base.Load(bRaiseError);

                if (retVal)
                {
                    NavigateToHtml();
                }
            }
            catch (Exception ex)
            {
                if (bRaiseError)
                    OnError("Error in Load(): " + ex);
            }

            return retVal;
        }

#if WINDOWS_PHONE

        protected override void Navigating()
        {
            ViewControl.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// animation for banner ad
        /// </summary>
        private void RotateInterstitialAd()
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

#endif
        private void AddCloseButton()
        {
            _closeBtn = new Image();
            _closeBtn.VerticalAlignment = VerticalAlignment.Top;
            _closeBtn.HorizontalAlignment = HorizontalAlignment.Left;
            _closeBtn.Stretch = Stretch.None;

            Maingrid.Children.Add(_closeBtn);

#if WINDOWS_PHONE
            _closeBtn.Tap += _closeBtn_Tap;
            _closeBtn.Source = new BitmapImage(new Uri("/Images/interstitial_close_button@2x.png", UriKind.RelativeOrAbsolute));
#elif WIN8
            RowDefinition r1 = new RowDefinition();
            r1.Height = new GridLength(10, GridUnitType.Star);
            Maingrid.RowDefinitions.Add(r1);
            RowDefinition r2 = new RowDefinition();
            r2.Height = new GridLength(90, GridUnitType.Star);
            Maingrid.RowDefinitions.Add(r2);

            _closeBtn.SetValue(Grid.RowProperty, 0);
            WebBrowser.SetValue(Grid.RowProperty, 1);

            _closeBtn.Tapped += _closeBtn_Tapped;
            _closeBtn.Source = new BitmapImage(new Uri("ms-appx:///TapIt-Win8/Images/interstitial_close_button@2x.png", UriKind.Absolute));

            Maingrid.Background = new SolidColorBrush(Colors.Black);
#endif
        }

#if WINDOWS_PHONE
        public override void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
            SetSizeToScreen(true);
        }
#endif
        #endregion

        #region Events

#if WINDOWS_PHONE
        void _closeBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Visible = Visibility.Collapsed;
        }
#elif WIN8
        void _closeBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Visible = Visibility.Collapsed;
        }
#endif

        protected override void OnContentLoad(object sender, NavigationEventArgs e)
        {
            base.OnContentLoad(sender, e);
        }

        #endregion
    }
}
