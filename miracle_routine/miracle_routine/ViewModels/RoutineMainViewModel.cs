using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using miracle_routine.Models;
using miracle_routine.Views;
using miracle_routine.Helpers;
using Plugin.SharedTransitions;

namespace miracle_routine.ViewModels
{
    public class RoutineMainViewModel : BaseViewModel
    {
        public RoutineMainViewModel(INavigation navigation) : base(navigation)
        {
            ConstructCommand();
            SubscribeMessage();
        }
        private void ConstructCommand()
        {
            ShowRoutineCommand = new Command(async () => await ShowRoutine());
        }

        private void SubscribeMessage()
        {
            MessagingCenter.Subscribe<MyMessagingCenter>(this, "changeRoutine", (sender) =>
            {
                RefreshRoutines();
            });
            
            MessagingCenter.Subscribe<MyMessagingCenter, Routine>(this, "showRoutine",  async (sender, routine) =>
            {
                await NavigateRoutinePage(routine);
            });
            
            MessagingCenter.Subscribe<MyMessagingCenter, Routine>(this, "showRoutineAction",  async (sender, routine) =>
            {
                await NavigateRoutineActionPage(routine);
            });
            
            MessagingCenter.Subscribe<MyMessagingCenter, Routine>(this, "showRoutineRecord",  async (sender, routine) =>
            {
                await NavigateRoutineRecordPage(routine);
            });
            
            MessagingCenter.Subscribe<MyMessagingCenter, Habit>(this, "showHabitRecord",  async (sender, habit) =>
            {
                await NavigateHabitRecordPage(habit);
            });
        }

        private ObservableCollection<Routine> routines;
        public ObservableCollection<Routine> Routines
        {
            get
            {
                if (routines == null)
                {
                    var _routines = App.RoutineService.Routines;
                    routines = Helper.ConvertIEnuemrableToObservableCollection(_routines);
                }
                return routines;
            }
            set
            {
                if (routines == value) return;
                routines = value;
                OnPropertyChanged(nameof(Routines));
            }
        }
        public bool HasRoutine
        {
            get { return Routines.Count != 0 ? true : false; }
        }
        public bool HasNoRoutine
        {
            get { return !HasRoutine; }
        }

        public Command ShowRoutineCommand { get; set; }
        private async Task ShowRoutine()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                await NavigateRoutinePage(new Routine());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateRoutinePage(Routine routine)
        {
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutinePage(routine)));
        }

        private async Task NavigateRoutineActionPage(Routine routine)
        {
            await Navigation.PushAsync(new RoutineActionPage(routine, null), true);
        }
        
        private async Task NavigateRoutineRecordPage(Routine routine)
        {
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutineRecordPage(routine)));
        }

        private async Task NavigateHabitRecordPage(Habit habit)
        {
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new HabitRecordPage(habit)));
        }

        public void RefreshRoutines()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                var _routines = App.RoutineService.Routines;
                routines = Helper.ConvertIEnuemrableToObservableCollection(_routines);
                OnPropertyChanged(nameof(Routines));
                OnPropertyChanged(nameof(HasRoutine));
                OnPropertyChanged(nameof(HasNoRoutine));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}