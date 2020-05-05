using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public interface IRecommendedHabitRepo
    {
        // getters
        List<RecommendedHabit> RecommendedHabitsFromDB { get; }

        // Habit
        RecommendedHabit GetRecommendedHabit(int id);
        IEnumerable<RecommendedHabit> GetRecommendedHabits();

        int SaveRecommendedHabit(RecommendedHabit habit);

        int DeleteRecommendedHabit(int id);
        void DeleteAllRecommendedHabits();
    }
}
