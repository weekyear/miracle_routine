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

            if (timeSpan == TimeSpan.MinValue)
            {
                return "None";
            }


            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ko-KR":
                    return $"{timeSpan.Minutes}분 {timeSpan.Seconds}초";
                case "en-US":
                    return $"{timeSpan.Minutes}min {timeSpan.Seconds}sec)";
                default:
                    return $"{timeSpan.Minutes}min {timeSpan.Seconds}sec)";
            }
        }
    }
}
