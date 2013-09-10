using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Windows.Graphics.Display;

#if WINDOWS_PHONE
using System.Windows.Threading;
using System.Net.Sockets;
using System.Device.Location;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;

#elif WIN8
using Windows.UI.Xaml.Controls;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml;
using Windows.Devices.Geolocation;
#endif

namespace TapIt_WP8
{
    class DeviceDataMgr
    {
        #region DataMember

        string _networkType = String.Empty;
        string _mobileOperator = String.Empty;
        bool _isCellularDataEnabled = false;
        bool _isNetworkAvailable = false;
        int _screenWidth = 0;
        int _screenHeight = 0;
        string _language = String.Empty;
        string _deviceID = String.Empty;
        double _latitude = 0;
        double _longitude = 0;
        string _userAgent = String.Empty;
        string _osVersion = String.Empty;
        int _connectionSpd = Convert.ToInt32(ConnectionSpeed.Unknown);
        private static DeviceDataMgr _instance;
        Version _version;
        #if WINDOWS_PHONE

        PageOrientation _pageOrientation = PageOrientation.None; 
#elif WIN8
        DisplayOrientations _pageOrientation = DisplayOrientations.None;
#endif
 
        string _pageOrientationVal = "p";

        #endregion

        #region enum

        public enum ConnectionSpeed
        {
            Unknown = -1,
            Low_Speed = 0,
            High_Speed = 1
        }

        #endregion

        #region Property

        public static DeviceDataMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeviceDataMgr();
                }
                return _instance;
            }
        }

        public string PageOrientationVal
        {
            get
            {
#if WINDOWS_PHONE
                 PageOrientation pageOrientation = DeviceOrientation;
                if (PageOrientation.LandscapeRight == pageOrientation ||
                    PageOrientation.LandscapeLeft == pageOrientation)
                {
                    _pageOrientationVal = "l";
                }
                else if (PageOrientation.PortraitDown == pageOrientation ||
                    PageOrientation.PortraitUp == pageOrientation)
                {
                    _pageOrientationVal = "p";
                }
#elif WIN8
                DisplayOrientations pageOrientation = DeviceOrientation;

                if (DisplayOrientations.Landscape == pageOrientation ||
                   DisplayOrientations.LandscapeFlipped == pageOrientation)
                {
                    _pageOrientationVal = "l";
                }
                else if (DisplayOrientations.Portrait == pageOrientation ||
                    DisplayOrientations.PortraitFlipped == pageOrientation)
                {
                    _pageOrientationVal = "p";
                }
#endif

                return _pageOrientationVal;
            }
        }


        #if WINDOWS_PHONE
         public PageOrientation DeviceOrientation
        {
            get
            {
                _pageOrientation = ((PhoneApplicationFrame)Application.Current.RootVisual).Orientation;
                return _pageOrientation;
            }
            set
            {
                _pageOrientation = value;
            }
        }
#elif WIN8
        public DisplayOrientations DeviceOrientation
        {
            get
            {
                _pageOrientation = DisplayProperties.CurrentOrientation;
                return _pageOrientation;
            }
            set
            {
                _pageOrientation = value;
            }
        }
#endif


        public string OsVersion
        {
            get { return _osVersion; }
        }

        public int ConnectionSpd
        {
            get { return _connectionSpd; }
        }

        public string UserAgent
        {
            get { return _userAgent; }
        }

        public bool IsCellularDataEnabled
        {
            get { return _isCellularDataEnabled; }
        }

        public double Latitude
        {
            get { return _latitude; }
        }

        public double Longitude
        {
            get { return _longitude; }
        }

        public string DeviceID
        {
            get { return _deviceID; }
        }

        public string Language
        {
            get { return _language; }
        }

        public int ScreenHeight
        {
            get { return _screenHeight; }
        }

        public int ScreenWidth
        {
            get { return _screenWidth; }
        }

        public string MobileOperator
        {
            get { return _mobileOperator; }
        }

        public string NetworkType
        {
            get { return _networkType; }
        }

        public bool IsNetworkAvailable
        {
            get { return _isNetworkAvailable; }
        }

        #endregion

        #region Constructor

        private DeviceDataMgr()
        {
#if WINDOWS_PHONE
      NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(OnNetworkChanged);
#endif
            UpdateDeviceStaticData();
            UpdateDeviceInfo();
        }

        #endregion

        #region Methods

        private void GetUserAgent()
        {
            if (!String.IsNullOrEmpty(_userAgent))
                return;

            string uaHtml = "<!DOCTYPE HTML> " +
                "<html> " +
                "<head> " +
                "<script language=\"javascript\" type=\"text/javascript\"> " +
                "function notifyUserAgent() { window.external.notify(navigator.userAgent); } " +
                "</script> " +
                "</head> " +
                "<body onload=\"notifyUserAgent()\"> " +
                "</body> " +
                "</html> ";

#if WINDOWS_PHONE
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.IsScriptEnabled = true;
#elif WIN8
            WebView webBrowser = new WebView();
            webBrowser.AllowedScriptNotifyUris = WebView.AnyScriptNotifyUri;
#endif
            webBrowser.ScriptNotify += webBrowser_ScriptNotify;
            webBrowser.NavigateToString(uaHtml);
        }

