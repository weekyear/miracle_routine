using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using miracle_routine.Droid.BroadcastReceivers;
using miracle_routine.Droid.Services;
using miracle_routine.Helpers;
using miracle_routine.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AlarmSetterAndroid))]
namespace miracle_routine.Droid.Services
{
    public class AlarmSetterAndroid : IAlarmSetter
    {
        public AlarmSetterAndroid() { }

        public static AlarmManager GetAlarmManager()
        {
            var manager = Application.Context.GetSystemService("alarm") as AlarmManager;

            return manager;
        }

        public void SetRoutineAlarm(Routine routine)
        {
            var diffMillis = CalculateTimeDiff(routine);

            SetAlarmByManager(routine, diffMillis);
        }

        public void DeleteRoutineAlarm(int id)
        {
            DeleteAlarmByManager(id);
            NotificationSetterAndroid.CancelRoutineNotification(Application.Context, id);
        }

        public void DeleteAllRoutineAlarms(IEnumerable<Routine> routines)
        {
            routines.ForEach((routine) => DeleteRoutineAlarm(routine.Id));
        }

        public static void SetRepeatAlarm(Routine routine)
        {
            long diffMillis = 0;

            if (DaysOfWeek.GetHasADayBeenSelected(routine.Days))
            {
                diffMillis = CalculateTimeDiff(routine);
            }

            if (diffMillis != 0)
            {
                SetAlarmByManager(routine, diffMillis);
            }
        }

        private static long CalculateTimeDiff(Routine routine)
        {
            var nextAlarmDateTime = routine.NextAlarmTime;

            var diffTimeSpan = nextAlarmDateTime.Subtract(DateTime.Now);

            return (long)diffTimeSpan.TotalMilliseconds;
        }

        public static void SetAlarmByManager(Routine routine, long diffMillis)
        {
            var _alarmIntent = SetAlarmIntent(routine);

            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, routine.Id, _alarmIntent, PendingIntentFlags.UpdateCurrent);
            var alarmManager = (AlarmManager)Application.Context.GetSystemService("alarm");

            Intent showIntent = new Intent(Application.Context, typeof(MainActivity));
            PendingIntent showOperation = PendingIntent.GetActivity(Application.Context, routine.Id, showIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager.AlarmClockInfo alarmClockInfo = new AlarmManager.AlarmClockInfo(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + diffMillis, showOperation);
            alarmManager.SetAlarmClock(alarmClockInfo, pendingIntent);
        }

        private static Intent SetAlarmIntent(Routine routine)
        {
            var _alarmIntent = new Intent(Application.Context, typeof(AlarmReceiver));
            _alarmIntent.SetFlags(ActivityFlags.IncludeStoppedPackages);
            _alarmIntent.PutExtra("id", routine.Id);
            _alarmIntent.PutExtra("name", routine.Name);
            return _alarmIntent;
        }

        public static void DeleteAlarmByManager(int id)
        {
            var alarmIntent = new Intent(Application.Context, typeof(AlarmReceiver));
            alarmIntent.SetFlags(ActivityFlags.IncludeStoppedPackages);
            alarmIntent.PutExtra("id", id);

            var alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            var toDeletePendingIntent = PendingIntent.GetBroadcast(Application.Context, id, alarmIntent, PendingIntentFlags.UpdateCurrent);
            alarmManager.Cancel(toDeletePendingIntent);
        }
    }
}