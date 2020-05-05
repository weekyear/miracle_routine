using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using miracle_routine.Droid.BroadcastReceivers;
using miracle_routine.Droid.Services;
using miracle_routine.Helpers;
using miracle_routine.Models;
using Xamarin.Essentials;
using Environment = System.Environment;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationSetterAndroid))]
namespace miracle_routine.Droid.Services
{
    public class NotificationSetterAndroid : INotifySetter
    {
        private static readonly string NOTIFICATION_CHANNEL_ID = "com.beside.miracle_routine";
        private static readonly string COUNT_NOTIFICATION_CHANNEL_ID = "com.beside.miracle_routine.count";
        private static readonly string channelName = "miracle_routine";
        private static readonly string countChannelName = "miracle_routine.count";
        private static readonly long[] vibrationPattern = new long[] { 0, 250, 250, 250 };

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

        public static void NotifyRoutineStart(Routine routine)
        {
            VibrateAndSoundNotificationByPreferences();

            SetNotificationManager().Notify(routine.Id, CreateForNotifyRoutineStart(routine));
        }

        public void NotifySoonFinishHabit(Habit habit, string nextHabitName)
        {
            VibrateAndSoundNotificationByPreferences();
            SetNotificationManager().Notify(98, CreateForNotifySoonFinishHabit(habit, nextHabitName));
        }

        public void NotifyFinishHabit(Habit habit, string nextHabitName)
        {
            VibrateAndSoundNotificationByPreferences();

            SetNotificationManager().Notify(98, CreateForNotifyFinishHabit(habit, nextHabitName));
        }

        public void NotifyHabitCount(Habit habit, TimeSpan countDown, bool isPause, bool isLastHabit)
        {
            SetCountNotificationManager();

            HabitCountNotification = CreateForNotifyHabitCount(habit, countDown, isPause, isLastHabit);

            StartCountService(false);
        }

        private static PendingIntent OpenAppIntent()
        {
            return PendingIntent.GetBroadcast(Application.Context, 100, CreateActionIntent("재시작", 0), PendingIntentFlags.OneShot);
        }

        private static Intent CreateActionIntent(string action, int id)
        {
            var actionIntent = new Intent(Application.Context, typeof(NotificationReceiver));

            actionIntent.SetFlags(ActivityFlags.IncludeStoppedPackages);
            actionIntent.SetAction(action);
            actionIntent.PutExtra("id", id);

            return actionIntent;
        }


        private static Notification CreateForNotifyRoutineStart(Routine routine)
        {
            var context = Application.Context;

            var wise_sayings = context.Resources.GetStringArray(Resource.Array.wise_sayings);
            var random = new Random();

            string title = $"{routine.Name} 시작!";
            string message = wise_sayings[random.Next(wise_sayings.Length)];

            var style = new NotificationCompat.BigTextStyle();
            style.BigText(message);

            Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            var actionIntent2 = CreateActionIntent("루틴 시작", routine.Id);
            var pIntent2 = PendingIntent.GetBroadcast(context, routine.Id, actionIntent2, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID);
            var notification = notificationBuilder.SetOngoing(false)
                    .SetSmallIcon(Resource.Drawable.ic_miracle_routine_mini)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetPriority((int)NotificationImportance.High)
                    .SetVisibility(NotificationCompat.VisibilityPublic)
                    .SetSound(alarmSound)
                    .SetVibrate(vibrationPattern)
                    .SetAutoCancel(true)
                    .SetStyle(style)
                    .SetContentIntent(pIntent2)
                    .AddAction(0, "루틴 시작", pIntent2)
                    .Build();

            return notification;
        }

        private Notification CreateForNotifySoonFinishHabit(Habit habit, string nextHabitName)
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
                    .SetVibrate(vibrationPattern)
                    .SetContentIntent(OpenAppIntent())
                    .SetAutoCancel(true)
                    .Build();

            return notification;
        }

        private Notification CreateForNotifyFinishHabit(Habit habit, string nextHabitName)
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
                    .SetVibrate(vibrationPattern)
                    .SetContentIntent(OpenAppIntent())
                    .SetAutoCancel(true)
                    .Build();

