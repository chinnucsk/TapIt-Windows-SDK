using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Networking.Connectivity;

namespace TapIt_WP8
{
    public class DeviceDataMgr
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
        PageOrientation _orientation;

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

        public PageOrientation Orientation
        {
            get { return _orientation; }
        }

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
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(OnNetworkChanged);
        }

        #endregion

        #region Methods

        private string GetUserAgent()
        {
            string strUserAgent = String.Empty;

            switch (OsVersion)
            {
                case "7.0":
                case "7.1":
                case "7.2": // todo: check for WP7.8
                    strUserAgent = TapItResource.UserAgent7_1;
                    break;

                case "8.0":
                    strUserAgent = TapItResource.UserAgent8_0;
                    //strUserAgent = "Mozilla%2F5.0+%28Linux%3B+U%3B+Android+2.2%3B+en-us%3B+sdk+Build%2FFRF91%29+AppleWebKit%2F533.1+%28KHTML%2C+like+Gecko%29+Version%2F4.0+Mobile+Safari%2F533.1";
                    break;
            }

            return strUserAgent;
        }

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

        ///<summary>
        ///collection of device parameters
        ///</summary>
        public void GetdeviceInfo()
        {
            try
            {
                _orientation = ((PhoneApplicationFrame)Application.Current.RootVisual).Orientation;

                _version = Environment.OSVersion.Version;
                _osVersion = _version.Major.ToString() + "." + _version.Minor.ToString();
                _userAgent = GetUserAgent();

                //airtel,tatadocomo
                _mobileOperator = DeviceNetworkInformation.CellularMobileOperator;

                //3g,wifi,gsm
                _networkType = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType.ToString();

                //MessageBox.Show("_networkType: " + _networkType);
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
                _screenWidth = Convert.ToInt32(Application.Current.Host.Content.ActualWidth);
                _screenHeight = Convert.ToInt32(Application.Current.Host.Content.ActualHeight);
                _language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                object uniqueID;
                if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueID) == true)
                {
                    byte[] byteID = (byte[])uniqueID;
                    _deviceID = Convert.ToBase64String(byteID);
                }

                GetLocationProperty();
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
        public bool GetLocationProperty()
        {
            bool bRet = false;
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

            return bRet;
        }

        #endregion
    }
}
