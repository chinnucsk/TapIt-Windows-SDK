using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace TapIt_WP8
{
    public abstract class AdViewBase
    {
        #region DataMember

        private int _width;
        private int _height;
        private int _adHeight;
        private int _adWidth;

        protected Visibility _visible = Visibility.Collapsed;

        private AdType _adtype = AdType.Unknown;
        private int _zoneId = -1;
        //private string _baseURL = TapItResource.BaseUrl; //TapIt server url
        private string _baseURL = TapItResource.BaseUrl_Local; //Local server url
        private string _format = TapItResource.Format;
        private JsonDataContract _jsonResponse;
        private string _sdkversion = TapItResource.SdkVersion;

        #endregion

        #region abstract Methods

        abstract protected void SetAdType();

        abstract protected void SetAdSize(int height, int width);

        #endregion

        #region Event decleration

        public event RoutedEventHandler ControlLoaded;
        public event LoadCompletedEventHandler ContentLoaded;

        public delegate void ErrorEventHandler(string strErrorMsg);
        public event ErrorEventHandler ErrorEvent;

        #endregion

        #region Enum

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

        public abstract Visibility Visible
        {
            get;
            set;
        }

        public JsonDataContract JsonResponse
        {
            get { return _jsonResponse; }
        }

        public string Format
        {
            get { return _format; }
        }

        public string BaseURL
        {
            get { return _baseURL; }
            set { _baseURL = value; }
        }

        public AdType Adtype
        {
            get { return _adtype; }
            set { _adtype = value; }
        }

        public int ZoneId
        {
            get { return _zoneId; }
            set { _zoneId = value; }
        }

        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public virtual int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int AdHeight
        {
            get { return _adHeight; }
            set { _adHeight = value; }
        }

        public int AdWidth
        {
            get { return _adWidth; }
            set { _adWidth = value; }
        }

        #endregion

        #region Methods

        public virtual void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            deviceData.PageOrientation = pageOrientation;
        }

        //url created using device parameters
        private string GetAdSrvURL()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            deviceData.UpdateDeviceInfoWithLocation();
            // url creation using tapIt base url.
            string AdSrvURL = BaseURL + "?" +
                "w=" + AdWidth +
                "&h=" + AdHeight +
                "&languages=" + deviceData.Language +
                "&ua=" + WebUtility.UrlEncode(deviceData.UserAgent) +
                "&udid=" + deviceData.DeviceID +
                "&connection_speed=" + deviceData.ConnectionSpd +
                "&carrier=" + deviceData.MobileOperator +
                "&sdk=" + _sdkversion +
                "&format=" + Format +
                "&zone=" + ZoneId +
                "&adtype=" + Convert.ToInt32(Adtype) +
                "&o=" + deviceData.PageOrientationVal +
                "&lat=" + deviceData.Latitude +
                "&long=" + deviceData.Longitude;

            // local server url. // temp code for testing purpose
            // string AdSrvURL = "http://ec2-107-20-3-62.compute-1.amazonaws.com/~chetanch/adrequest.php?zone=15087&format=json";
            Debug.WriteLine("Server Url: " + AdSrvURL);

            return AdSrvURL;
        }

        public virtual async Task<bool> Load()
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
                OnError("Exception in Load()", ex);
            }
            
            return isLoaded;
        }

        ///<summary>
        /// //The event-invoking method that derived classes can override.
        /// // The helper function to raise error event.
        ///</summary>
        protected void OnError(string strMsg, Exception ex = null)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            ErrorEventHandler handler = ErrorEvent;
            if (handler != null)
            {
                string str = strMsg;

                if (ex != null)
                    str += " Exception occured :" + ex.Message;
                handler(str);
            }
        }

        protected virtual void OnControlLoad(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnControlLoad");

            RoutedEventHandler handler = ControlLoaded;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        protected virtual void OnContentLoad(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("OnContentLoad");

            LoadCompletedEventHandler handler = ContentLoaded;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion
    }
}
