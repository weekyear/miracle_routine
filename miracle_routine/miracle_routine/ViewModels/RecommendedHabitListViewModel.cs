using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Views;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RecommendedHabitListViewModel : BaseViewModel
    {
        public RecommendedHabitListViewModel(INavigation navigation) : base(navigation)
        {
            ConstructCommand();
        }

        private void ConstructCommand()
        {
            CloseCommand = new Command(async () => await ClosePopup());
            ShowRecommendedHabitSettingCommand = new Command(async () => await NavigateRecommendedHabitSetting());
        }

        #region PROPERTY
        private OrderableCollection<RecommendedHabit> recommendedHabitList;
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

        public Command CloseCommand { get; set; }
        public Command ShowRecommendedHabitSettingCommand { get; set; }

        #endregion

        #region METHOD

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync(true);
        }

        private async Task NavigateRecommendedHabitSetting()
        {
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RecommendedHabitSettingPage(new RecommendedHabit())));
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
        #endregion
    }
}
