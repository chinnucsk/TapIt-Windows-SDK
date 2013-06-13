
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
    public abstract class AdView
    {
        #region DataMembers

        private WebBrowser _webBrowser;
        private Grid _maingrid;

        private int _width;
        private int _height;
        private Thickness _margin;
        protected Visibility _visible = Visibility.Collapsed;
        //private ViewState _viewState = ViewState.DEFAULT;
        private AdType _adtype = AdType.Unknown;
        private int _zoneId = -1;

        //private string _baseURL = TapItResource.BaseUrl; //TapIt server url
        private string _baseURL = TapItResource.BaseUrl_Local; //Local server url
        private string _format = TapItResource.Format;
        private string _htmlResponse = string.Empty; // get the string in html format
        private string _clickUrl = string.Empty;
        private JsonDataContract _jsonResponse;

        #endregion

        #region EventsDecleration

        public event RoutedEventHandler ControlLoaded;
        public event LoadCompletedEventHandler ContentLoaded;
        public event EventHandler<NavigatingEventArgs> Navigating;
        public event EventHandler<NavigationEventArgs> Navigated;
        public event NavigationFailedEventHandler NavigationFailed;

        public delegate void ErrorEventHandler(string strErrorMsg);
        public event ErrorEventHandler ErrorEvent;

        #endregion

        #region Enum

        /*private enum ViewState
        {
            DEFAULT,
            RESIZED,
            EXPANDED,
            HIDDEN
        }*/

        public enum AdType
        {
            Unknown = -1,
            Banner_Ad = 1,
            Interstitial_Ad = 2,
            Text_Ad = 5,
            Video_Ad = 6,
            Offerwall_Ad = 7,
            Ad_Prompt = 10
        }

        #endregion

        #region Property

        public JsonDataContract JsonResponse
        {
            get { return _jsonResponse; }
        }

        protected Grid Maingrid
        {
            get { return _maingrid; }
        }

        public int ZoneId
        {
            get { return _zoneId; }
            set { _zoneId = value; }
        }

        public AdType Adtype
        {
            get { return _adtype; }
            set { _adtype = value; }
        }

        public virtual Visibility Visible
        {
            get { return _visible; }
            set
            {
                _maingrid.Visibility = _visible = value;
            }
        }

        public string ClickUrl
        {
            get { return _clickUrl; }
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

        public int Width
        {
            get { return _width; }
            set { _webBrowser.Width = _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _webBrowser.Height = _height = value; }
        }

        public Thickness Margin
        {
            get { return _margin; }
            set { _webBrowser.Margin = _margin = value; }
        }

        public string BaseURL
        {
            get { return _baseURL; }
            set { _baseURL = value; }
        }

        #endregion

        #region Constructor

        public AdView()
        {
            //initailization of grid
            _maingrid = new Grid();
            _maingrid.Name = TapItResource.BrowserName;
            Maingrid.Background = new SolidColorBrush(Colors.Gray);

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

            if (ControlLoaded != null)
                ControlLoaded(sender, e);
        }

        /// <summary>
        ///  // This event is fired when the web browser content loading is completed.
        /// </summary>
        private void _webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_webBrowser_LoadCompleted");

            if (ContentLoaded != null)
                ContentLoaded(sender, e);
        }

        /// <summary>
        ///  // The event is fired when user clicks on the web browser.
        /// </summary>
        void _webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Navigating");

            if (Navigating != null)
                Navigating(sender, e);

            e.Cancel = true;
            WebBrowserTask browserTask = new WebBrowserTask();
            browserTask.Uri = e.Uri;
            browserTask.Show();
        }

        #endregion

        #region abstract Methods

        abstract protected void SetAdType();

        abstract protected void SetAdSize(int height, int width);

        #endregion

        #region Methods

        //url created using device parameters
        private string GetAdSrvURL()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            deviceData.GetdeviceInfo();
            //url creation using tapIt base url.
            string AdSrvURL = BaseURL + "?" +
                "w=" + Width +
                "&h=" + Height +
                "&languages=" + deviceData.Language +
                "&ua=" + deviceData.UserAgent +
                "&udid=" + deviceData.DeviceID +
                "&connection_speed=" + deviceData.ConnectionSpd +
                "&carrier=" + deviceData.MobileOperator +
                "&format=" + _format +
                "&zone=" + ZoneId +
                "&adtype=" + Convert.ToInt32(Adtype);

            //url creation using local server url.
            // string AdSrvURL = "http://ec2-107-20-3-62.compute-1.amazonaws.com/~chetanch/adrequest.php?zone=14999&format=json";
            Debug.WriteLine("Server Url: " + AdSrvURL);
            return AdSrvURL;
        }

        public async Task<bool> LoadAndNavigate()
        {
            bool retVal = await Load();

            if (retVal)
            {
                retVal = JsonToHtml();

                if (retVal)
                    NavigateToHtml();
            }

            return retVal;
        }

        ///<summary>
        ///
        ///</summary>
        ///
        public async Task<bool> Load()
        {
            bool isLoaded = false;
            try
            {
                TapItHttpRequest req = new TapItHttpRequest();
                string response = await req.HttpRequest(GetAdSrvURL());
                if (response == null || response.Contains("error"))
                {
                    Exception ex = new Exception("Response contains error");
                    throw ex;
                }

                JsonParser jsnParser = new JsonParser();
                _jsonResponse = jsnParser.ParseJson(response);
               
                isLoaded = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in Load() :" + ex.Message);
                RaiseErrorEvent("Exception in Load()", ex);
            }

            return isLoaded;
        }

        private bool JsonToHtml()
        {
            bool isLoaded = false;
            try
            {
                string response = JsonResponse.Html;
                JsonParser jsnParser = new JsonParser();

                _htmlResponse = jsnParser.WrapToHTML(response, null, null, JsonResponse);
                Debug.WriteLine("html response :" + _htmlResponse);

                if (!string.IsNullOrEmpty(_htmlResponse))
                {
                    isLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in JsonToHtml() :" + ex.Message);
                RaiseErrorEvent("Exception in JsonToHtml()", ex);
            }

            return isLoaded;
        }

        private void NavigateToHtml()
        {
            WebBrowser.NavigateToString(_htmlResponse);
        }

        ///<summary>
        /// // The helper function to raise error event.
        ///</summary>
        private void RaiseErrorEvent(string strMsg, Exception ex = null)
        {
            if (ErrorEvent != null)
            {
                string str = strMsg;

                if (ex != null)
                    str += " Exception occured :" + ex.Message;

                ErrorEvent(str);
            }
        }

        ///<summary>
        ///
        ///</summary>
        public void AppActivated()
        {
            if (PhoneApplicationService.Current.State["htmlResponse"] == null)
                return;

            _htmlResponse = PhoneApplicationService.Current.State["htmlResponse"] as string;
            WebBrowser.NavigateToString(_htmlResponse);
        }

        ///<summary>
        ///
        ///</summary>
        public void AppDeactivated()
        {
            // If there is data in the application member variable...
            if (!string.IsNullOrEmpty(_htmlResponse))
            {
                // Store it in the State dictionary.
                PhoneApplicationService.Current.State["htmlResponse"] = _htmlResponse;
            }
        }

        #endregion

    }
}
