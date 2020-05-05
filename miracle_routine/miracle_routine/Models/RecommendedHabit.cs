using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace miracle_routine.Models
{
    [Table(nameof(RecommendedHabit))]
    public class RecommendedHabit : IObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } = "ic_miracle_routine_mini.png";
        public string Description { get; set; }
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
        public TimeSpan TotalTime
        {
            get { return new TimeSpan(0, Minutes, Seconds); }
        }

        public RecommendedHabit() { }

        public RecommendedHabit(RecommendedHabit recommendedHabit)
        {
            Id = recommendedHabit.Id;
            Name = recommendedHabit.Name;
            Image = recommendedHabit.Image;
            Description = recommendedHabit.Description;
            Index = recommendedHabit.Index;
            Minutes = recommendedHabit.Minutes;
            Seconds = recommendedHabit.Seconds;
        }
    }
}
