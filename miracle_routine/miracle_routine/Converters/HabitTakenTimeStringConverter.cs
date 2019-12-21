using miracle_routine.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Converters
{
    public class HabitTakenTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeValue = value as string;
            TimeSpan _time = TimeSpan.Parse(timeValue);
            return _time;
        }

        private object TimeToString(object value)
        {
            var timeSpan = (TimeSpan)value;

            if (timeSpan == TimeSpan.MinValue)
            {
                return "None";
            }

            if (timeSpan.Minutes == 0)
            {
                return $"{timeSpan.Seconds}{StringResources.Second}";
            }
            else if (timeSpan.Seconds == 0)
            {
                return $"{timeSpan.Minutes}{StringResources.Minute}";
            }
            else
            {
                return $"{timeSpan.Minutes}{StringResources.Minute} {timeSpan.Seconds}{StringResources.Second}";
            }
        }
    }
}
