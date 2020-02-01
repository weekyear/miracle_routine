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

namespace miracle_routine.Droid.Services
{
    [Service]
    public class RoutineForegroundService : Service
    {
        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var bundle = intent.Extras;
            var isFinished = bundle.GetBoolean("isFinished", false);

            if (isFinished)
            {
                StopSelf();
                return StartCommandResult.NotSticky;
            }
            else
            {
                StartForeground(99, NotificationSetterAndroid.HabitCountNotification);
                return StartCommandResult.Sticky;
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnTaskRemoved(Intent intent)
        {
            Console.WriteLine("OnTaskRemoved_Foreground");
            base.OnTaskRemoved(intent);
            //do something you want
            //stop service
        }
    }
}