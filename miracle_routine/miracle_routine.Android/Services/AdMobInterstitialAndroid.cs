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
        public InterstitialAd _interstitialAd;

        public void Start()
        {
            var context = Application.Context;
            _interstitialAd = new InterstitialAd(context)
            {
                AdUnitId = "ca-app-pub-8413101784746060/9891550667"
            };

            var adListener = new InterstitialAdListener(this);

            _interstitialAd.AdListener = adListener;
            
            LoadAd();
        }

        public void LoadAd()
        {
            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
            //CreateRequestBuilderWhenTest();
        }

        public void Show()
        {
            if (_interstitialAd.IsLoaded) _interstitialAd.Show();
            else LoadAd();
        }

        [Conditional("DEBUG")]
        private void CreateRequestBuilderWhenTest()
        {
            _interstitialAd.LoadAd(new AdRequest.Builder().AddTestDevice("FA3E0133F649B126EB4B86A6DA3E60D2").Build());
        }
    }
}