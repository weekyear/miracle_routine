using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }
        private void ConstructCommand()
        {
            CloseCommand = new Command(async () => await ClosePopup());
            SaveCommand = new Command(async () => await Save());
        }

        #region PROPERTY
        public Command CloseCommand { get; set; }
        public Command SaveCommand { get; set; }

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

        #endregion

        #region METHOD

        private async Task Save()
        {
            Name = Name.TrimStart().TrimEnd();

            if (string.IsNullOrEmpty(Name))
            {
                await Application.Current.MainPage.DisplayAlert("", StringResources.ForgotRoutineName, StringResources.OK);
            }
            else
            {
                App.RoutineService.SaveRoutine(Routine);

                await ClosePopup();
            }
        }

        private async Task ClosePopup()
        {
            await Navigation.PopAsync(true);
        }

        #endregion
    }
}
