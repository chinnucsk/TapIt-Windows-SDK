
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Storage;

namespace TapIt_WP8
{
    public abstract class AdView : AdViewBase
    {
        #region DataMembers

        private Grid _maingrid;
        private WebBrowser _webBrowser;

        private Thickness _margin;

        private string _htmlResponse = string.Empty; // get the string in html format

        #endregion

        #region EventsDecleration

        public event EventHandler<NavigatingEventArgs> Navigating;
        public event EventHandler<NavigationEventArgs> Navigated;
        public event NavigationFailedEventHandler NavigationFailed;

        #endregion

        #region Property

        protected Grid Maingrid
        {
            get { return _maingrid; }
        }

        public override Visibility Visible
        {
            get { return _visible; }
            set
            {
                if (value == Visibility.Visible)
                {
                    if (!IsAdLoaded)
                    {
                        IsInternalLoad = true;
                        Task<bool> b = Load();
                    }
                    else
                    {
                        _maingrid.Visibility = _visible = value;
                    }
                }
                else
                {
                    _maingrid.Visibility = _visible = value;
                }
            }
        }

        // return control to add in UI tree
        public FrameworkElement ViewControl
        {
            get { return _maingrid; }
        }

        protected WebBrowser WebBrowser
        {
            get { return _webBrowser; }
        }

