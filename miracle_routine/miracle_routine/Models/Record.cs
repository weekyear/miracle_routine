using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public string RoutineName { get; set; }
        public TimeSpan RecordTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public DateTime Date { get; set; }

        [Ignore]
        public bool IsSuccess
        {
            get
            {
                if (RecordTime.TotalSeconds < TotalTime.TotalSeconds)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Record() { }

        public Record(Routine routine, TimeSpan recordTime)
        {
            RoutineId = routine.Id;
            RoutineName = routine.Name;
            RecordTime = recordTime;
            TotalTime = routine.TotalTime;
            Date = DateTime.Now.Date;
        }
    }
}
