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
using Plugin.CurrentActivity;

namespace miracle_routine.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    public class CountAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //CrossCurrentActivity.Current.Activity?.SetTurnScreenOn(true);
            var pm = context.GetSystemService(Context.PowerService) as PowerManager;
            var wl = pm.NewWakeLock(WakeLockFlags.ScreenBright | WakeLockFlags.AcquireCausesWakeup, "CountOver");
            wl.Acquire();
            wl.Release();
        }
    }
}