using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public interface IRecordRepo
    {
        // Record
        IEnumerable<RoutineRecord> RoutineRecordFromDB { get; }

        RoutineRecord GetRoutineRecord(int id);
        IEnumerable<RoutineRecord> GetAllRoutineRecords();
        int SaveRoutineRecord(RoutineRecord routineRecord);
        int DeleteRoutineRecord(int id);
        void DeleteAllRoutineRecords();

        //HabitRecord
        IEnumerable<HabitRecord> HabitRecordFromDB { get; }

        HabitRecord GetHabitRecord(int id);
        IEnumerable<HabitRecord> GetAllHabitRecords();
        int SaveHabitRecord(HabitRecord habitRecord);
        int DeleteHabitRecord(int id);
        void DeleteAllHabitRecords();
    }
}
