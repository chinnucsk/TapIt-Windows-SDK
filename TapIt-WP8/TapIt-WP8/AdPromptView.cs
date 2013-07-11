using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

        #region Property

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

        #endregion

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
                        Debug.WriteLine("Adprompt_Navigating");
                        if (NavigationServiceRef != null)
                        {

                            string uri =  WebUtility.UrlEncode(JsonResponse.clickUrl);
                            NavigationServiceRef.Navigate(new Uri(string.Format("/TapIt-WP8;component/Resources/InAppBrowserPage.xaml?myparameter1={0}", uri), UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            // todo: raise error
                            OnError("Failed to navigate");
                        }
                    }
                }
                IsAdDisplayed = true;
            }
            else
            {
                OnError(TapItResource.AdPromptData);
            }
        }

        public override async Task<bool> Load(bool bRaiseError = true)
        {
            bool retVal = await base.Load(bRaiseError);

            if (retVal)
            {
                Debug.WriteLine("_adPrompt_ContentLoaded");

                base.OnContentLoad(this, null);
            }

            return retVal;
        }
        
        #endregion

    }
}
