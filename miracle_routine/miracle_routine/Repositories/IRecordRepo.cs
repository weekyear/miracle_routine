using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public interface IRecordRepo
    {
        // Record
        IEnumerable<Record> RecordFromDB { get; }

        Record GetRecord(int id);
        IEnumerable<Record> GetAllRecords();
        int SaveRecord(Record record);
        int DeleteRecord(int id);
        void DeleteAllRecords();
    }
}
