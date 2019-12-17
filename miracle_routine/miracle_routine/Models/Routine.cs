using miracle_routine.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
        
        [OneToOne]
        public DaysOfWeek Days { get; set; } = new DaysOfWeek();

        public TimeSpan StartTime { get; set; } = new TimeSpan(7, 0, 0);

        [Ignore]
        public int Height
        {
            get { return (HabitList.Count * 40) + (HabitList.Count * 10); }
        }

        [Ignore]
        public TimeSpan ElapsedTime
        {
            get; set;
        }

        public List<Habit> HabitList
        {
            get { return App.HabitService.Habits.Where((habit) => habit.RoutineId == Id).ToList(); }
        }
        public DateTime NextAlarmTime
        {
            get { return CalculateNextAlarmTime.NextAlarmTime(this); }
        }

        public TimeSpan TotalTime
        {
            get
            {
                var totalTime = new TimeSpan(0);
                foreach (var habit in HabitList)
                {
                    totalTime = totalTime.Add(habit.Time);
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
            Days = routine.Days;
            IsLocation = routine.IsLocation;
            StartTime = routine.StartTime;
            ElapsedTime = routine.ElapsedTime;
        }
    }
}