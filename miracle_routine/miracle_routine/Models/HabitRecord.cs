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

        public TimeSpan TimeRemaining
        {
            get { return TotalTime.Subtract(ElapsedTime); }
        }

        public DateTime RecordTime { get; set; }


        public HabitRecord() { }

        public HabitRecord(Habit habit, TimeSpan elapsedTime)
        {
            HabitId = habit.Id;
            HabitName = habit.Name;
            TotalTime = habit.TotalTime;
            ElapsedTime = elapsedTime;
            RecordTime = DateTime.Now;
        }
    }
}
