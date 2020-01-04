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
using miracle_routine.Models;
using miracle_routine.ViewModels;
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

            if (intent.Action == "시작")
            {
                try
                {
                    OpenMainActivity(context);

                    App.Current.MainPage.Navigation.PushAsync(new RoutineActionPage(App.RoutineService.GetRoutine(id), 0));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.InnerException);
                }
            }
            else if (intent.Action == "일시정지")
            {
                try
                {
                    RoutineActionViewModel.ClickPlayCommandByNotification.Execute(null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.InnerException);
                }
            }
            else if (intent.Action == "다음")
            {
                try
                {
                    RoutineActionViewModel.ShowNextHabitCommandByNotification.Execute(null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.InnerException);
                }
            }
            else if (intent.Action == "완료")
            {
                try
                {
                    OpenMainActivity(context);

                    RoutineActionViewModel.ShowNextHabitCommandByNotification.Execute(null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.InnerException);
                }
            }
        }

        private void OpenMainActivity(Context context)
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