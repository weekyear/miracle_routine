using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace miracle_routine.Models
{
    public class HabitRecord : INotifyPropertyChanged, IObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }

        public int HabitId { get; set; }
        public int RoutineRecordId { get; set; }

        public string HabitName { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan TotalTime { get; set; }

        public TimeSpan OverTime
        {
            get { return TotalTime.Subtract(ElapsedTime); }
        }
        public DateTime Date { get; set; }


        public HabitRecord() { }

        public HabitRecord(Routine routine, TimeSpan recordTime)
        {
            HabitId = routine.Id;
            HabitName = routine.Name;
            ElapsedTime = recordTime;
            TotalTime = routine.TotalTime;
            Date = DateTime.Now.Date;
        }
    }
}
