using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#if WINDOWS_PHONE
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
#elif WIN8
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
#endif


#if WINDOWS_PHONE
namespace TapIt_WP8
#elif WIN8
namespace TapIt_Win8
#endif
{
    public class BannerAdView : AdView
    {
        #region Constants

        //Banner ad size
#if WINDOWS_PHONE
        public const int _bannerHeight = 80, _bannerWidth = 320;
#elif WIN8
        public const int _bannerHeight = 90, _bannerWidth = 728;
#endif

        #endregion

        #region DataMembers

        //banner ad animation data members
        private int _animationTimeInterval = 60;
        private int _animationDuration = 3;
        private bool _IsLeftToRight = true;
        private DispatcherTimer _animationTimer = new DispatcherTimer();
        private Storyboard _storyboard = new Storyboard();
        private DoubleAnimation _doubleAnimation = new DoubleAnimation();
        private PlaneProjection _planeProjection = new PlaneProjection();

        #endregion

        #region Property

        protected override AdType Adtype
        {
            get
            {
                return AdType.Banner_Ad;
            }
        }

        public override int Width
        {
            get { return base.Width; }
            set
            {
                // in case of Banner Ad the width should be equal to screen width
                // ignore the "value"
                base.Width = GetOrientationWidth();
            }
        }

        public int AnimationTimeInterval
        {
            get { return _animationTimeInterval; }
            set
            {
                if (value > 0)
                {
                    _animationTimeInterval = value;
                    _animationTimer.Interval = new TimeSpan(0, 0, value);
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int AnimationDuration
        {
            get { return _animationDuration; }
            set
            {
                if (value > 0)
                {
                    _animationDuration = value;
                    _doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(value));
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Constructor

        public BannerAdView()
        {
            SetAdSize(_bannerWidth, _bannerHeight);
#if WINDOWS_PHONE
            RotateBannerAd();
#endif

        }

        #endregion

        #region methods

        /// <summary>
        /// set size for ad - to support multiple sizes for banner ad
        /// </summary>
        public void SetAdSize(int width, int height)
        {
            AdHeight = height;
            AdWidth = width;
            SetSizeToScreen();

            SetControlHeight(height);
        }

        private void SetControlHeight(int height)
        {
            Height = height + 2;
        }

#if WINDOWS_PHONE
        /// <summary>
        /// animation for banner ad
        /// </summary>
        private void RotateBannerAd()
        {
            _animationTimer.Tick += dispatcherTimer_Tick;
            _animationTimer.Interval = new TimeSpan(0, 0, AnimationTimeInterval);
            _doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration));

            WebBrowser.Projection = _planeProjection;
            Storyboard.SetTarget(_doubleAnimation, WebBrowser.Projection);
            Storyboard.SetTargetProperty(_doubleAnimation, new PropertyPath(PlaneProjection.RotationYProperty));
            _storyboard.Children.Add(_doubleAnimation);
            _storyboard.Completed += _storyboard_Completed;
            _animationTimer.Start();
        }

        /// <summary>
        /// orientation change 
        /// </summary>
        public override void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
            SetSizeToScreen();
        }
#endif

        public override async Task<bool> Load(bool bRaiseError = true)
        {
            bool retVal = false;
            try
            {
                retVal = await base.Load(bRaiseError);

                if (retVal)
                {
                    if (IsTimerInitiatedLoad)
                    {
                        IsAdLoadedPending = true;
                    }
                    else
                    {
                        NavigateToHtml();
                    }
                }
            }
            catch (Exception ex)
            {
                if (bRaiseError)
                    OnError("Error in Load(): " + ex);
            }

            return retVal;
        }

        #endregion

        #region Event

        void _storyboard_Completed(object sender, EventArgs e)
        {
            _IsLeftToRight = !_IsLeftToRight;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this.Visible == Visibility.Collapsed)
                return;

            if (IsAdLoadedPending)
            {
                NavigateToHtml();
            }
            else
            {
                IsTimerInitiatedLoad = true;
                Task<bool> b = Load(false);
            }

            _doubleAnimation.From = 360;
            _doubleAnimation.To = 0;

            _storyboard.Begin();
        }

        #endregion
    }
}