        public override int Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                SetViewWidth(value);
            }
        }

        public override int Height
        {
            get { return base.Height; }
            set
            {
                base.Height = value;
                SetViewHeight(value);
            }
        }

        public Thickness Margin
        {
            get { return _margin; }
            set { _webBrowser.Margin = _margin = value; }
        }

        #endregion

        #region Constructor

        public AdView()
        {
            //initailization of grid
            _maingrid = new Grid();
            _maingrid.Name = TapItResource.BrowserName;

            //nitailization of browser control
            _webBrowser = new WebBrowser();

            //_webBrowser.Name = TapItResource.BrowserName;
            _webBrowser.Width = Width;
            _webBrowser.Height = Height;
            _webBrowser.Margin = Margin;

            //add webcontrol to grid
            _maingrid.Children.Add(_webBrowser);

            //events  for main grid
            _maingrid.SizeChanged += _maingrid_SizeChanged;

            //events  for web control
            _webBrowser.Loaded += _webBrowser_Loaded;
            _webBrowser.LoadCompleted += _webBrowser_LoadCompleted;
            _webBrowser.Navigating += _webBrowser_Navigating;
            _webBrowser.Navigated += _webBrowser_Navigated;
            _webBrowser.NavigationFailed += _webBrowser_NavigationFailed;
        }

        #endregion

        #region Events

        /// <summary>
        /// // Main grid size is determined by the user application. 
        /// // Resize the web browser to fit in to the main grid.
        /// </summary>
        void _maingrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int height = (int)Maingrid.ActualHeight;
            int width = (int)Maingrid.ActualWidth;

            _webBrowser.Height = height;
            _webBrowser.Width = width;

            if (JsonToHtml(GetOrientationWidth(), height)) // set new viewport
            {
                IsInternalLoad = true;
                NavigateToHtml();
            }
        }

        /// <summary>
        ///  // This event is fired when the web browser navigation fails.
        /// </summary>
        void _webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("_webBrowser_NavigationFailed :" + e.Exception.Message);

            if (NavigationFailed != null)
                NavigationFailed(sender, e);
        }

        /// <summary>
        ///  // This event is fired when the web browser is navigated to a url.
        /// </summary>
        void _webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Navigated");

            if (Navigated != null)
                Navigated(sender, e);
        }

        /// <summary>
        ///  // This event is fired when the web browser control is loaded.
        /// </summary>
        void _webBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Loaded");

            base.OnControlLoad(sender, e);
        }

        /// <summary>
        ///  // This event is fired when the web browser content loading is completed.
        /// </summary>
        private void _webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_webBrowser_LoadCompleted");

            base.OnContentLoad(sender, e);
        }

        protected virtual void OnNavigating()
        {
        }

        /// <summary>
        ///  // The event is fired when user clicks on the web browser.
        /// </summary>
        void _webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Navigating");
            OnNavigating();

            if (Navigating != null)
                Navigating(sender, e);

            if (NavigationServiceRef != null)
            {
               string encodedUri = WebUtility.UrlEncode(e.Uri.ToString());
               NavigationServiceRef.Navigate(new Uri(string.Format
                            ("/TapIt-WP8;component/Resources/InAppBrowserPage.xaml?myparameter1={0}",
                            encodedUri), UriKind.RelativeOrAbsolute));
            }
            else
            {
                OnError(TapItResource.ErrorMsg);
            }
            e.Cancel = true;
            //WebBrowserTask browserTask = new WebBrowserTask();
            //browserTask.Uri = e.Uri;
            //browserTask.Show();
        }

        #endregion

        #region Methods

        private void SetViewWidth(int value)
        {
            _maingrid.Width = value;
            _webBrowser.Width = value;
        }

        private void SetViewHeight(int value)
        {
            _maingrid.Height = value;
            _webBrowser.Height = value;
        }

        protected int GetOrientationWidth()
        {
            int width = 0;
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;

            if (PageOrientation.LandscapeRight == deviceData.PageOrientation ||
                   PageOrientation.LandscapeLeft == deviceData.PageOrientation)
            {
                width = deviceData.ScreenHeight - (SystemTray.IsVisible ? SystemTrayWidthLandscape : 0);
            }
            else if (PageOrientation.PortraitDown == deviceData.PageOrientation ||
                PageOrientation.PortraitUp == deviceData.PageOrientation)
            {
                width = deviceData.ScreenWidth;
            }

            return width;
        }

        protected int GetOrientationHeight()
        {
            int height = 0;
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;

            if (PageOrientation.LandscapeRight == deviceData.PageOrientation ||
                   PageOrientation.LandscapeLeft == deviceData.PageOrientation)
            {
                height = deviceData.ScreenWidth;
            }
            else if (PageOrientation.PortraitDown == deviceData.PageOrientation ||
                PageOrientation.PortraitUp == deviceData.PageOrientation)
            {
                height = deviceData.ScreenHeight - (SystemTray.IsVisible ? SystemTrayHeightPortrait : 0) - 1;
            }

            return height;
        }

        protected void SetSizeToScreen(bool isHeight = false)
        {
            SetViewWidth(GetOrientationWidth());

            if (isHeight)
                SetViewHeight(GetOrientationHeight());
        }

        public override async Task<bool> Load(bool bRaiseError = true)
        {
            bool retVal = false;
            try
            {
                retVal = await base.Load(bRaiseError);

                if (retVal)
                {
                    int ViewPortHeight = (int)WebBrowser.ActualHeight;
                    retVal = JsonToHtml(GetOrientationWidth(), (ViewPortHeight > 0 ? ViewPortHeight : GetOrientationHeight()));
                }
            }
            catch (Exception ex)
            {
                if (bRaiseError)
                    OnError("Error in Load()" + ex);
            }

            return retVal;
        }

        private bool JsonToHtml(int ViewPortWidth, int ViewPortHeight)
        {
            bool isLoaded = false;
            try
            {
                string response = JsonResponse.Html;
                JsonParser jsnParser = new JsonParser();
                _htmlResponse = jsnParser.WrapToHTML(response, null, null, JsonResponse,
                    ViewPortWidth, ViewPortHeight);
                Debug.WriteLine("html response :" + _htmlResponse);

                if (!string.IsNullOrEmpty(_htmlResponse))
                {
                    isLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in JsonToHtml() :" + ex.Message);
                OnError("Exception in JsonToHtml()", ex);
            }

            return isLoaded;
        }

        protected void NavigateToHtml()
        {
            WebBrowser.NavigateToString(_htmlResponse);
        }

        public override void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
        }

        ///<summary>
        /// Code to execute when the application is activated (brought to the foreground)
        ///</summary>
        public void AppActivated()
        {
            IsAppActived = true;
            if (PhoneApplicationService.Current.State["htmlResponse"] == null)
                return;

            _htmlResponse = PhoneApplicationService.Current.State["htmlResponse"] as string;
            WebBrowser.NavigateToString(_htmlResponse);
        }

        ///<summary>
        /// Code to execute when the application is deactivated (sent to background)
        ///</summary>
        public void AppDeactivated()
        {
            // If there is data in the application member variable...
            if (!string.IsNullOrEmpty(_htmlResponse))
            {
                // Store it in the State dictionary.
                PhoneApplicationService.Current.State["htmlResponse"] = _htmlResponse;
            }
            else
            {
                PhoneApplicationService.Current.State["htmlResponse"] = null;
            }
        }

        #endregion

    }
}
