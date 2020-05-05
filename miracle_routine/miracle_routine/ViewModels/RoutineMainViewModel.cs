using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using miracle_routine.Models;
using miracle_routine.Views;
using miracle_routine.Helpers;
using Plugin.SharedTransitions;
using miracle_routine.Resources;

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
            ShowMenuCommand = new Command(async () => await ShowMenu());
        }

        private void SubscribeMessage()
        {
            MessagingCenter.Subscribe<MyMessagingCenter>(this, "changeRoutine", (sender) =>
            {
                RefreshRoutines();
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
        public Command ShowMenuCommand { get; set; }



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
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutineSettingPage(routine)));
        }


        private async Task ShowMenu()
        {
            string[] actionSheetBtns = { StringResources.IconCopyright, StringResources.AppSettings };

            string action = await DependencyService.Get<MessageBoxService>().ShowActionSheet(StringResources.Menu, StringResources.Cancel, null, actionSheetBtns);

            if (action != StringResources.Cancel && !string.IsNullOrEmpty(action))
            {
                await ClickMenuAction(action);
            }
        }

        private async Task ClickMenuAction(string action)
        {
            if (action == StringResources.IconCopyright)
            {
                await Application.Current.MainPage.DisplayAlert(StringResources.IconCopyright, "https://icons8.com", StringResources.OK);
            }
            else if (action == StringResources.AppSettings)
            {
                await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new SettingsPage()));
            }
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