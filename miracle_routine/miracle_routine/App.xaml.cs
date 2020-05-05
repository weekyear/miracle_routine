using Xamarin.Forms;
using miracle_routine.Services;
using miracle_routine.Views;
using miracle_routine.Repositories;
using miracle_routine.Helpers;
using Plugin.SharedTransitions;
using System;
using Xamarin.Essentials;
using miracle_routine.ViewModels;
using miracle_routine.Models;

namespace miracle_routine
{
    public partial class App : Application
    {
        private static ItemDatabaseGeneric itemDatabase;
        public static ItemDatabaseGeneric ItemDatabase 
        {
            get
            {
                if (itemDatabase == null)
                {
                    itemDatabase = new ItemDatabaseGeneric(new Database().DBConnect());
                }
                return itemDatabase;
            }
        } 

        public App()
        {
            InitializeComponent();

            DependencyService.Register<INavigation>();
            DependencyService.Register<INotifySetter>();
            DependencyService.Register<IAlarmSetter>();
            DependencyService.Register<MessageBoxService>();

            DependencyService.Get<IAdMobInterstitial>().Start();

            MainPage = new SharedTransitionNavigationPage(new RoutinesPage());

            Routine.IsInitFinished = true;
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            if (IsRoutining())
            {
                var routine = App.RoutineService.GetRoutine(RoutineActionViewModel.CurrentRoutineId);

                var habitTimeList = RoutineActionViewModel.HabitTimeList;

                RoutineActionViewModel.deviceTimer?.Stop();

                if (habitTimeList.Count > 0)
                {
                    var currentIndex = RoutineActionViewModel.CurrentIndex;

                    var routinePage = new RoutineActionPage(routine, currentIndex, habitTimeList[currentIndex]);
                    MainPage.Navigation.PushAsync(routinePage);
                    

                    for (int i = 0; i < currentIndex; i++)
                    {
                        var addedPage = new RoutineActionPage(routine, i, habitTimeList[i]);
                        MainPage.Navigation.InsertPageBefore(addedPage, routinePage);
                    }

                    if (RoutineActionViewModel.IsFinished == true)
                    {
                        RoutineActionViewModel.ShowNextHabitCommandByNotification.Execute(null);
                        RoutineActionViewModel.IsFinished = false;
                    }
                }
                else
                {
                    MainPage.Navigation.PushAsync(new RoutineActionPage(routine, -1));
                }
            }
        }

        private bool IsRoutining()
        {
            return RoutineActionViewModel.CurrentRoutineId != 0;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            //Handle when your app resumes
        }

        private static IHabitRepo habitRepo;
        public static IHabitRepo HabitRepo 
        {
            get
            {
                if (habitRepo == null) habitRepo = new HabitRepo(ItemDatabase);
                return habitRepo;
            }
        }
        
        private static IRecommendedHabitRepo recommendedHabitRepo;
        public static IRecommendedHabitRepo RecommendedHabitRepo
        {
            get
            {
                if (recommendedHabitRepo == null) recommendedHabitRepo = new RecommendedHabitRepo(ItemDatabase);
                return recommendedHabitRepo;
            }
        }

        private static IHabitService habitService;
        public static IHabitService HabitService
        {
            get
            {
                if (habitService == null) habitService = new HabitService(HabitRepo);
                return habitService;
            }
        }

        private static IRoutineRepo routineRepo;
        public static IRoutineRepo RoutineRepo
        {
            get
            {
                if (routineRepo == null)
                {
                    routineRepo = new RoutineRepo(ItemDatabase);
                }
                return routineRepo;
            }
        }
        private static IRoutineService routineService;
        public static IRoutineService RoutineService
        {
            get
            {
                if (routineService == null)
                {
                    routineService = new RoutineService(RoutineRepo);
                }
                return routineService;
            }
        } 

        private static IRecordRepo recordRepo;
        public static IRecordRepo RecordRepo
        {
            get
            {
                if (recordRepo == null) recordRepo = new RecordRepo(ItemDatabase);
                return recordRepo;
            }
        }

        private static MyMessagingCenter messagingCenter;
        public static MyMessagingCenter MessagingCenter
        {
            get
            {
                if (messagingCenter == null) messagingCenter = new MyMessagingCenter();
                return messagingCenter;
            }
        } 
    }
}
