using miracle_routine.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Converters
{
    public class TakenTimeStringConverter : IValueConverter
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
            string timeString = timeSpan.ToString(@"mm\:ss");

            if (timeSpan == TimeSpan.MinValue)
            {
                return "";
            }

            if (timeSpan.Hours > 0) timeString = timeSpan.ToString(@"hh\:mm\:ss");

            if (timeSpan.TotalSeconds < 0)
            {
                timeString = $"+ {timeString}";
            }

            return timeString;
        }
    }
}
