using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace miracle_routine.Models
{
    [Table("Record")]
    public class Record : INotifyPropertyChanged, IObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }

        public int RoutineId { get; set; }

        public DateTime RecordTime { get; set; }

        public bool IsSuccess { get; set; } = true;

        public Record() { }

        public Record(Routine routine, bool isSuccess)
        {
            RoutineId = routine.Id;
            IsSuccess = isSuccess;
            RecordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }
    }
}
