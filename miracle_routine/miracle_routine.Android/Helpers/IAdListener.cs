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
    public interface IAdListener
    {
        AdView AdView { get; }
    }
}