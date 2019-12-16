using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RoutineActionViewModel : BaseViewModel
    {
        public RoutineActionViewModel(INavigation navigation, Routine routine, int index) : base(navigation)
        {
            Routine = new Routine(routine);
            CurrentIndex = index;

            ConstructCommand();
            SubscribeMessage();
        }
        private void ConstructCommand()
        {
        }

        private void SubscribeMessage()
        {
            CloseCommand = new Command(async () => await ClosePopup());
            ShowNextHabitCommand = new Command(async () => await ShowNextHabit());
        }

        #region PROPERTY
        public Command CloseCommand { get; set; }
        public Command ShowNextHabitCommand { get; set; }
        public Command ShowHabitSettingCommand { get; set; }

        public Routine Routine
        {
            get; set;
        }

        public int TotalCount
        {
            get { return Routine.HabitList.Count; }
        }
        
        public int CurrentIndex
        {
            get; set;
        }

        public int CurrentCount
        {
            get { return CurrentIndex + 1; }
        }

        public Habit CurrentHabit
        {
            get { return Routine.HabitList[CurrentIndex]; }
        }

        public string HabitName
        {
            get { return CurrentHabit.Name; }
        }
        
        public Habit NextHabit
        {
            get 
            {
                var nextIndex = CurrentIndex + 1;
                if (nextIndex < Routine.HabitList.Count)
                {
                    return Routine.HabitList[nextIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        public string NextHabitName
        {
            get
            {
                if (IsNotLastHabit)
                {
                    return NextHabit.Name;
                }
                else
                {
                    return "더 수행할 습관이 없습니다.";
                }
            }
        }

        public bool IsNotLastHabit
        {
            get { return NextHabit != null ? true : false; }
        }

        public TimeSpan NextHabitTime
        {
            get
            {
                if (IsNotLastHabit)
                {
                    return NextHabit.Time;
                }
                else
                {
                    return TimeSpan.MinValue;
                }
            }
        }

        private TimeSpan currentHabittime;
        public TimeSpan CurrentHabitTime
        {
            get
            {
                if (currentHabittime == null) currentHabittime = CurrentHabit.Time;
                return currentHabittime;
            }
            set
            {
                SetProperty(ref currentHabittime, value, nameof(CurrentHabitTime));
            }
        }

        private TimeSpan totalTime;
        public TimeSpan TotalTime
        {
            get
            {
                if (totalTime == null) totalTime = new TimeSpan(0);
                return totalTime;
            }
            set
            {
                SetProperty(ref totalTime, value, nameof(TotalTime));
            }
        }

        #endregion

        #region METHOD

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync(true);
        }
        
        private async Task ShowNextHabit()
        {
            if (IsNotLastHabit)
            {
                await Navigation.PushModalAsync(new NavigationPage(new RoutineActionPage(Routine, CurrentIndex + 1)));
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new RoutinesPage());
            }
        }


        #endregion
    }
}
