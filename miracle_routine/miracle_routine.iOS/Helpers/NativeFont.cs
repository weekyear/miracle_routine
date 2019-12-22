using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using miracle_routine.Helpers;
using miracle_routine.iOS.Helpers;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NativeFont))]
namespace miracle_routine.iOS.Helpers
{
    public class NativeFont : INativeFont
    {
        public float GetNativeSize(float size)
        {
            return size * (float)UIScreen.MainScreen.Scale;
        }
    }
}