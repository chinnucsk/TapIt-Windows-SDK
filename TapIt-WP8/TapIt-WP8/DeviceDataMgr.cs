using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
        string _userAgent = "Mozilla/5.0(compatible;MSIE 10.0;Windows phone 8.0;Trident/6.0;IEMobile/10.0;ARM;Touch)";

        #endregion

        #region Property

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

        public DeviceDataMgr()
        {
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(OnNetworkChanged);
        }

        #endregion

        #region Methods

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
                Console.WriteLine(ex.Message);
            }
        }

        ///<summary>
        ///collection of device parameters
        ///</summary>
        public void GetdeviceInfo()
        {
            _mobileOperator = DeviceNetworkInformation.CellularMobileOperator;
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