#if WINDOWS_PHONE
         ///<summary>
        ///get the changed network data
        ///</summary>
        private void OnNetworkChanged(object sender, EventArgs e)
        {
            try
            {
                _networkType = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in OnNetworkChanged() :" + ex.Message);
                throw ex;
            }
        }
#endif

        private void webBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            _userAgent = e.Value;
        }

        public async Task<bool> UpdateDeviceInfoWithLocation()
        {
            UpdateDeviceInfo();
            return await UpateLocation();
        }

        public void UpdateDeviceStaticData()
        {
            try
            {
                GetUserAgent();
#if WINDOWS_PHONE
                 _version = Environment.OSVersion.Version;
                _osVersion = _version.Major.ToString() + "." + _version.Minor.ToString();

                _screenWidth = Convert.ToInt32(Application.Current.Host.Content.ActualWidth);
                _screenHeight = Convert.ToInt32(Application.Current.Host.Content.ActualHeight);

                object uniqueID;
                if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueID) == true)
                {
                    byte[] byteID = (byte[])uniqueID;
                    _deviceID = Convert.ToBase64String(byteID);
                }
#elif WIN8
                _version = new Version(6, 2);
                _osVersion = _version.Major.ToString() + "." + _version.Minor.ToString();

                Windows.Foundation.Rect rect = Window.Current.Bounds;
                _screenWidth = Convert.ToInt32(rect.Width);
                _screenHeight = Convert.ToInt32(rect.Height);

                var profiles = NetworkInformation.GetConnectionProfiles();
                var adapter = profiles[0].NetworkAdapter;
                _deviceID = adapter.NetworkAdapterId.ToString();
#endif
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in GetdeviceInfo() :" + ex.Message);
                throw ex;
            }
        }

        ///<summary>
        ///collection of device parameters
        ///</summary>
        public void UpdateDeviceInfo()
        {
            try
            {
#if WINDOWS_PHONE
                DeviceOrientation = ((PhoneApplicationFrame)Application.Current.RootVisual).Orientation;

                // _mobileOperator = "Android"; // temp code for test purpose
                _mobileOperator = DeviceNetworkInformation.CellularMobileOperator;

                _networkType = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType.ToString();
                switch (_networkType)
                {
                    // The network interface uses an Ethernet connection. Ethernet is defined in
                    // IEEE standard 802.3. This is used for desktop pass-through.
                    case "Ethernet":
                    // The network interface uses a wireless LAN connection (IEEE 802.11 standard).
                    // This is used for any Wi-Fi (802.11, Bluetooth, and so on).
                    case "Wireless80211":
                    // The network interface uses a GSM cellular network.                
                    case "MobileBroadbandGsm":
                    // The network interface uses a CDMA cellular network.
                    case "MobileBroadbandCdma":
                        _connectionSpd = Convert.ToInt32(ConnectionSpeed.High_Speed);
                        break;

                    // There is no network available for accessing the Internet.
                    case "None":
                    default:
                        _connectionSpd = Convert.ToInt32(ConnectionSpeed.Unknown);
                        break;
                }

                _isCellularDataEnabled = DeviceNetworkInformation.IsCellularDataEnabled;
                _isNetworkAvailable = DeviceNetworkInformation.IsNetworkAvailable;
#elif WIN8
                DeviceOrientation = DisplayProperties.CurrentOrientation;
                
                ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile != null)
                {
                    var interfaceType = profile.NetworkAdapter.IanaInterfaceType;
                    switch (interfaceType)
                    {
                        case 71: // "WiFi";
                        case 6:  // "Ethernet";
                            _connectionSpd = Convert.ToInt32(ConnectionSpeed.High_Speed);
                            break;
                        default:
                            _connectionSpd = Convert.ToInt32(ConnectionSpeed.Unknown);
                            break;
                    }
                }
#endif
                //_language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                _language = CultureInfo.CurrentCulture.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in GetdeviceInfo() :" + ex.Message);
                throw ex;
            }
        }

        ///<summary>
        ///get device location(latitude,longitude)
        ///</summary>
        public async Task<bool> UpateLocation()
        {
            bool bRet = false;
#if WINDOWS_PHONE
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            GeoCoordinate coord = watcher.Position.Location;
            if (false == coord.IsUnknown)
            {
                _latitude = coord.Latitude;
                _longitude = coord.Longitude;
                bRet = true;
            }
#elif WIN8
            Geolocator geolocator = new Geolocator();
            //geolocator.PositionChanged += geolocator_PositionChanged;
            if (geolocator.LocationStatus != PositionStatus.Disabled)
            {
                Geoposition pos = await geolocator.GetGeopositionAsync();
                _latitude = pos.Coordinate.Latitude;
                _longitude = pos.Coordinate.Longitude;
                bRet = true;
            }
#endif
            return bRet;
        }

        #endregion
    }
}
