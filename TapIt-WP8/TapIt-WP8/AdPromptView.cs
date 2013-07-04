using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TapIt_WP8
{
    public class AdPromptView : AdViewBase
    {
        #region Constructor

        public AdPromptView()
        {

        }

        #endregion

        //Show the AdPrompt
        public override Visibility Visible
        {
            get { return _visible; }
            set
            {
                if (value == Visibility.Visible)
                {
                    if (IsAdDisplayed)
                    {
                        OnError(TapItResource.LoadNewAd);
                        return;
                    }

                    if (!IsAdLoaded)
                    {
                        IsInternalLoad = true;
                        Task<bool> b = Load();
                    }
                    else
                    {
                        _visible = value;
                        ShowAdPrompt();
                    }
                }
                else
                {
                    _visible = value;
                }
            }
        }

        #region Abstract methods

        protected override void SetAdType()
        {
            Adtype = AdType.Ad_Prompt;
        }

        protected override void SetAdSize(int height, int width)
        {
            // No code here 
        }

        #endregion

        #region Methods

        private void ShowAdPrompt()
        {
            if (JsonResponse != null)
            {
                IAsyncResult result = Microsoft.Xna.Framework.GamerServices.Guide.BeginShowMessageBox(
                   " ",
                  JsonResponse.adtitle,
                    new string[] { JsonResponse.declinestring, JsonResponse.calltoaction },
                    0,
                    Microsoft.Xna.Framework.GamerServices.MessageBoxIcon.None,
                    null,
                    null);
                result.AsyncWaitHandle.WaitOne();

                int? choice = Microsoft.Xna.Framework.GamerServices.Guide.EndShowMessageBox(result);
                if (choice.HasValue)
                {
                    if (choice.Value == 0)
                    {
                        //user clicks the first button 
                        Microsoft.Xna.Framework.GamerServices.Guide.EndShowMessageBox(result);
                    }
                    else
                    {
                        WebBrowserTask browserTask = new WebBrowserTask();
                        browserTask.Uri = new Uri(JsonResponse.clickUrl);
                        browserTask.Show();
                    }
                }
                IsAdDisplayed = true;
            }
            else
            {
                OnError(TapItResource.AdPromptData);
            }
        }

        public override async Task<bool> Load()
        {
            bool retVal = await base.Load();

            if (retVal)
            {
                Debug.WriteLine("_adPrompt_ContentLoaded");

                base.OnContentLoad(this, null);
            }

            return retVal;
        }

        // for Orientation change event
        public override void DeviceOrientationChanged(PageOrientation pageOrientation)
        {
            //base.DeviceOrientationChanged(pageOrientation);
            //SetAdPromptPosition();
        }


        #endregion

    }
}
