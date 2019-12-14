using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace miracle_routine.Models
{
    [Table(nameof(Habit))]
    public class Habit : IObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public int RoutineId { get; set; }
        public int Index { get; set; }

        public Habit() { }

        public Habit(Habit habit)
        {
            Id = habit.Id;
            Name = habit.Name;
            Image = habit.Image;
            Description = habit.Description;
            Time = habit.Time;
            RoutineId = habit.RoutineId;
            Index = habit.Index;
        }
    }
}
