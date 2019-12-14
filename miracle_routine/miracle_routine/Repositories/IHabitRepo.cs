using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public interface IHabitRepo
    {
        // getters
        List<Habit> HabitsFromDB { get; }

        // Habit
        Habit GetHabit(int id);
        IEnumerable<Habit> GetHabits();

        int SaveHabit(Habit habit);

        int DeleteHabit(int id);
        void DeleteAllHabits();
    }
}
