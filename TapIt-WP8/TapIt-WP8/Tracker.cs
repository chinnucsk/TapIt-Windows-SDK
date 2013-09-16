using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
#elif WIN8
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.ApplicationModel;
#endif

#if WINDOWS_PHONE
namespace TapIt_WP8
#elif WIN8
namespace TapIt_Win8
#endif
{
    // Conversion and Installation Tracker. Sends a notification to TapIt servers
    // with UDID, UA, and namespace Name

    public class Tracker
    {
        #region Data Members

#if WINDOWS_PHONE
        private static string TRACK_HOST = TapItResource.TrackHost;
        private static string TRACK_HANDLER = TapItResource.TrackHandler;
        private string FileName = TapItResource.FileName;

#elif WIN8
        private static string TRACK_HOST = ResourceStrings.TrackHost;
        private static string TRACK_HANDLER = ResourceStrings.TrackHandler;
        private string FileName = ResourceStrings.FileName;

#endif

        private static Tracker _instance;

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
            string fileData = string.Empty;
            try
            {
                if (String.IsNullOrEmpty(offerID))
                {
                    Debug.WriteLine("Error: Offer id is null");
                    return false;
                }
#if WINDOWS_PHONE
                 IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                 StreamReader sr = new StreamReader(isoStore.OpenFile(FileName, FileMode.OpenOrCreate));
                 fileData = sr.ReadToEnd();
                 sr.Close();
#elif WIN8
                StorageFile sampleFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
                if (sampleFile != null)
                {
                    fileData = await FileIO.ReadTextAsync(sampleFile);
                }
#endif

                if (!string.IsNullOrEmpty(fileData) &&
                    fileData ==
#if WINDOWS_PHONE
                    TapItResource.SuccessMesg
#elif WIN8
 ResourceStrings.SuccessMesg
#endif
)
                {
                    Debug.WriteLine(
#if WINDOWS_PHONE
                        TapItResource.TrackerMesg
#elif WIN8
ResourceStrings.TrackerMesg
#endif
);
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
#if WINDOWS_PHONE
                    IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                    StreamWriter sw = new StreamWriter(isoStore.OpenFile(FileName, FileMode.OpenOrCreate));
                    sw.Write(TapItResource.SuccessMesg);
                    sw.Close();
#elif WIN8
                    StorageFile trackFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(trackFile, ResourceStrings.SuccessMesg);
#endif
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
