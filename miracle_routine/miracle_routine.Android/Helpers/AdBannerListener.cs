using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace miracle_routine.Droid.Helpers
{
    public class AdBannerListener : Android.Gms.Ads.AdListener
    {
        readonly AdView _bannerAd;

        public AdBannerListener(AdView bannerAd)
        {
            _bannerAd = bannerAd;
        }

        public override void OnAdLoaded()
        {
            base.OnAdLoaded();
        }

        public override void OnAdFailedToLoad(int errorCode)
        {
            base.OnAdFailedToLoad(errorCode);
        }
    }
}