using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;
#if WINDOWS_PHONE
using TapIt_WP8.Resources;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
#elif WIN8
using Windows.UI.Xaml.Controls;
using TapIt_Win8;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Display;
#endif


namespace TapIt_WP8
{
    public abstract class AdView : AdViewBase
    {
        #region DataMembers

        private Grid _maingrid;
#if WINDOWS_PHONE
        private WebBrowser _webBrowser;
#elif WIN8
        private WebView _webBrowser;
#endif
        private Thickness _margin;

        private string _htmlResponse = string.Empty; // get the string in html format

        #endregion

        #region EventsDecleration

#if WINDOWS_PHONE
        public event EventHandler<NavigationEventArgs> Navigated;
        public event NavigationFailedEventHandler NavigationFailed;
#elif WIN8
        public event WebViewNavigationFailedEventHandler NavigationFailed;
#endif

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

        protected
#if WINDOWS_PHONE
        WebBrowser
#elif WIN8
 WebView
#endif
        WebBrowser
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
            set
            {
                _webBrowser.Margin = _margin = value; 
            }
        }

        #endregion

        #region Constructor

        public AdView()
        {
            //initailization of grid
            _maingrid = new Grid();

#if WINDOWS_PHONE
            _maingrid.Name = TapItResource.AdControlName;
            
            //initailization of browser control
            _webBrowser = new WebBrowser();
            
            //events  for web control
            _webBrowser.Navigating +=_webBrowser_Navigating;
            _webBrowser.Navigated += _webBrowser_Navigated;
            _webBrowser.NavigationFailed += _webBrowser_NavigationFailed;
#elif WIN8
            _maingrid.Name = ResourceStrings.AdControlName;
            
            //initailization of browser control
            _webBrowser = new WebView();
            
            //events  for web control
            _webBrowser.NavigationFailed += _webView_NavigationFailed;
#endif
            _webBrowser.Width = Width;
            _webBrowser.Height = Height;
            _webBrowser.Margin = Margin;

            _webBrowser.Loaded += _webBrowser_Loaded;
            _webBrowser.LoadCompleted += _webBrowser_LoadCompleted;

            //add webcontrol to grid
            _maingrid.Children.Add(_webBrowser);

            //events  for main grid
            _maingrid.SizeChanged += _maingrid_SizeChanged;
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
        ///  // This event is fired when the web browser control is loaded.
        /// </summary>
        void _webBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Loaded()");

            base.OnControlLoad(sender, e);
        }

        /// <summary>
        ///  // This event is fired when the web browser content loading is completed.
        /// </summary>
        private void _webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_webBrowser_LoadCompleted()");

            base.OnContentLoad(sender, e);
        }

#if WINDOWS_PHONE

        /// <summary>
        ///  // This event is fired when the web browser navigation fails.
        /// </summary>
        void _webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("_webBrowser_NavigationFailed() :" + e.Exception.Message);

            NavigationFailedEventHandler handler = NavigationFailed;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        ///  // This event is fired when the web browser is navigated to a url.
        /// </summary>
        void _webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Navigated()");

            EventHandler<NavigationEventArgs> handler = Navigated;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        protected virtual void Navigating()
        {

        }

        /// <summary>
        ///  // In-App browser is launched
        /// </summary>
        void _webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Navigating()");
            
            e.Cancel = true;
            Navigating();

            bool success = false;
            string encodedUri = String.Empty;
            if (GetNavigationServiceRef != null)
            {
                InAppBrowserPage._adViewBase = this;
                encodedUri = WebUtility.UrlEncode(e.Uri.ToString());
                success = GetNavigationServiceRef.Navigate(new Uri(string.Format
                             ("/TapIt-WP8;component/Resources/InAppBrowserPage.xaml?navigatingUri={0}",
                             encodedUri), UriKind.RelativeOrAbsolute));
            }
                
            if (success)
            {
                OnNavigatingToInAppBrowser(encodedUri);
            }
            else
            {
                OnError(TapItResource.NavigationErrorMsg);
            }
        }

#elif WIN8
        void _webView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            Debug.WriteLine("_webView_NavigationFailed() :" + e.WebErrorStatus.ToString());

            WebViewNavigationFailedEventHandler handler = NavigationFailed;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
#endif

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
#if WINDOWS_PHONE
            if (PageOrientation.LandscapeRight == deviceData.DeviceOrientation ||
                   PageOrientation.LandscapeLeft == deviceData.DeviceOrientation)
            {
                width = deviceData.ScreenHeight - (SystemTray.IsVisible ? SystemTrayWidthLandscape : 0);
            }
            else if (PageOrientation.PortraitDown == deviceData.DeviceOrientation ||
                PageOrientation.PortraitUp == deviceData.DeviceOrientation)
            {
                width = deviceData.ScreenWidth;
            }
#elif WIN8
            // todo: need to consider snapped and filled view
            if (DisplayOrientations.Landscape == deviceData.DeviceOrientation ||
                  DisplayOrientations.LandscapeFlipped == deviceData.DeviceOrientation)
            {
                width = deviceData.ScreenWidth;
            }
            else if (DisplayOrientations.Portrait == deviceData.DeviceOrientation ||
                DisplayOrientations.PortraitFlipped == deviceData.DeviceOrientation)
            {
                width = deviceData.ScreenHeight;
            }
#endif
            return width;
        }

        protected int GetOrientationHeight()
        {
            int height = 0;
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
#if WINDOWS_PHONE
             if (PageOrientation.LandscapeRight == deviceData.DeviceOrientation ||
                   PageOrientation.LandscapeLeft == deviceData.DeviceOrientation)
            {
                height = deviceData.ScreenWidth;
            }
            else if (PageOrientation.PortraitDown == deviceData.DeviceOrientation ||
                PageOrientation.PortraitUp == deviceData.DeviceOrientation)
            {
                height = deviceData.ScreenHeight - (SystemTray.IsVisible ? SystemTrayHeightPortrait : 0) ;
            }
#elif WIN8
            if (DisplayOrientations.Landscape == deviceData.DeviceOrientation ||
                   DisplayOrientations.LandscapeFlipped == deviceData.DeviceOrientation)
            {
                height = deviceData.ScreenHeight;
            }
            else if (DisplayOrientations.Portrait == deviceData.DeviceOrientation ||
                DisplayOrientations.PortraitFlipped == deviceData.DeviceOrientation)
            {
                height = deviceData.ScreenWidth;
            }
#endif
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
                    int ViewPortHeight = (int)
                    WebBrowser.ActualHeight;
                    retVal = JsonToHtml(GetOrientationWidth(),
                                                (ViewPortHeight > 0 ? ViewPortHeight : GetOrientationHeight()));
                }
            }
            catch (Exception ex)
            {
                if (bRaiseError)
                    OnError("Error in Load(): " + ex);
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
                _htmlResponse = jsnParser.WrapToHTML(response, JsonResponse,
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

        public override void DeviceOrientationChanged(
#if WINDOWS_PHONE
            PageOrientation
#elif WIN8
DisplayOrientations
#endif
 pageOrientation)
        {
            base.DeviceOrientationChanged(pageOrientation);
        }

#if WINDOWS_PHONE
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
#endif
        #endregion

    }
}
