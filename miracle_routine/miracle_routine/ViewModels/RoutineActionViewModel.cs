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

            if (index == 0)
            {
                AskIfYouWantStart();
            }
            else
            {
                SetTimer();
            }
        }
        private void ConstructCommand()
        {
        }

        private void SubscribeMessage()
        {
            CloseCommand = new Command(async () => await ClosePopup());
            ShowNextHabitCommand = new Command(async () => await ShowNextHabit());
            ClickPlayCommand = new Command(() => ClickPlay());
        }

        private void SetTimer()
        {
            Device.StartTimer(new TimeSpan(0,0,1), () =>
            {
                if (!IsCounting) return IsCounting;

                var currentHabitTimeSeconds = CurrentHabitTime.TotalSeconds - 1;
                var totalTimeSeconds = ElapsedTime.TotalSeconds + 1;

                CurrentHabitTime = TimeSpan.FromSeconds(currentHabitTimeSeconds);
                ElapsedTime = TimeSpan.FromSeconds(totalTimeSeconds);

                DependencyService.Get<INotifySetter>().NotifyHabitCount(CurrentHabit, CurrentHabitTime);

                if (currentHabitTimeSeconds == 0)
                {
                    DependencyService.Get<INotifySetter>().NotifyFinishHabit(CurrentHabit, NextHabitName);
                }

                if (currentHabitTimeSeconds < 0 && !IsMinusHabitTime)
                {
                    IsMinusHabitTime = true;
                }
                return IsCounting;
            });
        }

        #region PROPERTY
        public Command CloseCommand { get; set; }
        public Command ShowNextHabitCommand { get; set; }
        public Command ShowHabitSettingCommand { get; set; }
        public Command ClickPlayCommand { get; set; }

        public bool IsCounting { get; set; } = true;

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
            IsCounting = false;

            DependencyService.Get<INotifySetter>().CancelHabitCountNotify();
            DependencyService.Get<INotifySetter>().CancelFinishHabitNotify();
            var existingPages = Navigation.NavigationStack.ToList();
            for (int i = 1; i < existingPages.Count - 1; i++)
            {
                Navigation.RemovePage(existingPages[i]);
            }
            await Navigation.PopAsync(true);
        }
        
        private async Task ShowNextHabit()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                IsCounting = false;
                if (IsNotLastHabit)
                {
                    await Navigation.PushAsync(new RoutineActionPage(Routine, CurrentIndex + 1), true);
                    DependencyService.Get<INotifySetter>().CancelFinishHabitNotify();
                }
                else
                {
                    AlertFinishRoutine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        private void ClickPlay()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                IsCounting = !IsCounting;
                if (IsCounting) SetTimer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AskIfYouWantStart()
        {
            DependencyService.Get<MessageBoxService>().ShowConfirm(
                $"{Routine.Name} 루틴 시작",
                $"예상 소요 시간 {CreateTimeToString.TimeToString(Routine.TotalTime)}",
                async () => 
                {
                    await ClosePopup();
                },
                () =>
                {
                    SetTimer();
                });
        }

        private void AlertFinishRoutine()
        {
            DependencyService.Get<MessageBoxService>().ShowAlert(
                $"{Routine.Name} 루틴 완료!",
                $"총 소요 시간 : {CreateTimeToString.TimeToString(ElapsedTime)}",
                async () =>
                {
                    await ClosePopup();
                });
        }

        #endregion
    }
}
