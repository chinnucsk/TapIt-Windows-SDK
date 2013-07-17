using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TapIt_WP8
{
    // Conversion and Installation Tracker. Sends a notification to TapIt servers
    // with UDID, UA, and namespace Name

    public class Tracker
    {
        #region Data Members

        private static string TRACK_HOST = TapItResource.TrackHost;
        private static string TRACK_HANDLER = TapItResource.TrackHandler;

        private static Tracker _instance;

        private string FileName = TapItResource.FileName;

        #endregion

        #region Constructor

        private Tracker()
        {
        }

        #endregion

        #region Property

        public static Tracker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Tracker();
                }

                return _instance;
            }
        }

        #endregion

        #region Methods

        public async Task<bool> ReportInstall(string offerID)
        {
            bool isInstall = false;
            try
            {
                if (String.IsNullOrEmpty(offerID))
                {
                    Debug.WriteLine("Error: Offer id is null");
                    return false;
                }

                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                StreamReader sr = new StreamReader(isoStore.OpenFile(FileName, FileMode.OpenOrCreate));
                string fileData = sr.ReadToEnd();
                sr.Close();

                if (!string.IsNullOrEmpty(fileData) && 
                    fileData == TapItResource.SuccessMesg)
                {
                    Debug.WriteLine(TapItResource.TrackerMesg);
                    return true;
                }

                DeviceDataMgr deviceData = DeviceDataMgr.Instance;
                if (String.IsNullOrEmpty(deviceData.UserAgent))
                {
                    Debug.WriteLine("Error: Unable to get User Agent");
                    return false;
                }

                string nameSpace = Application.Current.GetType().Namespace;

                string installTrackerUrl = "http://"
                                           + TRACK_HOST
                                           + TRACK_HANDLER
                                           + "?pkg=" + nameSpace
                                           + "&offer=" + offerID
                                           + "&udid=" + deviceData.DeviceID
                                           + "&ua=" + WebUtility.UrlEncode(deviceData.UserAgent);

                isInstall = await TrackInstall(installTrackerUrl);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ReportInstall(): " + ex.Message);
            }

            return isInstall;
        }

        private async Task<bool> TrackInstall(string Url)
        {
            bool retVal = false;
            try
            {
                TapItHttpRequest tapItHttpReq = new TapItHttpRequest();
                string response = await tapItHttpReq.HttpRequest(Url);
                if (!string.IsNullOrEmpty(response))
                {
                    IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                    StreamWriter sw = new StreamWriter(isoStore.OpenFile(FileName, FileMode.OpenOrCreate));
                    sw.Write(TapItResource.SuccessMesg);
                    sw.Close();
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in TrackInstall(): " + ex.Message);
            }

            return retVal;
        }

        public async Task<bool> ReportEvent(string eventstring)
        {
            bool isEventTrack = false;
            try
            {
                if (String.IsNullOrEmpty(eventstring))
                {
                    Debug.WriteLine("Error: Event string is null");
                    return false;
                }

                DeviceDataMgr deviceData = DeviceDataMgr.Instance;
                if (String.IsNullOrEmpty(deviceData.UserAgent))
                {
                    Debug.WriteLine("Error: Unable to get User Agent");
                    return false;
                }

                string nameSpace = Application.Current.GetType().Namespace;

                string eventTrackerUrl = "http://"
                                         + TRACK_HOST
                                         + TRACK_HANDLER
                                         + "?pkg=" + nameSpace
                                         + "&event=" + eventstring
                                         + "&udid=" + deviceData.DeviceID
                                         + "&ua=" + WebUtility.UrlEncode(deviceData.UserAgent);

                TapItHttpRequest tapItHttpReq = new TapItHttpRequest();
                string response = await tapItHttpReq.HttpRequest(eventTrackerUrl);

                if (!String.IsNullOrEmpty(response))
                    isEventTrack = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ReportEvent(): " + ex.Message);
            }

            return isEventTrack;
        }

        #endregion
    }
}
