using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public string Image { get; set; } = "ic_blank_habit.png";
        public string Description { get; set; }
        public int RoutineId { get; set; }
        public int Index { get; set; } = -1;

        public int Minutes { get; set; } = 1;
        public int Seconds { get; set; } = 3;
        public bool IsNotEmptyDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Ignore]
        public TimeSpan Time
        {
            get { return new TimeSpan(0, Minutes, Seconds); }
        }

        public Habit() { }

        public Habit(Habit habit)
        {
            Id = habit.Id;
            Name = habit.Name;
            Image = habit.Image;
            Description = habit.Description;
            RoutineId = habit.RoutineId;
            Index = habit.Index;
            Minutes = habit.Minutes;
            Seconds = habit.Seconds;
        }
    }
}
