using miracle_routine.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace miracle_routine.ViewModels
{
    public class RecordViewModel : BaseViewModel
    {
        public RecordViewModel(INavigation navigation, Routine routine) : base(navigation)
        {
            SelectedRoutine = new Routine(routine);
            ConstructCommand();
            SubscribeMessage();

            Title = $"<{routine.Name}> 루틴 기록";

            UpdateWeekRecords();
        }
        private void ConstructCommand()
        {
            PreviousMonthCommand = new Command(() => PreviousMonth());
            NextMonthCommand = new Command(() => NextMonth());
        }

        private void SubscribeMessage()
        {
        }

        #region Property

        public Routine SelectedRoutine { get; set; }

        private List<Record> routines;
        public List<Record> Records
        {
            get
            {
                return App.RecordRepo.RecordFromDB.Where(r => r.RoutineId == SelectedRoutine.Id).OrderBy(r => r.RecordTime).ToList();
            }
            set
            {
                if (routines == value) return;
                routines = value;
                OnPropertyChanged(nameof(Records));
            }
        }

        public DateTime SelectedMonth { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

        public List<WeekRecord> WeekRecords { get; set; } = new List<WeekRecord>();

        private DateTime FirstDateOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
        public int AllDayOfSelectedMonthUntilToday
        {
            get 
            { 
                if (FirstDateOfThisMonth.Ticks > SelectedMonth.Ticks)
                {
                    return DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month);
                }
                else if (FirstDateOfThisMonth.Ticks == SelectedMonth.Ticks)
                {
                    return DateTime.Now.Day;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int RoutineNumOfThisMonth { get; set; }
        public int SuccessRoutineNumOfThisMonth { get; set; }
        public double RoutineRate
        {
            get
            {
                if (AllDayOfSelectedMonthUntilToday == 0) return -1;
                return (double)RoutineNumOfThisMonth / AllDayOfSelectedMonthUntilToday;
            }
        }
        
        public double RoutineSuccessRate
        {
            get
            {
                if (RoutineNumOfThisMonth == 0) return -1;
                return (double)SuccessRoutineNumOfThisMonth / RoutineNumOfThisMonth;
            }
        }

        public Command PreviousMonthCommand { get; set; }
        public Command NextMonthCommand { get; set; }

        #endregion

        #region Method

        private void UpdateWeekRecords()
        {
            var weekRecords = new List<WeekRecord>();
            var startDateOfSelectedMonth = SelectedMonth;
            var startDateOfWeek = new DateTime(startDateOfSelectedMonth.Year, startDateOfSelectedMonth.Month, 1, 0, 0, 0);

            while (startDateOfWeek.Date.DayOfWeek != DayOfWeek.Sunday) startDateOfWeek = startDateOfWeek.AddDays(-1);

            var startDateOfMonth = startDateOfWeek;

            RoutineNumOfThisMonth = 0;
            SuccessRoutineNumOfThisMonth = 0;

            while (startDateOfMonth.Ticks <= startDateOfSelectedMonth.Ticks)
            {
                var RecordsOfWeek = Records.FindAll(r => startDateOfWeek.Ticks <= r.RecordTime.Ticks && r.RecordTime.Ticks < startDateOfWeek.AddDays(7).Ticks);

                var weekRecord = new WeekRecord()
                {
                    SelectedMonth = SelectedMonth.Month,
                    StartDateOfWeek = startDateOfWeek,
                    DayRecords = RecordsOfWeek.OrderBy(r => r.RecordTime).ToList()
                };

                weekRecords.Add(weekRecord);

                startDateOfWeek = startDateOfWeek.AddDays(7);

                startDateOfMonth = new DateTime(startDateOfWeek.Year, startDateOfWeek.Month, 1, 0, 0, 0);
                var recordsOfSelectedMonth = RecordsOfWeek.Where(r => r.RecordTime.Month == SelectedMonth.Month).ToList();
                RoutineNumOfThisMonth += recordsOfSelectedMonth.Count();
                SuccessRoutineNumOfThisMonth += recordsOfSelectedMonth.Where(r => r.IsSuccess == true).Count();
            }

            WeekRecords.Clear();

            WeekRecords = weekRecords;
            OnPropertyChanged(nameof(WeekRecords));
            OnPropertyChanged(nameof(AllDayOfSelectedMonthUntilToday));
            OnPropertyChanged(nameof(RoutineNumOfThisMonth));
            OnPropertyChanged(nameof(SuccessRoutineNumOfThisMonth));
        }


        private void PreviousMonth()
        {
            SelectedMonth = SelectedMonth.AddMonths(-1);

            UpdateWeekRecords();
        }

        private void NextMonth()
        {
            SelectedMonth = SelectedMonth.AddMonths(1);

            UpdateWeekRecords();
        }
        public class WeekRecord
        {
            public int SelectedMonth { get; set; }
            public DateTime StartDateOfWeek { get; set; }
            public List<Record> DayRecords { get; set; } = new List<Record>();

            public string SunDate
            {
                get { return $"{StartDateOfWeek.Day}"; }
            }

            public bool IsSelectedMonthSun
            {
                get { return StartDateOfWeek.Month == SelectedMonth; }
            }

            public Record SunRecord
            {
                get { return DayRecords.FirstOrDefault(d => d.RecordTime.DayOfWeek == DayOfWeek.Sunday); }
            }
            public bool IsNotNullSun
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Sunday); }
            }
            public bool IsSuccessSun
            {
                get { return SunRecord != null ? SunRecord.IsSuccess : false; }
            }

            public string MonDate
            {
                get { return $"{StartDateOfWeek.AddDays(1).Day}"; }
            }
            public bool IsSelectedMonthMon
            {
                get { return StartDateOfWeek.AddDays(1).Month == SelectedMonth; }
            }
            public Record MonRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Monday); }
            }
            public bool IsNotNullMon
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Monday); }
            }
            public bool IsSuccessMon
            {
                get { return MonRecord != null ? MonRecord.IsSuccess : false; }
            }

            public string TueDate
            {
                get { return $"{StartDateOfWeek.AddDays(2).Day}"; }
            }
            public bool IsSelectedMonthTue
            {
                get { return StartDateOfWeek.AddDays(2).Month == SelectedMonth; }
            }
            public Record TueRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Tuesday); }
            }
            public bool IsNotNullTue
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Tuesday); }
            }
            public bool IsSuccessTue
            {
                get { return TueRecord != null ? TueRecord.IsSuccess : false; }
            }

            public string WedDate
            {
                get { return $"{StartDateOfWeek.AddDays(3).Day}"; }
            }
            public bool IsSelectedMonthWed
            {
                get { return StartDateOfWeek.AddDays(3).Month == SelectedMonth; }
            }
            public Record WedRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Wednesday); }
            }
            public bool IsNotNullWed
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Wednesday); }
            }
            public bool IsSuccessWed
            {
                get { return WedRecord != null ? WedRecord.IsSuccess : false; }
            }

            public string ThuDate
            {
                get { return $"{StartDateOfWeek.AddDays(4).Day}"; }
            }
            public bool IsSelectedMonthThu
            {
                get { return StartDateOfWeek.AddDays(4).Month == SelectedMonth; }
            }
            public Record ThuRecord
            {
                get 
                { 
                    return DayRecords.FirstOrDefault(d => d.RecordTime.DayOfWeek == DayOfWeek.Thursday); 
                }
            }
            public bool IsNotNullThu
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Thursday); }
            }
            public bool IsSuccessThu
            {
                get { return ThuRecord != null ? ThuRecord.IsSuccess : false; }
            }

            public string FriDate
            {
                get { return $"{StartDateOfWeek.AddDays(5).Day}"; }
            }
            public bool IsSelectedMonthFri
            {
                get { return StartDateOfWeek.AddDays(5).Month == SelectedMonth; }
            }
            public Record FriRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Friday); }
            }
            public bool IsNotNullFri
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Friday); }
            }
            public bool IsSuccessFri
            {
                get { return FriRecord != null ? FriRecord.IsSuccess : false; }
            }

            public string SatDate
            {
                get { return $"{StartDateOfWeek.AddDays(6).Day}"; }
            }
            public bool IsSelectedMonthSat
            {
                get { return StartDateOfWeek.AddDays(6).Month == SelectedMonth; }
            }
            public Record SatRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Saturday); }
            }
            public bool IsNotNullSat
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Saturday); }
            }
            public bool IsSuccessSat
            {
                get 
                { 
                    return SatRecord != null ? SatRecord.IsSuccess : false; 
                }
            }
        }
        #endregion
    }
}