            return notification;
        }
        private Notification CreateForNotifyHabitCount(Habit habit, TimeSpan countDown, bool isPause, bool isLastHabit)
        {
            var context = Application.Context;
            string title = $"{habit.Name}";
            string message = $"{CreateTimeToString.TimeCountToString(countDown)}";

            var fileName = habit.Image.Replace(".png", string.Empty);
            var imageId = context.Resources.GetIdentifier(fileName, "drawable", context.PackageName);

            string Btn1String;
            if (isPause)
            {
                Btn1String = "카운트";
            }
            else
            {
                Btn1String = "일시정지";
            }

            string Btn2String;
            if (isLastHabit)
            {
                Btn2String = "완료";
            }
            else
            {
                Btn2String = "다음";
            }

            var actionIntent1 = CreateActionIntent("일시정지", habit.Id);
            var pIntent1 = PendingIntent.GetBroadcast(context, 100, actionIntent1, PendingIntentFlags.OneShot);

            var actionIntent2 = CreateActionIntent(Btn2String, habit.Id);
            var pIntent2 = PendingIntent.GetBroadcast(context, 100, actionIntent2, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(context, COUNT_NOTIFICATION_CHANNEL_ID);
            var notification = notificationBuilder.SetOngoing(false)
                    .SetSmallIcon(imageId)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetVibrate(new long[] { -1 })
                    .SetPriority((int)NotificationImportance.Default)
                    .SetVisibility(NotificationCompat.VisibilityPublic)
                    .SetContentIntent(OpenAppIntent())
                    .SetAutoCancel(false)
                    .AddAction(0, Btn1String, pIntent1)
                    .AddAction(0, Btn2String, pIntent2)
                    .Build();

            return notification;
        }

        public void CancelFinishHabitNotify()
        {
            NotificationManager manager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.Cancel(98);
        }

        public void CancelHabitCountNotify()
        {
            StartCountService(true);
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


        private static void VibrateAndSoundNotificationByPreferences()
        {
            var am = Application.Context.GetSystemService(Context.AudioService) as AudioManager;

            switch (am.RingerMode)
            {
                case RingerMode.Normal:
                    VibrateWhenNotified();
                    break;
                case RingerMode.Vibrate:
                    PlayAudio();
                    break;
                case RingerMode.Silent:
                    PlayAudio();
                    VibrateWhenNotified();
                    break;
            }
        }

        private static void PlayAudio()
        {
            if (Preferences.Get("IsSound", false))
            {
                var _mediaPlayer = new MediaPlayer();

                if (_mediaPlayer.IsPlaying)
                    _mediaPlayer.Stop();

                _mediaPlayer.Reset();

                _mediaPlayer.SetDataSource(Application.Context, RingtoneManager.GetDefaultUri(RingtoneType.Notification));

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    _mediaPlayer.SetAudioAttributes(new AudioAttributes.Builder()
                        .SetUsage(AudioUsageKind.Alarm)
                        .SetContentType(AudioContentType.Sonification)
                        .Build());
                }
                else
                {
                    _mediaPlayer.SetAudioStreamType(Stream.Alarm);
                }

                _mediaPlayer.SetVolume(0.5f, 0.5f);
                _mediaPlayer.Looping = false;
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
            }
        }

        private static void VibrateWhenNotified()
        {
            if (Preferences.Get("IsVibrate", false))
            {
                var vibrator = (Vibrator)Application.Context.GetSystemService(Context.VibratorService);
                vibrator.Vibrate(VibrationEffect.CreateWaveform(vibrationPattern, -1));
            }
        }
        public static void StartCountService(bool isFinished)
        {
            Intent serviceIntent = new Intent(Application.Context, typeof(RoutineForegroundService));
            serviceIntent.PutExtra("isFinished", isFinished);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Application.Context.StartForegroundService(serviceIntent);
            }
            else
            {
                Application.Context.StartService(serviceIntent);
            }
        }

        public void NotifyFinishRoutine(Routine routine, TimeSpan ElapsedTime)
        {
            VibrateAndSoundNotificationByPreferences();

            SetNotificationManager().Notify(98, CreateForNotifyFinishHabit(routine, ElapsedTime));
        }

        private Notification CreateForNotifyFinishHabit(Routine routine, TimeSpan ElapsedTime)
        {
            var context = Application.Context;
            string title = $"{routine.Name} 완료";
            string message = $"{CreateTimeToString.TakenTimeToString(ElapsedTime)} 경과";

            var notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID);
            var notification = notificationBuilder.SetOngoing(false)
                    .SetSmallIcon(Resource.Drawable.ic_miracle_routine_mini)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetPriority((int)NotificationImportance.High)
                    .SetVisibility(NotificationCompat.VisibilityPublic)
                    .SetVibrate(vibrationPattern)
                    .SetContentIntent(OpenAppIntent())
                    .SetAutoCancel(true)
                    .Build();

            return notification;
        }
    }
}