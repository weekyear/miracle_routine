using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public class HabitRepo : IHabitRepo
    {
        public ItemDatabaseGeneric ItemDatabase { get; }
        public HabitRepo(ItemDatabaseGeneric itemDatabase)
        {
            //if (Device.RuntimePlatform == "Test") return;
            ItemDatabase = itemDatabase;
        }

        public List<Habit> HabitsFromDB
        {
            get { return GetHabits() as List<Habit>; }
        }

        public Habit GetHabit(int id)
        {
            return ItemDatabase.GetObject<Habit>(id);
        }

        public IEnumerable<Habit> GetHabits()
        {
            return ItemDatabase.GetObjects<Habit>();
        }

        public int SaveHabit(Habit dailyRecord)
        {
            return ItemDatabase.SaveObject(dailyRecord);
        }

        public int DeleteHabit(int id)
        {
            return ItemDatabase.DeleteObject<Habit>(id);
        }

        public void DeleteAllHabits()
        {
            ItemDatabase.DeleteAllObjects<Habit>();
        }
    }
}
