using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace TapIt_WP8
{
    public class BannerAdView : AdView
    {
        #region Constants

        //Banner ad size
        public const int _bannerHeight = 80, _bannerWidtht = 320;

        #endregion

        #region DataMembers

        //banner ad animation data members
        private int _animationTimeInterval = 10;
        private int _animationDuration = 2;
        private bool _IsLeftToRight = true;
        private DispatcherTimer _animationTimer = new DispatcherTimer();
        private Storyboard _storyboard = new Storyboard();
        private DoubleAnimation _doubleAnimation = new DoubleAnimation();
        private PlaneProjection _planeProjection = new PlaneProjection();

        #endregion

        #region Property

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

        #endregion

        #region Constructor

        public BannerAdView()
        {
            SetAdType();
            SetAdSize(_bannerHeight, _bannerWidtht);
            RotateBannerAd();
        }

        #endregion

        #region methods

        /// <summary>
        /// set the banner ad type
        /// </summary>
        protected override void SetAdType()
        {
            Adtype = AdType.Banner_Ad;
        }

        /// <summary>
        /// set size for ad
        /// </summary>
        protected override void SetAdSize(int height, int width)
        {
            AdHeight = height;
            AdWidth = width;
            SetSizeToScreen();
        }

        /// <summary>
        /// animation for banner ad
        /// </summary>
        private void RotateBannerAd()
        {
            _animationTimer.Tick += dispatcherTimer_Tick;
            _animationTimer.Interval = new TimeSpan(0, 0, AnimationTimeInterval);
            //if (!isRotate)
            //{
            //    _doubleAnimation.From = 360;
            //    _doubleAnimation.To = 180;
            //    isRotate = true;
            //}
            //else
            //{
            //    _doubleAnimation.From = 180;
            //    _doubleAnimation.To = 0;
            //    isRotate = false;
            //}
            _doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration));

            WebBrowser.Projection = _planeProjection;
            Storyboard.SetTarget(_doubleAnimation, WebBrowser.Projection);
            Storyboard.SetTargetProperty(_doubleAnimation, new PropertyPath(PlaneProjection.RotationYProperty));
            _storyboard.Children.Add(_doubleAnimation);
            _storyboard.Completed += _storyboard_Completed;
            _animationTimer.Start();
        }

        void _storyboard_Completed(object sender, EventArgs e)
        {
            IsAdRotating = false;
            _IsLeftToRight = !_IsLeftToRight;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            bool doAnimation = false;
            if (IsAdLoadedPending)
            {
                NavigateToHtml();
                doAnimation = true;
            }

            if (_IsLeftToRight)
            {
                _doubleAnimation.From = 360;
                _doubleAnimation.To = 180;
                IsInternalLoad = true;
                doAnimation = true;
                Task<bool> b = Load();
            }
            else
            {
                _doubleAnimation.From = 180;
                _doubleAnimation.To = 0;
            }

            if (doAnimation)
            {
                IsAdRotating = true;
                _storyboard.Begin();
            }
        }

        /// <summary>
        /// orientation change 
        /// </summary>
        public override void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
            SetSizeToScreen();
        }

        #endregion
    }
}
