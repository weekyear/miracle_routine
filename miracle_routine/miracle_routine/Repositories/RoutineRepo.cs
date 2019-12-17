using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public class RoutineRepo : IRoutineRepo
    {
        public ItemDatabaseGeneric ItemDatabase { get; }
        public RoutineRepo(ItemDatabaseGeneric itemDatabase)
        {
            //if (Device.RuntimePlatform == "Test") return;
            ItemDatabase = itemDatabase;
        }

        public List<Routine> RoutinesFromDB
        {
            get { return GetRoutines() as List<Routine>; }
        }
        public IEnumerable<DaysOfWeek> DaysOfWeeksFromDB
        {
            get { return GetAllDaysOfWeeks() as IEnumerable<DaysOfWeek>; }
        }

        public Routine GetRoutine(int id)
        {
            return ItemDatabase.GetObject<Routine>(id);
        }

        public IEnumerable<Routine> GetRoutines()
        {
            return ItemDatabase.GetObjects<Routine>();
        }

        public int SaveRoutine(Routine dailyRecord)
        {
            return ItemDatabase.SaveObject(dailyRecord);
        }

        public int DeleteRoutine(int id)
        {
            return ItemDatabase.DeleteObject<Routine>(id);
        }

        public void DeleteAllRoutines()
        {
            ItemDatabase.DeleteAllObjects<Routine>();
        }

        // DaysOfWeek

        public DaysOfWeek GetDaysOfWeek(int id)
        {
            return ItemDatabase.GetObject<DaysOfWeek>(id);
        }

        public IEnumerable<DaysOfWeek> GetAllDaysOfWeeks()
        {
            return ItemDatabase.GetObjects<DaysOfWeek>();
        }

        public int SaveDaysOfWeek(DaysOfWeek daysOfWeek)
        {
            return ItemDatabase.SaveObject(daysOfWeek);
        }

        public int DeleteDaysOfWeek(int id)
        {
            return ItemDatabase.DeleteObject<DaysOfWeek>(id);
        }

        public void DeleteAllDaysOfWeeks()
        {
            ItemDatabase.DeleteAllObjects<DaysOfWeek>();
        }
    }
}
