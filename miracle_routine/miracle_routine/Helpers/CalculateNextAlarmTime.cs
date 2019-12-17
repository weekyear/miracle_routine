using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Helpers
{
    public class CalculateNextAlarmTime
    {
        public static DateTime NextAlarmTime(Routine routine)
        {
            var nextDate = CalculateNextDate(routine);
            var nextTime = routine.StartTime;

            var nextAlarmDateTime = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, nextTime.Hours, nextTime.Minutes, 0);

            return nextAlarmDateTime;
        }

        private static DateTime CalculateNextDate(Routine routine)
        {
            return DateTime.Now.Date.AddDays(CalculateAddingDaysWhenHasDaysOfWeek(routine));
        }

        public static double CalculateAddingDaysWhenHasDaysOfWeek(Routine routine)
        {
            var allDays = routine.Days.AllDays;

            int addingDays = 8;
            int diffDays;

            bool isPastTime = routine.StartTime.Subtract(DateTime.Now.TimeOfDay).Ticks < 0;

            for (int i = 0; i < 7; i++)
            {
                if (allDays[i])
                {
                    var today = (int)DateTime.Now.DayOfWeek;

                    if (isPastTime)
                    {
                        diffDays = i - today > 0 ? i - today : i - today + 7;
                    }
                    else
                    {
                        diffDays = i - today >= 0 ? i - today : i - today + 7;
                    }

                    if (addingDays > diffDays)
                    {
                        addingDays = diffDays;
                    }
                }
            }

            return addingDays;
        }
    }
}
