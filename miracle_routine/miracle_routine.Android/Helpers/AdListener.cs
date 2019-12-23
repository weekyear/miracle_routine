using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace miracle_routine.Droid.Helpers
{
    public class AdListener : Android.Gms.Ads.AdListener
    {
        private readonly IAdListener that;

        public AdListener(IAdListener t)
        {
            that = t;
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