using miracle_routine.Helpers;
using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class HabitSettingViewModel : BaseViewModel
    {
        public HabitSettingViewModel(INavigation navigation, Habit habit) : base(navigation)
        {
            Habit = new Habit(habit);

            ConstructCommand();
        }

        private void ConstructCommand()
        {
            SaveCommand = new Command(async () => await Save());
            CloseCommand = new Command(async () => await ClosePopup());
        }

        #region PROPERTY
        public Habit Habit { get; set; }

        public string Name
        {
            get { return Habit.Name; }
            set
            {
                if (Habit.Name == value) return;
                Habit.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        
        public int Minutes
        {
            get { return Habit.Minutes; }
            set
            {
                if (Habit.Minutes == value) return;
                Habit.Minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public int Seconds
        {
            get { return Habit.Seconds; }
            set
            {
                if (Habit.Seconds == value) return;
                Habit.Seconds = value;
                OnPropertyChanged(nameof(Seconds));
            }
        }

        public Command SaveCommand { get; set; }
        public Command CloseCommand { get; set; }

        #endregion

        #region METHOD
        private async Task Save()
        {
            Habit.Seconds = Habit.Seconds * 10;
            DependencyService.Get<MyMessagingCenter>().SendChangeHabitMessage(Habit);

            await ClosePopup();
        }

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync(true);
        }
        #endregion
    }
}
