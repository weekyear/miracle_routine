using Android.Content;
using Android.OS;

namespace miracle_routine.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    public class CountAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var pm = context.GetSystemService(Context.PowerService) as PowerManager;
            var wl = pm.NewWakeLock(WakeLockFlags.ScreenBright | WakeLockFlags.AcquireCausesWakeup, "CountOver");
            wl.Acquire();
            wl.Release();
        }
    }
}