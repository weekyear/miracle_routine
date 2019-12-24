using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using miracle_routine.Droid.Helpers;
using miracle_routine.Droid.Services;
using miracle_routine.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(AdMobInterstitialAndroid))]
namespace miracle_routine.Droid.Services
{
    public class AdMobInterstitialAndroid : IAdMobInterstitial
    {
        InterstitialAd _ad;
        private AdRequest requestBuilder;

        public void Show(string adUnit)
        {
            var context = Application.Context;
            _ad = new InterstitialAd(context)
            {
                AdUnitId = adUnit
            };

            var intlistener = new InterstitialAdListener(_ad);
            intlistener.OnAdLoaded();
            _ad.AdListener = intlistener;

            requestBuilder = new AdRequest.Builder().Build();

            //CreateRequestBuilderWhenTest();

            _ad.LoadAd(requestBuilder);
        }

        [Conditional("DEBUG")]
        private void CreateRequestBuilderWhenTest()
        {
            requestBuilder.Dispose();
            requestBuilder = new AdRequest.Builder().AddTestDevice("FA3E0133F649B126EB4B86A6DA3E60D2").Build();
        }
    }
}