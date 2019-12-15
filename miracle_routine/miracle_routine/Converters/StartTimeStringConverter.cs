using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Converters
{
    public class StartTimeStringConverter : IValueConverter
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
            var dateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, timeSpan.Hours, timeSpan.Minutes, 0);

            if (timeSpan == TimeSpan.MinValue)
            {
                return "None";
            }

            return dateTime.ToString("h:mm tt", CultureInfo.CurrentCulture);
        }
    }
}
