﻿using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TapIt_WP8.Resources;

namespace TapIt_WP8
{
    public abstract class AdViewBase
    {
        #region DataMember

        private const int _systemTrayHeightPortrait = 32;
        private const int _systemTrayWidthLandscape = 72;

        private int _width;
        private int _height;
        private int _adHeight;
        private int _adWidth;

        protected Visibility _visible = Visibility.Collapsed;

        private AdType _adtype = AdType.Unknown;
        private int _zoneId = -1;
        private string _baseURL = TapItResource.BaseUrl; //TapIt server url
        //private string _baseURL = TapItResource.BaseUrl_Local; //Local server url
        private string _format = TapItResource.Format;
        private JsonDataContract _jsonResponse;
        private string _sdkversion = TapItResource.SdkVersion;

        private bool _isAdLoaded = false;
        private bool _isAdLoadedPending = false;

        private bool _isInternalLoad = false;
        private bool _isAdDisplayed = false;
        private bool _isAppActived = false;
        private bool _isTimerInitiatedLoad = false;

        private NavigationService _navigationService = null;

        private Dictionary<string, string> _urlAdditionalParameters =
                                                new Dictionary<string, string>();

        #endregion

        #region abstract Methods

        abstract protected void SetAdType();

        abstract protected void SetAdSize(int height, int width);

        #endregion

        #region Constructor

        public AdViewBase()
        {
        }

        #endregion

        #region Event decleration

        public event RoutedEventHandler ControlLoaded;
        public event LoadCompletedEventHandler ContentLoaded;

        public delegate void ErrorEventHandler(string errorMsg);
        public event ErrorEventHandler ErrorEvent;

        public delegate void InAppBrowserClosedEventHandler();
        public event InAppBrowserClosedEventHandler InAppBrowserClosed;

        public delegate void NavigatingToInAppBrowserEventHandler(string uri);
        public event NavigatingToInAppBrowserEventHandler NavigatingToInAppBrowser;

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

        public Dictionary<string, string> UrlAdditionalParameters
        {
            get { return _urlAdditionalParameters; }
            set
            {
                if (null == value)
                    throw new ArgumentNullException();
                _urlAdditionalParameters = value;
            }
        }

        public NavigationService NavigationServiceRef
        {
            set { _navigationService = value; }
        }

        protected NavigationService GetNavigationServiceRef
        {
            get { return _navigationService; }
        }

        protected bool IsTimerInitiatedLoad
        {
            get { return _isTimerInitiatedLoad; }
            set { _isTimerInitiatedLoad = value; }
        }

        protected bool IsAdLoadedPending
        {
            get { return _isAdLoadedPending; }
            set { _isAdLoadedPending = value; }
        }

        protected int SystemTrayWidthLandscape
        {
            get { return _systemTrayWidthLandscape; }
        }

        protected int SystemTrayHeightPortrait
        {
            get { return _systemTrayHeightPortrait; }
        }

        protected bool IsAppActived
        {
            get { return _isAppActived; }
            set { _isAppActived = value; }
        }

        protected bool IsAdDisplayed
        {
            get { return _isAdDisplayed; }
            set { _isAdDisplayed = value; }
        }

        protected bool IsInternalLoad
        {
            get { return _isInternalLoad; }
            set { _isInternalLoad = value; }
        }

        protected bool IsAdLoaded
        {
            get { return _isAdLoaded; }
            set { _isAdLoaded = value; }
        }

        public abstract Visibility Visible
        {
            get;
            set;
        }

        protected JsonDataContract JsonResponse
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

        protected AdType Adtype
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

        protected int AdHeight
        {
            get { return _adHeight; }
            set { _adHeight = value; }
        }

        protected int AdWidth
        {
            get { return _adWidth; }
            set { _adWidth = value; }
        }

        #endregion

        #region Methods

        public void OnInAppBrowserClosed(object obj)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.

            InAppBrowserPage page = (InAppBrowserPage)obj;

            if (page == null)
                return;

            InAppBrowserClosedEventHandler handler = InAppBrowserClosed;
            if (handler != null)
            {
                handler();
            }
        }

        public virtual void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            deviceData.DeviceOrientation = pageOrientation;
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

            for (int i = 0; i < UrlAdditionalParameters.Count; i++)
            {
                AdSrvURL = AdSrvURL += "&"
                                       + UrlAdditionalParameters.ElementAt(i).Key
                                       + "="
                                       + UrlAdditionalParameters.ElementAt(i).Value;
            }


            // local server url. // temp code for testing purpose
            // string AdSrvURL = "http://ec2-107-20-3-62.compute-1.amazonaws.com/~chetanch/adrequest.php?zone=15087&format=json";
            Debug.WriteLine("Server Url: " + AdSrvURL);

            return AdSrvURL;
        }

        public virtual async Task<bool> Load(bool bRaiseError = true)
        {
            bool isLoaded = false;
            try
            {
                TapItHttpRequest req = new TapItHttpRequest();
                string response = await req.HttpRequest(GetAdSrvURL());
                if (response == null || response.Contains("error"))
                {
                    Exception ex = new Exception(TapItResource.ErrorResponse);
                    throw ex;
                }

                JsonParser jsnParser = new JsonParser();
                _jsonResponse = jsnParser.ParseJson(response);

                isLoaded = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in Load() :" + ex.Message);
                if (bRaiseError)
                    OnError("Exception in Load()", ex);
            }

            return isLoaded;
        }

        ///<summary>
        /// //The event-invoking method that derived classes can override.
        /// // The helper function to raise error event.
        ///</summary>
        protected void OnError(string errorMsg, Exception ex = null)
        {
            IsAppActived = false;
            IsInternalLoad = false;

            string errorMsgDetail = errorMsg;

            if (ex != null)
                errorMsgDetail += " Exception occured :" + ex.Message;

            Debug.WriteLine("OnError(): " + errorMsgDetail);

            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            ErrorEventHandler handler = ErrorEvent;
            if (handler != null)
            {
                handler(errorMsgDetail);
            }
        }

        protected void OnNavigatingToInAppBrowser(string uri)
        {
            Debug.WriteLine("OnNavigatingToInAppBrowser(), uri: " + uri);

            NavigatingToInAppBrowserEventHandler handler = NavigatingToInAppBrowser;
            if (handler != null)
            {
                handler(uri);
            }
        }

        protected virtual void OnControlLoad(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnControlLoad()");

            RoutedEventHandler handler = ControlLoaded;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        protected virtual void OnContentLoad(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine("OnContentLoad()");

            IsAdLoadedPending = false;
            IsAdLoaded = true;
            IsAdDisplayed = false;

            if (!IsInternalLoad)
            {
                if (!IsAppActived)
                {
                    if (!IsTimerInitiatedLoad)
                    {
                        LoadCompletedEventHandler handler = ContentLoaded;
                        if (handler != null)
                        {
                            handler(sender, e);
                        }
                    }
                }
            }
            else
            {
                IsInternalLoad = false;
                Visible = Visibility.Visible;
            }

            IsAppActived = false;
            IsTimerInitiatedLoad = false;
        }

        #endregion
    }
}
