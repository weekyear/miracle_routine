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
    }
}
