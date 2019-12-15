using miracle_routine.Models;
using miracle_routine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace miracle_routine.Services
{
    public class HabitService : IHabitService
    {
        public HabitService(IHabitRepo repository)
        {
            Repository = repository;

            Habits = GetAllHabits();
        }

        public IHabitRepo Repository { get; }

        public List<Habit> Habits { get; private set; } = new List<Habit>();


        public Habit GetHabit(int id)
        {
            return Habits.FirstOrDefault(m => m.Id == id);
        }

        public List<Habit> GetAllHabits()
        {
            return Repository.HabitsFromDB;
        }

        public int SaveHabit(Habit habit)
        {
            var id = Repository.SaveHabit(habit);
            RefreshHabits();
            //DependencyService.Get<MyMessagingCenter>().SendChangeHabitsMessage();

            return id;
        }

        public void RefreshHabits()
        {
            Habits = GetAllHabits();
        }

        public int DeleteHabit(int id)
        {
            var dailyId = Repository.DeleteHabit(id);
            RefreshHabits();

            return dailyId;
        }

        public void DeleteAllHabits()
        {
            foreach (var dailyRecord in Habits)
            {
                DeleteHabit(dailyRecord.Id);
            }
        }

        public void SaveHabits(IEnumerable<Habit> habits)
        {
            foreach (var habit in habits)
            {
                Repository.SaveHabit(habit);
            }
            RefreshHabits();
        }
    }
}
