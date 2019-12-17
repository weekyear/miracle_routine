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
                OpenMainActivity(context, bundle);
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