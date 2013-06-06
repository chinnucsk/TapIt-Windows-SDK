using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapIt_WP8
{
    public class BannerAdView : AdView
    {
        public BannerAdView()
        {
            SetAdType();
            Width = 320;
            Height = 50;
            ZoneId = 1;
        }

        protected override void SetAdType()
        {
            Adtype = AdType.Banner_Ad;
        }
    }
}
