using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using miracle_routine.Droid.Services;
using miracle_routine.Models;

namespace miracle_routine.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        private int id;
        public override void OnReceive(Context context, Intent intent)
        {
            var pm = context.GetSystemService(Context.PowerService) as PowerManager;
            var wl = pm.NewWakeLock(WakeLockFlags.ScreenBright | WakeLockFlags.AcquireCausesWakeup, "CountOver");
            wl.Acquire();
            wl.Release();

            var bundle = intent.Extras;

            id = bundle.GetInt("id", -100000);

            var routine = App.RoutineService.GetRoutine(id);

            AlarmSetterAndroid.SetRepeatAlarm(routine);
            NotificationSetterAndroid.NotifyRoutineStart(routine);
            if (!App.RecordRepo.RoutineRecordFromDB.Any(r => r.RoutineId == routine.Id))
            {
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord(routine, false));
            }
        }
    }
}