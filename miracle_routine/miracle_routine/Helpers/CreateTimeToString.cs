using miracle_routine.Models;
using miracle_routine.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace miracle_routine.Helpers
{
    public class CreateTimeToString
    {
        public static string CreateTimeRemainingString(DateTime dateTime)
        {
            var diff = dateTime.Subtract(DateTime.Now);

            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ko-KR":
                    return CreateTimeRemainingString_ko_KR(dateTime, diff);
                case "en-US":
                    return CreateTimeRemainingString_en_US(dateTime, diff);
                default:
                    return CreateTimeRemainingString_en_US(dateTime, diff);
            }
        }

        private static string CreateTimeRemainingString_ko_KR(DateTime dateTime, TimeSpan diff)
        {
            if (diff.Days > 0)
            {
                return $"{dateTime.Month}월 {dateTime.Day}일 {dateTime.ToString("tt")} {dateTime.Hour}:{dateTime.ToString("mm")}에 모닝 루틴을 실행합니다!";
            }
            else if (diff.Hours > 0)
            {
                return $"{diff.Hours}시간 {diff.Minutes + 1}분 후에 모닝 루틴을 실행합니다!";
            }
            else if (diff.Minutes > 0)
            {
                return $"{diff.Minutes + 1}분 후에 모닝 루틴을 실행합니다!";
            }
            else if (diff.Seconds > 0)
            {
                return $"{diff.Seconds}초 후에 모닝 루틴을 실행합니다!";
            }
            else
            {
                return "이미 지난 시간입니다.";
            }
        }
        private static string CreateTimeRemainingString_en_US(DateTime dateTime, TimeSpan diff)
        {
            if (diff.Days > 0)
            {
                return $"Alarm set for  {dateTime.Hour}:{dateTime.ToString("mm")} {dateTime.ToString("tt")} on {dateTime.DayOfWeek}, {dateTime.Month} {dateTime.Day}";
            }
            else if (diff.Hours > 0)
            {
                return $"Alarm set for {diff.Hours} hours {diff.Minutes + 1} minutes from now.";
            }
            else if (diff.Minutes > 0)
            {
                return $"Alarm set for {diff.Minutes + 1} minutes from now.";
            }
            else if (diff.Seconds > 0)
            {
                return $"Alarm set for {diff.Seconds} seconds from now.";
            }
            else
            {
                return "이미 지난 시간입니다.";
            }
        }
        public static string TimeCountToString(TimeSpan timeSpan)
        {
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


        public static string TakenTimeToString(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.MinValue)
            {
                return "None";
            }

            var stringBuilder = new StringBuilder();

            if (timeSpan.Hours != 0) stringBuilder.Append($"{timeSpan.Hours}{StringResources.Hours}");

            if (timeSpan.Minutes != 0) stringBuilder.Append($" {timeSpan.Minutes}{StringResources.Minute}");

            if (timeSpan.Seconds != 0) stringBuilder.Append($" {timeSpan.Seconds}{StringResources.Second}");

            return stringBuilder.ToString();
        }
        public static string TakenTimeToString_en(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.MinValue)
            {
                return "None";
            }

            var stringBuilder = new StringBuilder();

            if (timeSpan.Hours != 0) stringBuilder.Append($"{timeSpan.Hours}h");

            if (timeSpan.Minutes != 0) stringBuilder.Append($" {timeSpan.Minutes}m");

            if (timeSpan.Seconds != 0) stringBuilder.Append($" {timeSpan.Seconds}s");

            return stringBuilder.ToString();
        }
        public static string ConvertDayOfWeekToKorDayOfWeek(DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Mon";
                case DayOfWeek.Tuesday:
                    return "Tue";
                case DayOfWeek.Wednesday:
                    return "Wed";
                case DayOfWeek.Thursday:
                    return "Thu";
                case DayOfWeek.Friday:
                    return "Fri";
                case DayOfWeek.Saturday:
                    return "Sat";
                case DayOfWeek.Sunday:
                    return "Sun";
                default:
                    return "Mon";
            }
        }
        public static string ConvertDaysOfWeekToString(Routine routine)
        {
            var stringBuilder = new StringBuilder();

            var allDays = routine.Days.AllDays;
            var allDaysString = DaysOfWeek.AllDaysString;

            for (int i = 0; i < 7; i++)
            {
                if (allDays[i])
                {
                    stringBuilder.Append($", {allDaysString[i]}");
                }
            }

            stringBuilder.Remove(0, 2);

            if (stringBuilder.ToString() == $"{StringResources.Sunday}, {StringResources.Monday}, {StringResources.Tuesday}, {StringResources.Wednesday}, {StringResources.Thursday}, {StringResources.Friday}, {StringResources.Saturday}")
            {
                return StringResources.Everyday;
            }
            else if (stringBuilder.ToString() == $"{StringResources.Sunday}, {StringResources.Saturday}")
            {
                return StringResources.Weekend;
            }
            else if (stringBuilder.ToString() == $"{StringResources.Monday}, {StringResources.Tuesday}, {StringResources.Wednesday}, {StringResources.Thursday}, {StringResources.Friday}")
            {
                return StringResources.Weekday;
            }
            else
            {
                return stringBuilder.ToString();
            }
        }
    }
}
