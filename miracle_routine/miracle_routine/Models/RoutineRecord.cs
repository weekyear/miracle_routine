using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace miracle_routine.Models
{
    [Table("RoutineRecord")]
    public class RoutineRecord : INotifyPropertyChanged, IObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }

        public int RoutineId { get; set; }

        public DateTime RecordTime { get; set; }

        public string RoutineName { get; set; }
        //public TimeSpan ElapsedTime { get; set; }
        //public TimeSpan TotalTime { get; set; }
        //public TimeSpan TimeRemaining
        //{
        //    get { return TotalTime.Subtract(ElapsedTime); }
        //}

        public bool IsSuccess { get; set; } = true;

        //public List<HabitRecord> HabitList
        //{
        //    get { return App.RecordRepo.HabitRecordFromDB.Where(h => h.RoutineRecordId == Id).ToList(); }
        //}

        public RoutineRecord() { }

        public RoutineRecord(Routine routine, bool isSuccess)
        {
            RoutineId = routine.Id;
            RoutineName = routine.Name;
            //ElapsedTime = recordTime;
            //TotalTime = routine.TotalTime;
            IsSuccess = isSuccess;
            RecordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }
    }
}
