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
using System.Timers;
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
            SetHabitTimer();
        }
        private void ConstructCommand()
        {
        }

        private void SubscribeMessage()
        {
            CloseCommand = new Command(async () => await ClosePopup());
            ShowNextHabitCommand = new Command(async () => await ShowNextHabit());
        }

        private void SetHabitTimer()
        {
            HabitTimer.Elapsed += HabitTimer_Elapsed;
            TotalTimer.Elapsed += TotalTimer_Elapsed;
        }

        private void HabitTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var currentHabitTimeSeconds = CurrentHabitTime.TotalSeconds - 1;

            CurrentHabitTime = TimeSpan.FromSeconds(currentHabitTimeSeconds);

            if (currentHabitTimeSeconds == 0)
            {
                DependencyService.Get<INotifySetter>().NotifyFinishHabit(CurrentHabit, NextHabitName);
            }

            if (currentHabitTimeSeconds < 0 && !IsMinusHabitTime)
            {
                IsMinusHabitTime = true;
            }
        }
        
        private void TotalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var totalTimeSeconds = ElapsedTime.TotalSeconds + 1;

            ElapsedTime = TimeSpan.FromSeconds(totalTimeSeconds);
        }

        #region PROPERTY
        public Command CloseCommand { get; set; }
        public Command ShowNextHabitCommand { get; set; }
        public Command ShowHabitSettingCommand { get; set; }

        private Timer HabitTimer { get; } = new Timer()
        {
            Enabled = true,
            Interval = 1000
        };

        private Timer TotalTimer { get; } = new Timer()
        {
            Enabled = true,
            Interval = 1000
        };

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

        private TimeSpan currentHabittime = TimeSpan.MinValue;
        public TimeSpan CurrentHabitTime
        {
            get
            {
                if (currentHabittime == TimeSpan.MinValue) currentHabittime = CurrentHabit.Time;
                return currentHabittime;
            }
            set
            {
                SetProperty(ref currentHabittime, value, nameof(CurrentHabitTime));
            }
        }

        public bool IsMinusHabitTime { get; set; } = false;

        public TimeSpan ElapsedTime
        {
            get
            {
                return Routine.ElapsedTime;
            }
            set
            {
                if (Routine.ElapsedTime == value) return;
                Routine.ElapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }

        public string DoneBtnName
        {
            get
            {
                if (IsNotLastHabit)
                {
                    return StringResources.Next;
                }
                else
                {
                    return StringResources.Complete;
                }
            }
        }

        #endregion

        #region METHOD

        private async Task ClosePopup()
        {
            Application.Current.MainPage = new NavigationPage(new RoutinesPage());
        }
        
        private async Task ShowNextHabit()
        {
            if (IsNotLastHabit)
            {
                await Navigation.PushAsync(new RoutineActionPage(Routine, CurrentIndex + 1));
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new RoutinesPage());
            }
        }


        #endregion
    }
}
