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
using Android.Support.V4.Content;
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
        private static readonly string COUNT_NOTIFICATION_CHANNEL_ID = "com.beside.miracle_routine.count";
        private static readonly string channelName = "miracle_routine";
        private static readonly string countChannelName = "miracle_routine.count";
        private static readonly long[] vibrationPattern = new long[] { 500, 800, 1000, 1000 };

        public static Notification HabitCountNotification;

        private static NotificationManager SetNotificationManager()
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

        private static NotificationManager SetCountNotificationManager()
        {
            var manager = Application.Context.GetSystemService("notification") as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var chan = new NotificationChannel(COUNT_NOTIFICATION_CHANNEL_ID, countChannelName, NotificationImportance.Default)
                {
                    LockscreenVisibility = NotificationVisibility.Public,
                    Importance = NotificationImportance.Low
                };

                manager?.CreateNotificationChannel(chan);
            }

            return manager;
        }

        private static void StartService(string notificationType, int id)
        {
            Intent serviceIntent = new Intent(Application.Context, typeof(RoutineForegroundService));
            serviceIntent.PutExtra("notificationType", notificationType);
            serviceIntent.PutExtra("id", id);


            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Application.Context.StartForegroundService(serviceIntent);
            }
            else
            {
                Application.Context.StartService(serviceIntent);
            }
        }


        public static void NotifyRoutineStart(Routine routine)
        {
            VibrateWhenNotified();
            SetNotificationManager().Notify(routine.Id, CreateForNotifyRoutineStart(routine));
        }

        public void NotifySoonFinishHabit(Habit habit, string nextHabitName)
        {
            VibrateWhenNotified();
            SetNotificationManager().Notify(98, CreateForNotifySoonFinishHabit(habit, nextHabitName));
        }

        public void NotifyFinishHabit(Habit habit, string nextHabitName)
        {
            VibrateWhenNotified();
            SetNotificationManager().Notify(98, CreateForNotifyFinishHabit(habit, nextHabitName));
        }

        public void NotifyHabitCount(Habit habit, TimeSpan countDown)
        {
            SetCountNotificationManager();

            HabitCountNotification = CreateForNotifyHabitCount(habit, countDown);

            StartService("HabitCountNotification", 99);
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

        private static void VibrateWhenNotified()
        {
            var vibrator = (Vibrator)Application.Context.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(VibrationEffect.CreateWaveform(vibrationPattern, -1));
        }

        public void CancelFinishHabitNotify()
        {
            NotificationManager manager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.Cancel(98);
        }

        public void CancelHabitCountNotify()
        {
            NotificationManager manager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.Cancel(99);
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


        public static Notification CreateForNotifyRoutineStart(Routine routine)
        {
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
                    .SetSmallIcon(Resource.Drawable.ic_miracle_routine_mini)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetPriority((int)NotificationImportance.High)
                    .SetVisibility(NotificationCompat.VisibilityPublic)
                    .SetSound(alarmSound)
                    .SetAutoCancel(false)
                    .SetContentIntent(pIntent2)
                    .AddAction(0, "취소", pIntent1)
                    .AddAction(0, "시작", pIntent2)
                    .Build();

            return notification;
        }

        public static Notification CreateForNotifySoonFinishHabit(Habit habit, string nextHabitName)
        {
            var context = Application.Context;
            string title = $"{habit.Name}가 곧 완료됩니다.";
            string message = $"{habit.Name}을 마무리하시고 다음 습관 {nextHabitName}을 시작할 준비해주세요~";

            if (nextHabitName == "더 수행할 습관이 없습니다.")
            {
                message = nextHabitName;
            }

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

            return notification;
        }

        public static Notification CreateForNotifyFinishHabit(Habit habit, string nextHabitName)
        {
            var context = Application.Context;
            string title = $"{habit.Name} 완료";
            string message = $"다음 습관 {nextHabitName}을 시작해주세요~";

            if (nextHabitName == "더 수행할 습관이 없습니다.")
            {
                message = nextHabitName;
            }

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

            return notification;
        }
        public static Notification CreateForNotifyHabitCount(Habit habit, TimeSpan countDown)
        {
            var context = Application.Context;
            string title = $"{habit.Name}";
            string message = $"{CreateTimeToString.TimeCountToString(countDown)}";

            var fileName = habit.Image.Replace(".png", string.Empty);
            var imageId = context.Resources.GetIdentifier(fileName, "drawable", context.PackageName);

            var notificationBuilder = new NotificationCompat.Builder(context, COUNT_NOTIFICATION_CHANNEL_ID);
            var notification = notificationBuilder.SetOngoing(true)
                    .SetSmallIcon(imageId)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetVibrate(new long[] { -1 })
                    .SetPriority((int)NotificationImportance.Default)
                    .SetVisibility(NotificationCompat.VisibilitySecret)
                    .SetContentIntent(OpenAppIntent())
                    .SetAutoCancel(true)
                    .Build();

            return notification;
        }
    }
}