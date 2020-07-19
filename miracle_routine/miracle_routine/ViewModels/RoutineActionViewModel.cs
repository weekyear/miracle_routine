using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RoutineActionViewModel : BaseViewModel
    {
        public static DeviceTimer deviceTimer;

        public static int CurrentIndex { get; set; } = -1;
        public static List<TimeSpan> HabitTimeList { get; } = new List<TimeSpan>();
        public static List<TimeSpan> ElapsedTimeList { get; } = new List<TimeSpan>();
        public static int CurrentRoutineId = 0;
        public static bool IsFinished { get; set; } = false;


        private DateTime previousTime = DateTime.MinValue;
        public RoutineActionViewModel(INavigation navigation, Routine routine, int habitIndex, TimeSpan _currentHabitTime = new TimeSpan()) : base(navigation)
        {
            Routine = new Routine(routine);

            CurrentRoutineId = Routine.Id;
            HabitIndex = habitIndex;

            if (IsNotRoutineActionWhenRestart(_currentHabitTime))
            {
                if (habitIndex == -1)
                {
                    HabitIndex = 0;
                    CurrentIndex = HabitIndex;

                    if (!App.RecordRepo.RecordFromDB.Any(r => r.RoutineId == CurrentRoutineId && r.RecordTime.Date == DateTime.Now.Date))
                    {
                        App.RecordRepo.SaveRecord(new Record(routine, false));
                    }

                    AskIfYouWantStart();
                }
                else
                {
                    CurrentIndex = HabitIndex;
                    IsCounting = true;
                }
            }
            else
            {
                CurrentHabitTime = _currentHabitTime;

                ElapsedTime = ElapsedTimeList[HabitIndex];

                if (HabitIndex == CurrentIndex) IsCounting = true;
            }

            ConstructCommand();
        }

        private bool IsNotRoutineActionWhenRestart(TimeSpan _currentHabitTime)
        {
            return _currentHabitTime == new TimeSpan();
        }

        private void ConstructCommand()
        {
            UndoCommand = new Command(() => Close());
            CloseAllCommand = new Command(() => CloseAll());
            ShowNextHabitCommand = new Command(async () => await ShowNextHabit());
            ClickPlayCommand = new Command(() => ClickPlay());

            if (HabitIndex == CurrentIndex)
            {
                ConstuctNotifictionCommand();
            }
        }


        private void ConstuctNotifictionCommand()
        {
            ShowNextHabitCommandByNotification = new Command(async () =>
            {
                await ShowNextHabit();
            });
            ClickPlayCommandByNotification = new Command(() => ClickPlay());
        }

        private void SetTimer()
        {
            if (HabitTimeList.Count <= CurrentIndex)
            {
                HabitTimeList.Add(CurrentHabitTime);
                ElapsedTimeList.Add(ElapsedTime);
            }

            void action()
            {
                if (previousTime == DateTime.MinValue) previousTime = DateTime.Now;

                var currentTime = DateTime.Now;

                var diffTimeSpan = currentTime.Subtract(previousTime);

                var oldHabitTime = CurrentHabitTime;
                ElapsedTime = ElapsedTime.Add(diffTimeSpan);
                CurrentHabitTime = CurrentHabitTime.Add(-diffTimeSpan);

                if (oldHabitTime.Seconds != CurrentHabitTime.Seconds)
                {
                    Console.WriteLine($"{ElapsedTime.ToString(@"mm\:ss")}");

                    RefreshHabitAndElapsedTimeList();

                    if (!IsSoonFinishTime)
                    {
                        DependencyService.Get<IAlarmSetter>().SetCountAlarm(CurrentHabitTime.Add(TimeSpan.FromSeconds(-10)));
                    }

                    DependencyService.Get<INotifySetter>().NotifyHabitCount(CurrentHabit, CurrentHabitTime, false, !IsNotLastHabit);

                    if (CurrentHabit.TotalTime.TotalSeconds > 20 && CurrentHabitTime.TotalSeconds < 11 && !IsSoonFinishTime)
                    {
                        DependencyService.Get<INotifySetter>().NotifySoonFinishHabit(CurrentHabit, NextHabitName);
                        IsSoonFinishTime = true;
                    }

                    if (CurrentHabitTime.TotalSeconds < 1 && !IsMinusHabitTime)
                    {
                        DependencyService.Get<INotifySetter>().NotifyFinishHabit(CurrentHabit, NextHabitName);
                        IsMinusHabitTime = true;

                        if (Preferences.Get("IsAutoFlipHabit", false)) ShowNextHabitCommand.Execute(null);
                    };
                }

                previousTime = currentTime;
            }

            void stoppingaAction()
            {
                IsCounting = false;
                DependencyService.Get<INotifySetter>().CancelHabitCountNotify();
                DependencyService.Get<INotifySetter>().CancelFinishHabitNotify();
            }

            deviceTimer = new DeviceTimer(action, TimeSpan.FromSeconds(0.1), true, true, stoppingaAction);
        }

        private void RefreshHabitAndElapsedTimeList()
        {
            if (CurrentIndex != -1)
            {
                if (HabitTimeList.Count > CurrentIndex)
                {
                    HabitTimeList.RemoveAt(CurrentIndex);
                    HabitTimeList.Insert(CurrentIndex, CurrentHabitTime);
                    ElapsedTimeList.RemoveAt(CurrentIndex);
                    ElapsedTimeList.Insert(CurrentIndex, ElapsedTime);
                }
            }
        }

        #region PROPERTY00000.0

        public Command UndoCommand { get; set; }
        public Command CloseAllCommand { get; set; }
        public Command ShowNextHabitCommand { get; set; }
        public static Command ShowNextHabitCommandByNotification { get; set; }
        public Command ShowHabitSettingCommand { get; set; }
        public Command ClickPlayCommand { get; set; }
        public static Command ClickPlayCommandByNotification { get; set; }

        private bool isCounting = false;
        public bool IsCounting 
        {
            get { return isCounting; }
            set
            {
                if (isCounting == value) return;

                SetProperty(ref isCounting, value, nameof(IsCounting));

                if (isCounting)
                {
                    SetTimer();
                    deviceTimer?.Start();
                }
                else
                {
                    DependencyService.Get<IAlarmSetter>().DeleteCountAlarm();
                    DependencyService.Get<INotifySetter>().NotifyHabitCount(CurrentHabit, CurrentHabitTime, true, !IsNotLastHabit);
                    previousTime = DateTime.MinValue;
                    deviceTimer?.Pause();
                }
            }
        }

        public  Routine Routine
        {
            get; set;
        }

        public int TotalCount
        {
            get { return Routine.HabitList.Count; }
        }
        
        public int HabitIndex { get; set; }

        public int HabitNum
        {
            get { return HabitIndex + 1; }
        }

        public Habit CurrentHabit
        {
            get { return Routine.HabitList[HabitIndex]; }
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
                var nextIndex = HabitIndex + 1;
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
            get { return HabitNum < Routine.HabitList.Count ? true : false; }
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
                if (currentHabitTime == TimeSpan.MinValue) currentHabitTime = CurrentHabit.TotalTime.Add(TimeSpan.FromSeconds(0.9));
                return currentHabitTime;
            }
            set
            {
                SetProperty(ref currentHabitTime, value, nameof(CurrentHabitTime));
            }
        }

        public bool IsSoonFinishTime { get; set; } = false;
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


        public void Close()
        {
            if (HabitIndex == 0)
            {
                CloseAll();
            }
            else
            {
                Undo();
            }
        }
        private void Undo()
        {
            IsCounting = false;

            DependencyService.Get<MessageBoxService>().ShowConfirm(
                $"이전 습관",
                $"이전 습관으로 이동하시겠습니까?",
                () =>
                {
                    IsCounting = true;
                },
                async () =>
                {
                    CurrentIndex -= 1;
                    await Navigation.PopAsync(true);
                });
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
                    deviceTimer?.Stop();
                    await CloseAllNavigationPage();
                });
        }

        private async Task CloseAllNavigationPage()
        {
            CurrentIndex = -1;
            HabitTimeList.Clear();
            ElapsedTimeList.Clear();

            var existingPages = Navigation.NavigationStack.ToList();
            for (int i = 1; i < existingPages.Count - 1; i++)
            {
                Navigation.RemovePage(existingPages[i]);
            }

            await PopRoutineActionPage();
        }
        
        public async Task ShowNextHabit()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                deviceTimer?.Stop();

                if (IsNotLastHabit)
                {
                    await Navigation.PushAsync(new RoutineActionPage(Routine, HabitIndex + 1), true);
                }
                else
                {
                    var record = App.RecordRepo.RecordFromDB.FirstOrDefault(r => r.RoutineId == Routine.Id && r.RecordTime.Date == DateTime.Now.Date);

                    if (record == null)
                    {
                        record = new Record(Routine, true);
                    }
                    else
                    {
                        record.IsSuccess = true;
                    }

                    App.RecordRepo.SaveRecord(record);

                    AlertFinishRoutine();

                    if (Preferences.Get("IsAutoFlipHabit", false)) DependencyService.Get<INotifySetter>().NotifyFinishRoutine(Routine, ElapsedTime);
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

        public void ClickPlay()
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
                    await PopRoutineActionPage();
                },
                () =>
                {
                    IsCounting = true;
                });
        }

        private void AlertFinishRoutine()
        {
            DependencyService.Get<MessageBoxService>().ShowAlert(
                $"{Routine.Name} 루틴 완료!",
                $"총 소요 시간 : {CreateTimeToString.TakenTimeToString(ElapsedTime)}",
                async () =>
                {
                    await CloseAllNavigationPage();
                    //DependencyService.Get<INotifySetter>().CancelHabitCountNotify();
                    DependencyService.Get<INotifySetter>().CancelFinishHabitNotify();
                    //DependencyService.Get<IAdMobInterstitial>().Show();
                });
        }

        private async Task PopRoutineActionPage()
        {
            CurrentRoutineId = 0;
            await Navigation.PopAsync(true);
        }

        #endregion
    }
}
