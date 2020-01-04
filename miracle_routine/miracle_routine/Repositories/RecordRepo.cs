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
        public IEnumerable<Record> RecordFromDB
        {
            get { return GetAllRecords() as IEnumerable<Record>; }
        }

        public Record GetRecord(int id)
        {
            return ItemDatabase.GetObject<Record>(id);
        }

        public IEnumerable<Record> GetAllRecords()
        {
            return ItemDatabase.GetObjects<Record>();
        }

        public int SaveRecord(Record record)
        {
            return ItemDatabase.SaveObject(record);
        }

        public int DeleteRecord(int id)
        {
            return ItemDatabase.DeleteObject<Record>(id);
        }

        public void DeleteAllRecords()
        {
            ItemDatabase.DeleteAllObjects<Record>();
        }
    }
}
