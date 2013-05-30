using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
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
    public class AdView
    {
        #region DataMembers

        private WebBrowser _webBrowser;
        public event LoadCompletedEventHandler PageLoaded;
        private int _width = 480;
        private int _height = 150;
        private Thickness _margin;
        private string htmlResponse = string.Empty; // get the string in html format
        private string _latitude = string.Empty;
        private string _longitude = string.Empty;
        private string _deviceId = string.Empty;
        private string _baseURL = "http://r.tapit.com/adrequest.php"; //TapIt server url
        //private string _baseURL = "http://ec2-107-20-3-62.compute-1.amazonaws.com/~chetanch/adrequest.php"; //Local server url

        #endregion

        #region Property

        //return control
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

        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        public string Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
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
            _webBrowser.Name = "TapItAdViewControl";   // todo: 
            _webBrowser.Width = Width;
            _webBrowser.Height = Height;
            _webBrowser.Margin = Margin;
            _webBrowser.LoadCompleted += _webBrowser_LoadCompleted;
            _webBrowser.Navigating += _webBrowser_Navigating;
        }

        #endregion

        #region Events

        /// <summary>
        ///  //this event is fired when web browser contents are loaded.
        /// </summary>
        private void _webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (PageLoaded != null)
                PageLoaded(sender, e);
        }
       
        /// <summary>
        ///   //The event for advertise opening in an internal browser.
        /// </summary>
        void _webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            e.Cancel = true;
            WebBrowserTask browserTask = new WebBrowserTask();
            browserTask.Uri = e.Uri;
            browserTask.Show();
        }

        #endregion

        #region Methods
       
        private string GetAdSrvURL()
        {
            DeviceDataMgr deviceData = new DeviceDataMgr();
            deviceData.GetdeviceInfo();
            //url creation using tapIt base url.
            string AdSrvURL = BaseURL + "?&w=320&languages=en&connection_speed=1&ua=" + deviceData.UserAgent + "&carrier=Android&sdk=android-v1.7.6&format=json&udid=5284047f4ffb4e04824a2fd1d1f0cd62&h=50&zone=&o=p";
            //string AdSrvURL = BaseURL + "?" + "&" + "w=" + "320" + "&" + "languages=" + deviceData.Language + "&" + "connection_speed=1"+"&" + "carrier=Android&sdk=android-v1.7.6" + "&" + "format=json" + "&" + "udid=" + deviceData.DeviceID + "&" + "h=" +"80"+ "&" + "zone=&o=p";

            //url creation using local server url.
            //string AdSrvURL = BaseURL + "?" + "zone=14999&ip=121.242.40.15" + "&mode=test";
            return AdSrvURL;
        }

        public async Task<bool> Show() // todo: return errorcode insted of void
        {
            TapItHttpRequest req = new TapItHttpRequest();
            string response = await req.HttpRequest(GetAdSrvURL());
            if (response == null || response.Contains("error"))
            {
                MessageBox.Show("Response contains error"); // temp code
                return false;
            }
            JsonParser jsnParser = new JsonParser();
            JsonDataContract jsonData = jsnParser.ParseJson(response);
            response = jsonData.Html;
            htmlResponse = jsnParser.WrapToHTML(response, null, null, jsonData);
            WebBrowser.NavigateToString(htmlResponse);
            return true;
        }

        ///<summary>
        ///This event is fired when the app came to foreground
        ///</summary>
        public void AppActivated()
        {
            if (PhoneApplicationService.Current.State["htmlResponse"] == null) return;
            htmlResponse = PhoneApplicationService.Current.State["htmlResponse"] as string;
            WebBrowser.NavigateToString(htmlResponse);
        }

        ///<summary>
        ///This event is fired just before the app will be sent to the background.
        ///</summary>
        public void AppDeactivated()
        {
            // If there is data in the application member variable...
            if (!string.IsNullOrEmpty(htmlResponse))
            {
                // Store it in the State dictionary.
                PhoneApplicationService.Current.State["htmlResponse"] = htmlResponse;
            }
        }

        #endregion
    }
}
