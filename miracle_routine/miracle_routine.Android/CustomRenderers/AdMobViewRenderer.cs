using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using miracle_routine.CustomControls;
using miracle_routine.Droid.CustomRenderers;
using miracle_routine.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobViewRenderer))]
namespace miracle_routine.Droid.CustomRenderers
{
    public class AdMobViewRenderer : ViewRenderer<AdMobView, AdView>, IAdListener
    {
        AdView adView;
        public AdMobViewRenderer(Context context) : base(context) { }

        public AdView AdView => adView;
        private AdRequest requestBuilder;

        protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control == null)
                SetNativeControl(CreateAdView());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(AdView.AdUnitId))
                Control.AdUnitId = Element.AdUnitId;
        }

        private AdView CreateAdView()
        {
            adView = new AdView(Context)
            {
                AdSize = AdSize.Banner,
                AdUnitId = Element.AdUnitId
            };

            int heightPixels = AdSize.Banner.GetHeightInPixels(Context);
            adView.SetMinimumHeight(heightPixels);

            adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            adView.AdListener = new Helpers.AdListener(this);

            requestBuilder = new AdRequest.Builder().Build();

            CreateRequestBuilderWhenTest();

            adView.LoadAd(requestBuilder);

            requestBuilder.Dispose();

            return adView;
        }

        [Conditional("DEBUG")]
        private void CreateRequestBuilderWhenTest()
        {
            requestBuilder.Dispose();
            requestBuilder = new AdRequest.Builder().AddTestDevice("FA3E0133F649B126EB4B86A6DA3E60D2").Build();
        }
    }
}