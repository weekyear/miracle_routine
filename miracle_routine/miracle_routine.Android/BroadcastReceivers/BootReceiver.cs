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
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted, "android.intent.action.ACTION_BOOT_COMPLETED" })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine("OnReceive_BootReceiver_00");
            if (intent.Action.Equals(Intent.ActionBootCompleted) || intent.Action.Equals("android.intent.action.ACTION_BOOT_COMPLETED"))
            {
                Console.WriteLine("OnReceive_BootReceiver_01");
                AlarmSetterAndroid.SetAllAlarmWhenRestart();
            }
        }
    }
}