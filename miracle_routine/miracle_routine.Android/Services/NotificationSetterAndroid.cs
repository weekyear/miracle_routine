using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using miracle_routine.Droid.BroadcastReceivers;
using miracle_routine.Droid.Services;
using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationSetterAndroid))]
namespace miracle_routine.Droid.Services
{
    public class NotificationSetterAndroid : INotifySetter
    {
        private static readonly string NOTIFICATION_CHANNEL_ID = "com.beside.miracle_routine";
        private static readonly string channelName = "miracle_routine";
        private static readonly long[] vibrationPattern = new long[] { 500, 800, 1000, 1000 };

        public static NotificationManager SetNotificationManager()
        {
            var manager = Application.Context.GetSystemService("notification") as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, channelName, NotificationImportance.High)
                {
                    LockscreenVisibility = NotificationVisibility.Public,
                    Importance = NotificationImportance.High
                };

                manager?.CreateNotificationChannel(chan);
            }

            return manager;
        }

        public static void NotifyRoutineStart(Routine routine)
        {
            var manager = SetNotificationManager();

            var context = Application.Context;

            string title = $"{routine.Name} 시작!";
            string message = $"{routine.Name} 루틴을 바로 시작해보세요!";

            Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            var actionIntent1 = CreateActionIntent("취소", routine.Id);
            var pIntent1 = PendingIntent.GetBroadcast(context, 100, actionIntent1, PendingIntentFlags.OneShot);

            var actionIntent2 = CreateActionIntent("시작", routine.Id);
            var pIntent2 = PendingIntent.GetBroadcast(context, 100, actionIntent2, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID);
            var notification = notificationBuilder.SetOngoing(true)
                    .SetSmallIcon(Resource.Mipmap.icon)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetPriority((int)NotificationImportance.High)
                    .SetVisibility(NotificationCompat.VisibilityPublic)
                    .SetSound(alarmSound)
                    .SetAutoCancel(false)
                    .AddAction(0, "취소", pIntent1)
                    .AddAction(0, "시작", pIntent2)
                    .Build();

            VibrateWhenNotified(context);

            manager.Notify(routine.Id, notification);
        }

        public void NotifyFinishHabit(Habit habit, string nextHabitName)
        {
            var manager = SetNotificationManager();

            var context = Application.Context;
            string title = $"{habit.Name} 완료";
            string message = $"다음 습관 {nextHabitName}을 시작해주세요~";

            Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            var fileName = habit.Image.Replace(".png", string.Empty);
            var imageId = context.Resources.GetIdentifier(fileName, "drawable", context.PackageName);

            var notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID);
            var notification = notificationBuilder.SetOngoing(false)
                    .SetSmallIcon(imageId)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetPriority((int)NotificationImportance.High)
                    .SetVisibility(NotificationCompat.VisibilityPublic)
                    .SetSound(alarmSound)
                    .SetAutoCancel(true)
                    .Build();

            VibrateWhenNotified(context);

            manager.Notify(98, notification);
        }

        public static void NotifyNextDailyReport(bool isFinalNotify)
        {
        }

        private static PendingIntent OpenAppIntent()
        {
            Intent notificationIntent = new Intent(Application.Context, typeof(MainActivity));

            notificationIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

            PendingIntent pendingIntent = PendingIntent.GetActivity(Application.Context, 0, notificationIntent, 0);

            return pendingIntent;
        }

        private static Intent CreateActionIntent(string action, int id)
        {
            var actionIntent = new Intent(Application.Context, typeof(NotificationReceiver));

            actionIntent.SetFlags(ActivityFlags.IncludeStoppedPackages);
            actionIntent.SetAction(action);
            actionIntent.PutExtra("id", id);

            return actionIntent;
        }

        private static void VibrateWhenNotified(Context context)
        {
            var vibrator = (Vibrator)context.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(VibrationEffect.CreateWaveform(vibrationPattern, -1));
        }

        public void CancelFinishHabitNotify()
        {
            NotificationManager manager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.Cancel(98);
        }

        public static void CancelRoutineNotification(Context context, int id)
        {
            NotificationManager manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            if (id != -100000)
            {
                manager.Cancel(id);
            }
        }
        public void DeleteAllNotifications()
        {
            var notificationManager = Application.Context.GetSystemService("notification") as NotificationManager;
            notificationManager.CancelAll();
        }
    }
}