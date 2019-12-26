using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace miracle_routine.Droid.Services
{
    [Service]
    public class RoutineForegroundService : Service
    {
        private int id;
        private string notificationType;
        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            StartForeground(99, NotificationSetterAndroid.HabitCountNotification);

            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnTaskRemoved(Intent intent)
        {
            base.OnTaskRemoved(intent);
            //do something you want
            //stop service
            this.StopSelf();
        }
    }
}