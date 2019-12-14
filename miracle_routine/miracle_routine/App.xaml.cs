using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using miracle_routine.Services;
using miracle_routine.Views;
using miracle_routine.Repositories;
using miracle_routine.Helpers;

namespace miracle_routine
{
    public partial class App : Application
    {
        public static ItemDatabaseGeneric ItemDatabase { get; } = new ItemDatabaseGeneric(new Database().DBConnect());

        public App()
        {
            InitializeComponent();

            DependencyService.Register<INavigation>();
            DependencyService.Register<IAlarmSetter>();
            DependencyService.Register<MyMessagingCenter>();
            DependencyService.Register<MessageBoxService>();

            MainPage = new NavigationPage(new RoutinesPage());
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
            // Handle when your app resumes
        }
        public static IHabitRepo HabitRepo { get; } = new HabitRepo(ItemDatabase);
        public static IHabitService HabitService { get; } = new HabitService(HabitRepo);
        public static IRoutineRepo RoutineRepo { get; } = new RoutineRepo(ItemDatabase);
        public static IRoutineService RoutineService { get; } = new RoutineService(RoutineRepo);
    }
}
