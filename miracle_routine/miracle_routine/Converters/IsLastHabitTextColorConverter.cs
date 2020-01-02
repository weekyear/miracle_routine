using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Converters
{
    public class IsLastHabitTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BooleanToColor(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private object BooleanToColor(object value)
        {
            var isNotLastHabit = (bool)value;
            if (isNotLastHabit)
            {
                return (Color)App.Current.Resources["PrimaryDark"];
            }
            else
            {
                return (Color)App.Current.Resources["Accent"];
            }
        }
    }
}
