using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TapIt_WP8
{
    public class AlertAdView : AdView
    {
        #region Datamember

        Popup _alertpopUp;

        #endregion

        #region Property

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

        #endregion


        #region Constructor

        public AlertAdView()
        {
            SetAdType();
        }

        #endregion

        #region Abstract methods

        protected override void SetAdType()
        {
            Adtype = AdType.Ad_Prompt;
        }

        protected override void SetAdSize(int height, int width)
        {
        }

        #endregion

        #region Methods

        private void ShowAdPrompt()
        {
            // Create the popup object.
            _alertpopUp = new Popup();

            // Create some content to show in the popup. Typically you would 
            // create a user control.
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(2.0);

            StackPanel _MainPanel = new StackPanel();
            _MainPanel.Background = new SolidColorBrush(Color.FromArgb(255, 0, 110, 200));

            StackPanel _stackPanel = new StackPanel();
            _stackPanel.Background = new SolidColorBrush(Colors.White);
            _stackPanel.Orientation = Orientation.Horizontal;

            Button _closeBtn = new Button();
            _closeBtn.BorderBrush = new SolidColorBrush(Colors.Black);
            _closeBtn.Content = JsonResponse.declinestring;
            _closeBtn.Margin = new Thickness(15, 30, 0, 30);
            _closeBtn.Background = new SolidColorBrush(Color.FromArgb(255, 0, 110, 200));
            _closeBtn.Click += _closeBtn_Click;

            Button _callTocationBtn = new Button();
            _callTocationBtn.BorderBrush = new SolidColorBrush(Colors.Black);
            _callTocationBtn.Content = JsonResponse.calltoaction;
            _callTocationBtn.Background = new SolidColorBrush(Color.FromArgb(255, 0, 110, 200));
            _callTocationBtn.Margin = new Thickness(15, 30, 15, 30);
            _callTocationBtn.Click += _callTocationBtn_Click;

            TextBlock _titleBlk = new TextBlock();
            _titleBlk.Text = JsonResponse.adtitle;
            _titleBlk.Margin = new Thickness(10.0);
            _titleBlk.FontSize = 32;
            _titleBlk.HorizontalAlignment = HorizontalAlignment.Center;

            _stackPanel.Children.Add(_closeBtn);
            _stackPanel.Children.Add(_callTocationBtn);
            _MainPanel.Children.Add(_titleBlk);
            _MainPanel.Children.Add(_stackPanel);
            border.Child = _MainPanel;

            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            _alertpopUp.Child = border;


            //Maingrid.Children.Add(AlertpopUp);
            //AlertpopUp.VerticalAlignment = VerticalAlignment.Center;
            //AlertpopUp.HorizontalAlignment = HorizontalAlignment.Center;

            // Set where the popup will show up on the screen.
            _alertpopUp.VerticalOffset = 350;
            _alertpopUp.HorizontalOffset = 60;

            // Open the popup.
            _alertpopUp.IsOpen = true;
        }

        void _callTocationBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        void _closeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            _alertpopUp.IsOpen = false;
        }

        #endregion
    }
}
