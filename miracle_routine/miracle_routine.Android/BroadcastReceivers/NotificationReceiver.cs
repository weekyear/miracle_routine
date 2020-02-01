using System;
using Android.App;
using Android.Content;
using miracle_routine.ViewModels;
using miracle_routine.Views;

namespace miracle_routine.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    public class NotificationReceiver : BroadcastReceiver
    {
        private int id;

        public override void OnReceive(Context context, Intent intent)
        {
            var bundle = intent.Extras;
            id = bundle.GetInt("id", 0);

            try
            {
                if (intent.Action == "루틴 시작")
                {
                    OpenMainActivity(context);

                    App.Current.MainPage.Navigation.PushAsync(new RoutineActionPage(App.RoutineService.GetRoutine(id), -1));
                }
                else if (intent.Action == "일시정지")
                {
                    RoutineActionViewModel.ClickPlayCommandByNotification.Execute(null);
                }
                else if (intent.Action == "다음")
                {
                    RoutineActionViewModel.ShowNextHabitCommandByNotification.Execute(null);
                }
                else if (intent.Action == "완료")
                {
                    OpenMainActivity(context);

                    RoutineActionViewModel.IsFinished = true;

                    RoutineActionViewModel.ShowNextHabitCommandByNotification.Execute(null);
                }
                else if (intent.Action == "재시작")
                {
                    OpenMainActivity(context);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(intent.Action);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);

                if (ex is NullReferenceException && intent.Action == "루틴 시작")
                {
                    RoutineActionViewModel.CurrentRoutineId = id;
                }
            }
            finally
            {
                if (intent.Action == "루틴 시작")
                {
                    CancelRoutineStartNotification(context);
                }

                if (intent.Action == "재시작" || intent.Action == "완료")
                {
                    CancelNotification(context);
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
                manager.Cancel(99);
            }
        }

        private void CancelRoutineStartNotification(Context context)
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