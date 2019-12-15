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
using miracle_routine.CustomControls;
using miracle_routine.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoRippleListView), typeof(NoRippleListViewRenderer))]
namespace miracle_routine.Droid.CustomRenderers
{
    public class NoRippleListViewRenderer : ListViewRenderer
    {

        public NoRippleListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            Control.SetSelector(Resource.Layout.no_selector);
        }
    }
}