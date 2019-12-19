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

        public string Image
        {
            get { return Habit.Image; }
            set
            {
                if (Habit.Image == value) return;
                Habit.Image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        
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
            get 
            {
                if (Habit.Seconds >= 10)
                {
                    return Habit.Seconds / 10;
                }
                return Habit.Seconds; 
            }
            set
            {
                if (Habit.Seconds == value) return;
                Habit.Seconds = value;
                OnPropertyChanged(nameof(Seconds));
            }
        }

        public List<Habit> RecommendeHabitList
        {
            get
            {
                return new List<Habit>
                {
                    new Habit(){ Name = "물 마시기", Image = "ic_water.png", Minutes = 0, Seconds = 30 },
                    new Habit(){ Name = "차 마시기", Image = "ic_tea.png", Minutes = 3, Seconds = 0 },
                    new Habit(){ Name = "이불 개기", Image = "ic_bed.png", Minutes = 1, Seconds = 0 },
                    new Habit(){ Name = "기지개 펴기", Image = "ic_stretching.png", Minutes = 0, Seconds = 30 },
                    new Habit(){ Name = "명상", Image = "ic_meditation.png", Minutes = 10, Seconds = 0 },
                    new Habit(){ Name = "일정 세우기", Image = "ic_todo.png", Minutes = 10, Seconds = 0 },
                    new Habit(){ Name = "일기 쓰기", Image = "ic_diary.png", Minutes = 10, Seconds = 0 },
                    new Habit(){ Name = "아침 독서", Image = "ic_reading.png", Minutes = 30, Seconds = 0 },
                    new Habit(){ Name = "무산소 운동", Image = "ic_exercise.png", Minutes = 2, Seconds = 0 },
                    new Habit(){ Name = "조깅하기", Image = "ic_jogging.png", Minutes = 40, Seconds = 0 },
                    new Habit(){ Name = "과일 챙겨먹기", Image = "ic_apple.png", Minutes = 5, Seconds = 0 },
                    new Habit(){ Name = "약 복용", Image = "ic_pills.png", Minutes = 1, Seconds = 0 },
                    new Habit(){ Name = "식사", Image = "ic_meal.png", Minutes = 30, Seconds = 0 },
                    new Habit(){ Name = "샤워", Image = "ic_shower.png", Minutes = 30, Seconds = 0 }
                };
            }
        }

        public Command SaveCommand { get; set; }
        public Command CloseCommand { get; set; }

        #endregion

        #region METHOD
        private async Task Save()
        {
            if (Seconds < 10)
            {
                Habit.Seconds = Habit.Seconds * 10;
            }
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
