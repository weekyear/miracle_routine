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
using miracle_routine.Views;
using Plugin.SharedTransitions;
using Xamarin.Essentials;

namespace miracle_routine.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    public class NotificationReceiver : BroadcastReceiver
    {
        private int id;

        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine("OnReceive_NotificationReceiver");
            var bundle = intent.Extras;
            id = bundle.GetInt("id", 0);

            CancelNotification(context);

            //context.GetString(Resource.String.GoOffNow);

            if (intent.Action == "취소")
            {
            }
            else if (intent.Action == "시작")
            {
                try
                {
                    Preferences.Set("StartRoutineId", id);

                    OpenMainActivity(context, bundle);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.InnerException);
                }
            }
            else if (intent.Action == "입력")
            {
                OpenMainActivity(context, bundle);
            }
        }

        private void OpenMainActivity(Context context, Bundle bundle)
        {
            var disIntent = new Intent(context, typeof(MainActivity));
            disIntent.SetFlags(ActivityFlags.NewTask);
            disIntent.PutExtra("id", id);

            context.StartActivity(disIntent);
        }


        private void CancelNotification(Context context)
        {
            NotificationManager manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            if (id != -100000)
            {
                Console.WriteLine("CancelNotification_NotificationReceiver");
                manager.Cancel(id);
            }
        }
    }
}