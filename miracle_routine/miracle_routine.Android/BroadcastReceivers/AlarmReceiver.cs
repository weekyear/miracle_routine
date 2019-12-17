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

namespace miracle_routine.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        private int id;
        public override void OnReceive(Context context, Intent intent)
        {
            var bundle = intent.Extras;

            id = bundle.GetInt("id", -100000);

            var routine = App.RoutineService.GetRoutine(id);

            AlarmSetterAndroid.SetRepeatAlarm(routine);
            NotificationSetterAndroid.NotifyRoutineStart(routine);
        }
    }
}