using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace miracle_routine.Models
{
    [Table(nameof(Routine))]
    public class Routine : IObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int DaysId { get; set; }
        public bool IsLocation { get; set; }

        public TimeSpan StartTime { get; set; }

        [Ignore]
        public List<Habit> HabitList { get; set; }

        [Ignore]
        public TimeSpan TotalTime
        {
            get
            {
                var totalTime = new TimeSpan(0);
                foreach (var habit in HabitList)
                {
                    totalTime.Add(habit.Time);
                }
                return totalTime;
            }
        }
        public Routine() { }

        public Routine(Routine routine)
        {
            Id = routine.Id;
            Name = routine.Name;
            DaysId = routine.DaysId;
            IsLocation = routine.IsLocation;
            StartTime = routine.StartTime;
        }
    }
}