using SQLite;
using System;
using System.ComponentModel;

namespace miracle_routine.Models
{
    [Table(nameof(Routine))]
    public class Routine : IObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}