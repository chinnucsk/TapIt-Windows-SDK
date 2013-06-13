using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        #endregion

        #region Constructor

        public InterstitialAdView()
        {
            SetAdType();
            SetAdSize(_intestitialAdHeight, _intestitialAdWidth);
            AddCloseButton();
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

        void _closeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Maingrid.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Methods

        protected override void SetAdType()
        {
            Adtype = AdType.Interstitial_Ad;
        }

        protected override void SetAdSize(int height, int width)
        {
            Width = width;
            Height = height;
        }

        #endregion

    }
}
