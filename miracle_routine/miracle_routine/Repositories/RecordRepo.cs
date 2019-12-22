using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public class RecordRepo : IRecordRepo
    {
        public ItemDatabaseGeneric ItemDatabase { get; }
        public RecordRepo(ItemDatabaseGeneric itemDatabase)
        {
            ItemDatabase = itemDatabase;
        }
        public IEnumerable<RoutineRecord> RoutineRecordFromDB
        {
            get { return GetAllRoutineRecords() as IEnumerable<RoutineRecord>; }
        }

        public IEnumerable<HabitRecord> HabitRecordFromDB
        {
            get { return GetAllHabitRecords() as IEnumerable<HabitRecord>; }
        }

        public RoutineRecord GetRoutineRecord(int id)
        {
            return ItemDatabase.GetObject<RoutineRecord>(id);
        }

        public IEnumerable<RoutineRecord> GetAllRoutineRecords()
        {
            return ItemDatabase.GetObjects<RoutineRecord>();
        }

        public int SaveRoutineRecord(RoutineRecord routineRecord)
        {
            return ItemDatabase.SaveObject(routineRecord);
        }

        public int DeleteRoutineRecord(int id)
        {
            return ItemDatabase.DeleteObject<RoutineRecord>(id);
        }

        public void DeleteAllRoutineRecords()
        {
            ItemDatabase.DeleteAllObjects<RoutineRecord>();
        }

        public HabitRecord GetHabitRecord(int id)
        {
            return ItemDatabase.GetObject<HabitRecord>(id);
        }

        public IEnumerable<HabitRecord> GetAllHabitRecords()
        {
            return ItemDatabase.GetObjects<HabitRecord>();
        }

        public int SaveHabitRecord(HabitRecord habitRecord)
        {
            return ItemDatabase.SaveObject(habitRecord);
        }

        public int DeleteHabitRecord(int id)
        {
            return ItemDatabase.DeleteObject<HabitRecord>(id);
        }

        public void DeleteAllHabitRecords()
        {
            ItemDatabase.DeleteAllObjects<HabitRecord>();
        }
    }
}
