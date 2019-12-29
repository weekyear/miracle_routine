using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RoutineActionViewModel : BaseViewModel
    {
        public static DeviceTimer deviceTimer;
        private DateTime previousTime = DateTime.MinValue;
        public RoutineActionViewModel(INavigation navigation, Routine routine, List<HabitRecord> _habitRecords) : base(navigation)
        {
            Routine = new Routine(routine);

            if (_habitRecords != null)
            {
                HabitRecords = _habitRecords;
                CurrentIndex = _habitRecords.Count;
            }
            else
            {
                CurrentIndex = 0;
            }


            ConstructCommand();
            SubscribeMessage();

            if (CurrentIndex == 0)
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
            UndoCommand = new Command(async () => await ClosePopup());
            CloseAllCommand = new Command(() => CloseAll());
            ShowNextHabitCommand = new Command(async () => await ShowNextHabit());
            ClickPlayCommand = new Command(() => ClickPlay());
        }

        private void SetTimer()
        {
            void action()
            {
                if (IsCounting)
                {
                    var currentTime = DateTime.Now;
                    if (previousTime == DateTime.MinValue) previousTime = currentTime.AddSeconds(-1);

                    var diffTimeSpan = currentTime.Subtract(previousTime);

                    CurrentHabitTime = CurrentHabitTime.Add(-diffTimeSpan);
                    ElapsedTime = ElapsedTime.Add(diffTimeSpan);

                    DependencyService.Get<INotifySetter>().NotifyHabitCount(CurrentHabit, CurrentHabitTime);

                    if (CurrentHabit.TotalTime.TotalSeconds > 20 && CurrentHabitTime.TotalSeconds < 11)
                    {
                        DependencyService.Get<INotifySetter>().NotifySoonFinishHabit(CurrentHabit, NextHabitName);
                    }

                    if (CurrentHabitTime.TotalSeconds < 1 && !IsMinusHabitTime)
                    {
                        DependencyService.Get<INotifySetter>().NotifyFinishHabit(CurrentHabit, NextHabitName);
                        IsMinusHabitTime = true;
                    };

                    previousTime = currentTime;
                }
            }
            deviceTimer = new DeviceTimer(action, TimeSpan.FromSeconds(1), true, true);
        }

        #region PROPERTY
        public Command UndoCommand { get; set; }
        public Command CloseAllCommand { get; set; }
        public Command ShowNextHabitCommand { get; set; }
        public Command ShowHabitSettingCommand { get; set; }
        public Command ClickPlayCommand { get; set; }

        private bool isCounting = true;
        public bool IsCounting 
        {
            get { return isCounting; }
            set
            {
                if (isCounting == value) return;

                SetProperty(ref isCounting, value, nameof(IsCounting));

                if (isCounting)
                {
                    deviceTimer?.Start();
                }
                else
                {
                    deviceTimer?.Stop();
                }
            }
        }

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

        public string HabitDescription
        {
            get { return CurrentHabit.Description; }
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
                    return NextHabit.TotalTime;
                }
                else
                {
                    return TimeSpan.MinValue;
                }
            }
        }

        private TimeSpan currentHabitTime = TimeSpan.MinValue;
        public TimeSpan CurrentHabitTime
        {
            get
            {
                if (currentHabitTime == TimeSpan.MinValue) currentHabitTime = CurrentHabit.TotalTime;
                return currentHabitTime;
            }
            set
            {
                SetProperty(ref currentHabitTime, value, nameof(CurrentHabitTime));
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

        private List<HabitRecord> habitRecords = new List<HabitRecord>();
        public List<HabitRecord> HabitRecords
        {
            get { return habitRecords; }
            set
            {
                if (habitRecords == value) return;
                habitRecords = value;
                OnPropertyChanged(nameof(HabitRecords));
            }
        }

        private bool IsFinished { get; set; } = false;

        #endregion

        #region METHOD

        public async Task ClosePopup()
        {
            IsCounting = false;

            if (CurrentIndex == 0 || IsFinished)
            {
                DependencyService.Get<INotifySetter>().CancelHabitCountNotify();
                DependencyService.Get<INotifySetter>().CancelFinishHabitNotify();
                var existingPages = Navigation.NavigationStack.ToList();
                for (int i = 1; i < existingPages.Count - 1; i++)
                {
                    Navigation.RemovePage(existingPages[i]);
                }
            }

            await Navigation.PopAsync(true);
        }

        private void CloseAll()
        {
            IsCounting = false;

            DependencyService.Get<MessageBoxService>().ShowConfirm(
                $"루틴 종료",
                $"루틴을 종료하시겠습니까?", 
                () =>
                {
                    IsCounting = true;
                },
                async () =>
                {
                    IsFinished = true;
                    await ClosePopup();
                });
        }
        
        private async Task ShowNextHabit()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                IsCounting = false;

                if (CurrentIndex < HabitRecords.Count)
                {
                    var recordsCount = HabitRecords.Count;
                    for (int i = CurrentIndex; i < recordsCount; i++)
                    {
                        HabitRecords.RemoveAt(CurrentIndex);
                    }
                }

                HabitRecords.Add(new HabitRecord(CurrentHabit, CurrentHabitTime));

                if (IsNotLastHabit)
                {
                    await Navigation.PushAsync(new RoutineActionPage(Routine, HabitRecords), true);
                    DependencyService.Get<INotifySetter>().CancelFinishHabitNotify();
                }
                else
                {
                    foreach(var habitRec in HabitRecords)
                    {
                        App.RecordRepo.SaveHabitRecord(habitRec);
                    }

                    App.RecordRepo.SaveRoutineRecord(new RoutineRecord(Routine, ElapsedTime));
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
                $"예상 소요 시간 {CreateTimeToString.TakenTimeToString(Routine.TotalTime)}",
                async () =>
                {
                    await Navigation.PopAsync(true);
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
                $"총 소요 시간 : {CreateTimeToString.TakenTimeToString(ElapsedTime)}",
                async () =>
                {
                    IsFinished = true;
                    await ClosePopup();
                    DependencyService.Get<IAdMobInterstitial>().Show();
                });
        }

        #endregion
    }
}
