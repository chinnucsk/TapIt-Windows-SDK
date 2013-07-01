using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapIt_WP8
{
  // Conversion and Installation Tracker. Sends a notification to TapIt servers
  // with UDID, UA, and Package Name

    public class InstallTracker
    {
        #region Data Members
        private string _nameSpace = String.Empty;
        private string _offerId = String.Empty;
        private string _ua = String.Empty;

        private static string TRACK_HOST = "a.tapit.com";
        private static string TRACK_HANDLER = "/adconvert.php";

        private static InstallTracker _instance;

        #endregion

        #region Constructor

        private InstallTracker()
        {

        }

        #endregion

        #region Property

        public static InstallTracker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InstallTracker();
                }
                return _instance;
            }
        }

        public void ReportInstall()
        {
            DeviceDataMgr deviceData = DeviceDataMgr.Instance;
            string _deviceId = deviceData.DeviceID;
            string ua = deviceData.UserAgent;
        }

        #endregion
    }
}
