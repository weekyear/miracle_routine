using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.CustomControls
{
    public class NumberPicker : View
    {
        public NumberPicker() { }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value),
                typeof(int),
                typeof(NumberPicker),
                0,
                BindingMode.TwoWay);

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly BindableProperty PickerTypeProperty =
            BindableProperty.Create(nameof(PickerType),
                typeof(string),
                typeof(NumberPicker),
                "null",
                BindingMode.TwoWay);

        public string PickerType
        {
            get
            {
                return (string)GetValue(PickerTypeProperty);
            }
            set
            {
                SetValue(PickerTypeProperty, value);
            }
        }
    }
}
