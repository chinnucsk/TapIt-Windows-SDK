
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

        private int _width;
        private int _height;
        private Thickness _margin;
        private Visibility _visible = Visibility.Collapsed;
        private ViewState _viewState = ViewState.DEFAULT;

        private AdType _adtype = AdType.Unknown;

        private int _zoneId = -1;

       // private string _baseURL = TapItResource.BaseUrl; //TapIt server url
        private string _baseURL = TapItResource.BaseUrl_Local; //Local server url
        private string _format = TapItResource.Format;
        private string _htmlResponse = string.Empty; // get the string in html format
        private string _clickUrl = string.Empty;

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

        private enum ViewState
        {
            DEFAULT,
            RESIZED,
            EXPANDED,
            HIDDEN
        }

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

        public Visibility Visible
        {
            get { return _visible; }
            set
            {
                _webBrowser.Visibility = _visible = value;
            }
        }

        public string ClickUrl
        {
            get { return _clickUrl; }
        }

        // return control to add in UI tree
        public Control ViewControl
        {
            get { return _webBrowser; }
        }

        private WebBrowser WebBrowser
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
            _webBrowser = new WebBrowser();
            _webBrowser.Name = TapItResource.BrowserName;
            _webBrowser.Width = Width;
            _webBrowser.Height = Height;
            _webBrowser.Margin = Margin;

            _webBrowser.Loaded += _webBrowser_Loaded;
            _webBrowser.LoadCompleted += _webBrowser_LoadCompleted;
            _webBrowser.Navigating += _webBrowser_Navigating;
            _webBrowser.Navigated += _webBrowser_Navigated;
            _webBrowser.NavigationFailed += _webBrowser_NavigationFailed;
        }

        #endregion

        #region Events

        void _webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("_webBrowser_NavigationFailed :" + e.Exception.Message);

            if (NavigationFailed != null)
                NavigationFailed(sender, e);
        }

        void _webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Navigated");

            if (Navigated != null)
                Navigated(sender, e);
        }

        void _webBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("_webBrowser_Loaded");

            if (ControlLoaded != null)
                ControlLoaded(sender, e);
        }

        /// <summary>
        ///  //this event is fired when web browser contents are loaded.
        /// </summary>
        private void _webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("_webBrowser_LoadCompleted");

            if (ContentLoaded != null)
                ContentLoaded(sender, e);
        }

        /// <summary>
        ///   //The event for advertise opening in an internal browser.
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

        #region Methods

        abstract protected void SetAdType();

        private string GetAdSrvURL()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            deviceData.GetdeviceInfo();
            //url creation using tapIt base url.
            string AdSrvURL = BaseURL + "?" +
                "&w=" + Width + 
                "&h=" + Height +
                "&languages=" + deviceData.Language +
                "&ua=" + deviceData.UserAgent +
                "&udid=" + deviceData.DeviceID +
                "&connection_speed=" + deviceData.ConnectionSpd +
                "&carrier=" + deviceData.MobileOperator +
                "&format=" + _format +
                "&zone=" + ZoneId + 
                "&o=p"+
                "&adtype=" + Convert.ToInt32(Adtype);

            //url creation using local server url.
            //string AdSrvURL = BaseURL + "?" + "&format=json" + "&zone=1&ip=121.242.40.15" + "&mode=live";
            //string AdSrvURL = BaseURL + "?" + "&format=json" + "&zone=2719" + "&w="+deviceData.ScreenWidth+"&h="+deviceData.ScreenHeight;
            //AdSrvURL = "http://ec2-107-20-3-62.compute-1.amazonaws.com/~chetanch/adrequest.php?&w=320&languages=en&connection_speed=0&ua=Mozilla/5.0%28compatible;MSIE%2010.0;Windows%20phone%208.0;Trident/6.0;IEMobile/10.0;ARM;Touch%29&carrier=Vodafone%20IN&format=format&udid=IzKY3niXGzF3laaY0rhbvnNcCv8=&h=50&zone=1&o=p&adtype=1";
            Debug.WriteLine("Server Url: " + AdSrvURL);
            return AdSrvURL;
        }

        public async Task<bool> Load() // todo: return errorcode insted of void
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
                JsonDataContract jsonData = jsnParser.ParseJson(response);
                response = jsonData.Html;

                _clickUrl = jsonData.clickUrl;
                Debug.WriteLine("click url :" + ClickUrl);

                _htmlResponse = jsnParser.WrapToHTML(response, null, null, jsonData);
                Debug.WriteLine("html response :" + _htmlResponse);

                if (!string.IsNullOrEmpty(_htmlResponse))
                {
                    WebBrowser.NavigateToString(_htmlResponse);
                    isLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in Load() :" + ex.Message);
                RaiseErrorEvent("Exception in Load()", ex); 
            }

            return isLoaded;
        }

        private void RaiseErrorEvent(string strMsg, Exception ex = null)
        {
            if (ErrorEvent != null)
            {
                string str = strMsg;

                if (ex !=null)
                    str += " Exception occured :" + ex.Message;

                ErrorEvent(str);
            }
        }

        ///<summary>
        ///This event is fired when the app came to foreground
        ///</summary>
        public void AppActivated()
        {
            if (PhoneApplicationService.Current.State["htmlResponse"] == null) 
                return;

            _htmlResponse = PhoneApplicationService.Current.State["htmlResponse"] as string;
            WebBrowser.NavigateToString(_htmlResponse);
        }

        ///<summary>
        ///This event is fired just before the app will be sent to the background.
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
