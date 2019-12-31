using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Converters
{
    public class BooleanSwitchThumbColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanToColor(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            if (color == Color.FromHex("#8c7b75"))
            {
                return true;
            }
            else if (color == Color.LightGray)
            {
                return false;
            }
            return null;
        }
        private object BooleanToColor(object value)
        {
            var isActive = (bool)value;
            if (isActive)
            {
                return Color.FromHex("#8c7b75");
            }
            else
            {
                return Color.LightGray;
            }
        }
    }
}
