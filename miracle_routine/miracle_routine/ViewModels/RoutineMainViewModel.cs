using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using miracle_routine.Models;
using miracle_routine.Views;
using miracle_routine.Helpers;
using System.Linq;
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
            await NavigateRoutinePage(new Routine());
        }

        private async Task NavigateRoutinePage(Routine routine)
        {
            await Navigation.PushModalAsync(new NavigationPage(new RoutinePage(routine))
            {
                BarBackgroundColor = (Color) App.Current.Resources["Accent"]
            });
        }
        
        private async Task NavigateRoutineActionPage(Routine routine)
        {
            Application.Current.MainPage = new SharedTransitionNavigationPage(new RoutineActionPage(routine, 0));
        }

        public void RefreshRoutines()
        {
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