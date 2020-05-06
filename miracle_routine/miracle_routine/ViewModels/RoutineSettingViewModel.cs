using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.Views;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RoutineSettingViewModel : BaseViewModel
    {
        public RoutineSettingViewModel(INavigation navigation, Routine routine) : base(navigation)
        {
            Routine = new Routine(routine);

            habits = null;

            ConstructCommand();
        }
        private void ConstructCommand()
        {
            CloseCommand = new Command(async () => await ClosePopup());
            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(() => Delete());
            AddMiracleMorningCommand = new Command(() => AddMiracleMorning());
            ShowHabitSettingCommand = new Command(async () => await ShowHabitSetting());
        }

        #region PROPERTY
        public Command CloseCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command AddMiracleMorningCommand { get; set; }
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

        public IEnumerable<Habit> OldHabits { get; set; }

        public static OrderableCollection<Habit> habits;
        public OrderableCollection<Habit> Habits
        {
            get
            {
                if (habits == null)
                {
                    var orderedHabits = AssignIndexToHabits(Routine.HabitList.OrderBy(t => t.Index));
                    habits = Helper.ConvertIEnuemrableToObservableCollection(orderedHabits);
                    OldHabits = new List<Habit>(habits);
                }
                return habits;
            }
        }

        public static List<Habit> HabitsForDelete { get; } = new List<Habit>();

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


        public List<Habit> MiracleMorningList
        {
            get
            {
                return new List<Habit>
                {
                    new Habit(){ Name = "명상", Description="침묵의 시간 동안 마음을 조용히 가라앉히고 내 문제들에 대한 걱정을 멈춥니다.", Image = "ic_meditation.png", Minutes = 3, Seconds = 0 },
                    new Habit(){ Name = "'확신의 말' 말하기", Description="나의 '확신의 말'을 되새기면서 내 잠재의식에 변화를 줍니다.", Image = "ic_speak.png", Minutes = 3, Seconds = 0 },
                    new Habit(){ Name = "'최고의 나' 상상하기", Description="내가 이뤄낼 성과를 그려보는 것은 현실에서도 좋은 결과를 만들어 냅니다.", Image = "ic_think.png", Minutes = 3, Seconds = 0 },
                    new Habit(){ Name = "일기 쓰기", Description="직접 적어보면서 생각을 명확히 정리하고 새로운 영감을 얻게 해줍니다.", Image = "ic_diary.png", Minutes = 10, Seconds = 0 },
                    new Habit(){ Name = "아침 독서", Description="독서는 삶을 변화시키는 가장 빠른 길입니다.", Image = "ic_reading.png", Minutes = 30, Seconds = 0 },
                    new Habit(){ Name = "무산소 운동", Description="기초대사량을 증가시켜 살이 찌지 않는 체질로 만들어줍니다.", Image = "ic_exercise.png", Minutes = 10, Seconds = 0 },
                };
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
                if (Routine.Id == 0) Routine.Index = App.RoutineService.Routines.Count;
                var id = App.RoutineService.SaveRoutine(Routine);

                foreach (var habit in Habits)
                {
                    habit.RoutineId = id;
                }
                App.HabitService.SaveHabits(Habits);

                foreach(var habit in HabitsForDelete)
                {
                    if (habit.Id != 0)
                    {
                        App.HabitService.DeleteHabit(habit.Id);
                    }
                }

                await ClosePopup();

                var diffString = CreateTimeToString.CreateTimeRemainingString(Routine.NextAlarmTime);
                DependencyService.Get<IToastService>().Show(diffString);
            }
        }

        private void Delete()
        {
            async void deleteAction() => await DeleteRoutineAndHabitList();
            DependencyService.Get<MessageBoxService>().ShowConfirm(StringResources.DeleteRoutine, StringResources.AskDeleteRoutine, null, deleteAction);
        }
        private async Task DeleteRoutineAndHabitList()
        {
            if (Routine.Id != 0)
            {
                foreach (var habit in Routine.HabitList)
                {
                    App.HabitService.DeleteHabit(habit.Id);
                }
                App.RoutineService.DeleteRoutine(Routine);
                App.MessagingCenter.SendChangeRoutineMessage();
            }
            await ClosePopup();
        }

        private IOrderedEnumerable<Habit> AssignIndexToHabits(IEnumerable<Habit> habits)
        {
            int i = 0;
            foreach (var habit in habits)
            {
                habit.Index = i++;
            }
            return habits.OrderBy((d) => d.Index);
        }

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync(true);
        }
        
        private async Task ShowHabitSetting()
        {
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new HabitSettingPage(new Habit()))).ConfigureAwait(false);
        }
        
        private void AddMiracleMorning()
        {
            DependencyService.Get<MessageBoxService>().ShowConfirm(
                $"미라클 모닝!",
                $"미라클 모닝을 실천하기 위한 습관 6개를 추가하시겠습니까?", null,
                () =>
                {
                    foreach (var miracleHabit in MiracleMorningList)
                    {
                        miracleHabit.Index = Habits.Count;
                        Habits.Add(miracleHabit);
                    }
                    RefreshHabits();
                });
        }

        public void RefreshHabits()
        {
            try
            {
                var orderedHabits = AssignIndexToHabits(Habits.OrderBy(t => t.Index));
                habits = Helper.ConvertIEnuemrableToObservableCollection(orderedHabits);
                OnPropertyChanged(nameof(HasNoHabit));
                OnPropertyChanged(nameof(HasHabit));
                OnPropertyChanged(nameof(Habits));
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

        #endregion
    }
}
