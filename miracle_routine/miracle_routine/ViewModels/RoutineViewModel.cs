using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.Views;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RoutineViewModel : BaseViewModel
    {
        public RoutineViewModel(INavigation navigation, Routine routine) : base(navigation)
        {
            Routine = new Routine(routine);

            ConstructCommand();
            SubscribeMessage();
        }
        private void ConstructCommand()
        {
            CloseCommand = new Command(async () => await ClosePopup());
            SaveCommand = new Command(async () => await Save());
            ShowHabitSettingCommand = new Command(async () => await ShowHabitSetting());
        }

        private void SubscribeMessage()
        {
            MessagingCenter.Subscribe<MyMessagingCenter, Habit>(this, "changeHabit", (sender, habit) =>
            {
                ChangeHabitList(habit);
            });
        }

        #region PROPERTY
        public Command CloseCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command ShowHabitSettingCommand { get; set; }

        public Routine Routine
        {
            get; set;
        }

        public string Name
        {
            get { return Routine.Name; }
            set
            {
                if (Routine.Name == value) return;
                Routine.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsLocation
        {
            get { return Routine.IsLocation; }
            set
            {
                if (Routine.IsLocation == value) return;
                Routine.IsLocation = value;
                OnPropertyChanged(nameof(IsLocation));
            }
        }

        public TimeSpan StartTime
        {
            get { return Routine.StartTime; }
            set
            {
                if (Routine.StartTime == value) return;
                Routine.StartTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        private ObservableCollection<Habit> habits;
        public ObservableCollection<Habit> Habits
        {
            get
            {
                if (habits == null) habits = Helper.ConvertIEnuemrableToObservableCollection(Routine.HabitList);
                return habits;
            }
        }

        public bool HasNoHabit
        {
            get
            {
                if (Habits.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public bool HasHabit
        {
            get
            {
                return !HasNoHabit;
            }
        }

        #endregion

        #region METHOD

        private async Task Save()
        {
            Name = Name.TrimStart().TrimEnd();

            if (string.IsNullOrEmpty(Name))
            {
                await Application.Current.MainPage.DisplayAlert("", StringResources.ForgotRoutineName, StringResources.OK);
            }
            else if (!DaysOfWeek.GetHasADayBeenSelected(Routine.Days))
            {
                await Application.Current.MainPage.DisplayAlert("", StringResources.ForgotRoutineDays, StringResources.OK);
            }
            else if (HasNoHabit)
            {
                await Application.Current.MainPage.DisplayAlert("", StringResources.ForgotHabit, StringResources.OK);
            }
            else
            {
                var id = App.RoutineService.SaveRoutine(Routine);

                foreach (var habit in Habits)
                {
                    habit.RoutineId = id;
                }
                App.HabitService.SaveHabits(Habits);

                await ClosePopup();

                var diffString = CreateTimeToString.CreateTimeRemainingString(Routine.NextAlarmTime);
                DependencyService.Get<IToastService>().Show(diffString);
            }
        }

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync(true);
        }
        
        private async Task ShowHabitSetting()
        {
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new HabitSettingPage(new Habit()))).ConfigureAwait(false);
        }

        private void ChangeHabitList(Habit habit)
        {
            if (habit.Index == -1)
            {
                habit.Index = Habits.Count;
                Habits.Add(habit);
            }
            else
            {
                var oldHabit = Habits.FirstOrDefault(h => h.Index == habit.Index);
                int i = Habits.IndexOf(oldHabit);
                Habits.Remove(oldHabit);
                Habits.Insert(i, habit);
            }
            OnPropertyChanged(nameof(HasNoHabit));
            OnPropertyChanged(nameof(HasHabit));
            OnPropertyChanged(nameof(Habits));
        }

        #endregion
    }
}
