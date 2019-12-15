using miracle_routine.Models;
using miracle_routine.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Services
{
    public interface IHabitService
    {
        IHabitRepo Repository { get; }
        List<Habit> Habits { get; }
        Habit GetHabit(int id);
        int DeleteHabit(int id);
        int SaveHabit(Habit habit);
        void SaveHabits(IEnumerable<Habit> habits);
        void DeleteAllHabits();
        List<Habit> GetAllHabits();
        void RefreshHabits();
    }
}
