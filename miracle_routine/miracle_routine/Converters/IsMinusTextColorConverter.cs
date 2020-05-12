using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Converters
{
    public class IsMinusTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IsMinusToColor(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private object IsMinusToColor(object value)
        {
            var isMinus = (bool)value;
            if (isMinus)
            {
                return Color.IndianRed;
            }
            else
            {
                return (Color)App.Current.Resources["DynamicPrimaryTextColor"];
            }
        }
    }
}
