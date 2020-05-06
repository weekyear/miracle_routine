using miracle_routine.Helpers;
using miracle_routine.Models;
using System;
using System.Collections.Generic;
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
            ShowRecommendedHabitSettingCommand = new Command(async () => await ClosePopup());
        }

        #region PROPERTY
        public static OrderableCollection<RecommendedHabit> recommendedHabitList;
        public OrderableCollection<RecommendedHabit> RecommendedHabitList
        {
            get
            {
                if (recommendedHabitList == null)
                {
                    var orderedHabits = AssignIndexToHabits(App.RecommendedHabitRepo.RecommendedHabitsFromDB.OrderBy(r => r.Index));
                    recommendedHabitList = Helper.ConvertIEnuemrableToObservableCollection(orderedHabits);
                }
                return recommendedHabitList;
            }
        }

        private IOrderedEnumerable<RecommendedHabit> AssignIndexToHabits(IEnumerable<RecommendedHabit> habits)
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
        #endregion
    }
}
