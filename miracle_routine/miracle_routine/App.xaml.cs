using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using miracle_routine.Services;
using miracle_routine.Views;
using miracle_routine.Repositories;
using miracle_routine.Helpers;
using Plugin.SharedTransitions;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace miracle_routine
{
    public partial class App : Application
    {
        public static ItemDatabaseGeneric ItemDatabase { get; } = new ItemDatabaseGeneric(new Database().DBConnect());

        public App()
        {
            InitializeComponent();

            DependencyService.Register<INavigation>();
            DependencyService.Register<INotifySetter>();
            DependencyService.Register<IAlarmSetter>();
            DependencyService.Register<MessageBoxService>();

            DependencyService.Get<IAdMobInterstitial>().Start();

            MainPage = new SharedTransitionNavigationPage(new RoutinesPage());

            if (Preferences.Get("StartRoutineId", 0) != 0)
            {
                var routine = RoutineService.GetRoutine(Preferences.Get("StartRoutineId", 0));
                Current.MainPage.Navigation.PushAsync(new RoutineActionPage(routine, null));
                Preferences.Set("StartRoutineId", 0);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            //Handle when your app resumes
        }

        public static IHabitRepo HabitRepo { get; } = new HabitRepo(ItemDatabase);
        public static IHabitService HabitService { get; } = new HabitService(HabitRepo);
        public static IRoutineRepo RoutineRepo { get; } = new RoutineRepo(ItemDatabase);
        public static IRoutineService RoutineService { get; } = new RoutineService(RoutineRepo);

        private static IRecordRepo recordRepo;
        public static IRecordRepo RecordRepo
        {
            get
            {
                if (recordRepo == null) recordRepo = new RecordRepo(ItemDatabase);
                return recordRepo;
            }
        }
        public static MyMessagingCenter MessagingCenter { get; } = new MyMessagingCenter();
    }
}
