using Android.Gms.Ads;
using miracle_routine.Droid.Services;

namespace miracle_routine.Droid.Helpers
{
    public class InterstitialAdListener : AdListener
    {
        readonly AdMobInterstitialAndroid _interstitialAd;

        public InterstitialAdListener(AdMobInterstitialAndroid interstitialAd)
        {
            _interstitialAd = interstitialAd;
        }

        public override void OnAdClosed()
        {
            _interstitialAd.LoadAd();
        }
    }
}