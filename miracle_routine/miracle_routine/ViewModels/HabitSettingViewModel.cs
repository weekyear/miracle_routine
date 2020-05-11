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
            DeleteCommand = new Command(async () => await Delete());
            SaveCommand = new Command(async () => await Save());
            ShowRecommendedHabitsCommand = new Command(async () => await ShowRecommendedHabits());
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
        
        public string Description
        {
            get 
            {
                return Habit.Description; 
            }
            set
            {
                if (Habit.Description == value) return;
                Habit.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public List<HabitImages> HabitImageList
        {
            get
            {
                return HabitImages.HabitImageList;
            }
        }

        public static OrderableCollection<RecommendedHabit> recommendedHabitList;
        public OrderableCollection<RecommendedHabit> RecommendedHabitList
        {
            get
            {
                if (recommendedHabitList == null)
                {
                    var orderedHabits = AssignIndexToRecommendedHabits(App.RecommendedHabitRepo.RecommendedHabitsFromDB.OrderBy(r => r.Index));
                    recommendedHabitList = Helper.ConvertIEnuemrableToObservableCollection(orderedHabits);
                }
                return recommendedHabitList;
            }
        }

        private IOrderedEnumerable<RecommendedHabit> AssignIndexToRecommendedHabits(IEnumerable<RecommendedHabit> habits)
        {
            int i = 0;
            foreach (var habit in habits)
            {
                habit.Index = i++;
            }
            return habits.OrderBy((d) => d.Index);
        }

        public Command ShowRecommendedHabitsCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command CloseCommand { get; set; }

        #endregion

        #region METHOD
        private async Task Save()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                if (Seconds < 10)
                {
                    Seconds = Seconds * 10;
                }
                AddHabitList(Habit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                await ClosePopup();
                IsBusy = false;
            }
        }
        
        private async Task Delete()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                DependencyService.Get<MessageBoxService>().ShowConfirm(
                    $"습관 삭제",
                    $"습관을 삭제하시겠습니까?", null,
                    async () =>
                    {
                        RemoveHabitList(Habit);

                        await ClosePopup();
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync(true);
        }

        private async Task ShowRecommendedHabits()
        {
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RecommendedHabitListPage()));
            //await Application.Current.MainPage.DisplayAlert("", StringResources.RecommendedHabitSettingDescription, StringResources.OK);
        }
        public void RefreshRecommendedHabitList()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                var _habits = AssignIndexToRecommendedHabits(App.RecommendedHabitRepo.GetRecommendedHabits().OrderBy(r => r.Index));
                foreach (var _habit in _habits)
                {
                    App.RecommendedHabitRepo.SaveRecommendedHabit(_habit);
                }
                recommendedHabitList = Helper.ConvertIEnuemrableToObservableCollection(_habits);
                OnPropertyChanged(nameof(RecommendedHabitList));
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

        private void AddHabitList(Habit habit)
        {
            var Habits = RoutineSettingViewModel.habits;
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
        }

        private void RemoveHabitList(Habit habit)
        {
            var Habits = RoutineSettingViewModel.habits;
            if (habit.Index == -1)
            {
                return;
            }
            else
            {
                var oldHabit = Habits.FirstOrDefault(h => h.Index == habit.Index);
                int i = Habits.IndexOf(oldHabit);
                Habits.Remove(oldHabit);

                RoutineSettingViewModel.HabitsForDelete.Add(oldHabit);
            }
        }
        #endregion
    }
}
