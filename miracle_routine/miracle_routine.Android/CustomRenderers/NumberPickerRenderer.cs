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
using NumberPicker = Android.Widget.NumberPicker;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using static Android.Widget.NumberPicker;
using System.ComponentModel;
using miracle_routine.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Graphics;

[assembly: ExportRenderer(typeof(miracle_routine.CustomControls.NumberPicker), typeof(NumberPickerRenderer))]
namespace miracle_routine.Droid.CustomRenderers
{
    public class NumberPickerRenderer : ViewRenderer<CustomControls.NumberPicker, NumberPicker>
    {
        private Context _context;
        private string[] seconds = new string[] { "00", "10", "20", "30", "40", "50" };
        public NumberPickerRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomControls.NumberPicker> e)
        {
            base.OnElementChanged(e);

            var numberPicker = new NumberPicker(_context)
            {
                ScaleX = (float)1.3,
                ScaleY = (float)1.3
            };
            SetDividerColor(numberPicker);
            SetTextColor(numberPicker);

            numberPicker.WrapSelectorWheel = false;

            switch (Element.PickerType)
            {
                case "Minutes":
                    numberPicker.MaxValue = 59;
                    numberPicker.MinValue = 0;

                    numberPicker.Value = Element.Value;
                    break;
                case "Seconds":
                    numberPicker.MaxValue = 5;
                    numberPicker.MinValue = 0;
                    var secondsArray = seconds;
                    numberPicker.SetDisplayedValues(secondsArray);

                    numberPicker.Value = Element.Value;
                    break;
            }

            numberPicker.ValueChanged += NumberPicker_ValueChanged;

            SetNativeControl(numberPicker);
        }

        private void SetDividerColor(NumberPicker picker)
        {
            try
            {
                var numberPickerType = Java.Lang.Class.FromType(typeof(NumberPicker));
                var divider = numberPickerType.GetDeclaredField("mSelectionDivider");
                divider.Accessible = true;

                var dividerDraw = new ColorDrawable(Android.Graphics.Color.Transparent);
                divider.Set(picker, dividerDraw);
            }
            catch
            {
                // ignored
            }
        }

        private void SetTextColor(NumberPicker picker)
        {

            Android.Graphics.Color color = new Android.Graphics.Color(27, 27, 27);
            if (Preferences.Get("IsDarkTheme", false))
            {
                color = new Android.Graphics.Color(245, 245, 245);
            }
            try
            {
                var selectorWheelPaintField = picker.Class.GetDeclaredField("mSelectorWheelPaint");
                selectorWheelPaintField.Accessible = true;

                ((Paint)selectorWheelPaintField.Get(picker)).Color = color;
            }
            catch
            {
                // ignored
            }

            int count = picker.ChildCount;
            for (int i = 0; i < count; i++)
            {
                Android.Views.View child = picker.GetChildAt(i);
                if (child is EditText)
                {
                    ((EditText)child).SetTextColor(color);
                }
                picker.Invalidate();
            }
        }

        private void NumberPicker_ValueChanged(object sender, ValueChangeEventArgs e)
        {
            Element.Value = e.NewVal;
            var numberPicker = sender as NumberPicker;
            SetTextColor(numberPicker);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Value")
                Control.Value = Element.Value;
        }
    }
}