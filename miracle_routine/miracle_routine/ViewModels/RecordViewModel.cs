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

        public int RoutineNumOfThisMonth { get; set; }
        public int SuccessRoutineNumOfThisMonth { get; set; }
        
        public double RoutineSuccessRateOfThisMonth
        {
            get
            {
                if (RoutineNumOfThisMonth == 0) return -1;
                return (double)SuccessRoutineNumOfThisMonth / RoutineNumOfThisMonth;
            }
        }

        public int RoutineNumOfThreeMonth { get; set; }
        public int SuccessRoutineNumOfThreeMonth { get; set; }

        public double RoutineSuccessRateOfThreeMonth
        {
            get
            {
                if (RoutineNumOfThreeMonth == 0) return -1;
                return (double)SuccessRoutineNumOfThreeMonth / RoutineNumOfThreeMonth;
            }
        }

        public int RoutineNumOfAll { get; set; }
        public int SuccessRoutineNumOfAll { get; set; }

        public double RoutineSuccessRateOfAll
        {
            get
            {
                if (RoutineNumOfAll == 0) return -1;
                return (double)SuccessRoutineNumOfAll / RoutineNumOfAll;
            }
        }

        public Command PreviousMonthCommand { get; set; }
        public Command NextMonthCommand { get; set; }

        private bool IsInThisMonth(DateTime recordDate)
        {
            var firstDady = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1, 0, 0, 0);
            var lastDay = new DateTime(SelectedMonth.Year, SelectedMonth.Month, DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month), 23, 59, 59);
            return recordDate <= lastDay && recordDate >= firstDady;
        }

        private bool IsInThreeMonth(DateTime recordDate)
        {
            var firstDady = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1, 0, 0, 0).AddMonths(-2);
            var lastDay = new DateTime(SelectedMonth.Year, SelectedMonth.Month, DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month), 23, 59, 59);
            return recordDate <= lastDay && recordDate >= firstDady;
        }

        #endregion

        #region Method

        private void UpdateWeekRecords()
        {
            var weekRecords = new List<WeekRecord>();
            var startDateOfSelectedMonth = SelectedMonth;
            var startDateOfWeek = new DateTime(startDateOfSelectedMonth.Year, startDateOfSelectedMonth.Month, 1, 0, 0, 0);

            while (startDateOfWeek.Date.DayOfWeek != DayOfWeek.Sunday) startDateOfWeek = startDateOfWeek.AddDays(-1);

            var startDateOfMonth = startDateOfWeek;

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
            }
            ResetSuccessRate();

            WeekRecords.Clear();

            WeekRecords = weekRecords;
            OnPropertyChanged(nameof(WeekRecords));
            OnPropertyChanged(nameof(RoutineNumOfThisMonth));
            OnPropertyChanged(nameof(SuccessRoutineNumOfThisMonth));
        }

        private void ResetSuccessRate()
        {
            RoutineNumOfThisMonth = 0;
            SuccessRoutineNumOfThisMonth = 0;
            RoutineNumOfThreeMonth = 0;
            SuccessRoutineNumOfThreeMonth = 0;
            RoutineNumOfAll = 0;
            SuccessRoutineNumOfAll = 0;

            var recordsOfSelectedMonth = Records.Where(r => IsInThisMonth(r.RecordTime));

            foreach (var record in recordsOfSelectedMonth)
            {
                // SelectedRoutine.Days.SelectedDateList.Contains((int)record.RecordTime.DayOfWeek) && 
                if (record.IsStartByNotify)
                {
                    RoutineNumOfThisMonth++;
                }
            }
            foreach (var record in recordsOfSelectedMonth.Where(r => r.IsSuccess == true))
            {
                if (record.IsStartByNotify)
                {
                    SuccessRoutineNumOfThisMonth++;
                }
            }
            var recordsOfThreeSelectedMonth = Records.Where(r => IsInThreeMonth(r.RecordTime));
            foreach (var record in recordsOfThreeSelectedMonth)
            {
                if (record.IsStartByNotify)
                {
                    RoutineNumOfThreeMonth++;
                }
            }
            foreach (var record in recordsOfThreeSelectedMonth.Where(r => r.IsSuccess == true))
            {
                if (record.IsStartByNotify)
                {
                    SuccessRoutineNumOfThreeMonth++;
                }
            }
            foreach (var record in Records)
            {
                if (record.IsStartByNotify)
                {
                    RoutineNumOfAll++;
                }
            }
            foreach (var record in Records.Where(r => r.IsSuccess == true))
            {
                if (record.IsStartByNotify)
                {
                    SuccessRoutineNumOfAll++;
                }
            }
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
            public string MonDate
            {
                get { return $"{StartDateOfWeek.AddDays(1).Day}"; }
            }
            public string TueDate
            {
                get { return $"{StartDateOfWeek.AddDays(2).Day}"; }
            }
            public string WedDate
            {
                get { return $"{StartDateOfWeek.AddDays(3).Day}"; }
            }
            public string ThuDate
            {
                get { return $"{StartDateOfWeek.AddDays(4).Day}"; }
            }
            public string FriDate
            {
                get { return $"{StartDateOfWeek.AddDays(5).Day}"; }
            }
            public string SatDate
            {
                get { return $"{StartDateOfWeek.AddDays(6).Day}"; }
            }

            public bool IsSelectedMonthSun
            {
                get { return StartDateOfWeek.Month == SelectedMonth; }
            }
            public bool IsSelectedMonthMon
            {
                get { return StartDateOfWeek.AddDays(1).Month == SelectedMonth; }
            }
            public bool IsSelectedMonthTue
            {
                get { return StartDateOfWeek.AddDays(2).Month == SelectedMonth; }
            }
            public bool IsSelectedMonthWed
            {
                get { return StartDateOfWeek.AddDays(3).Month == SelectedMonth; }
            }
            public bool IsSelectedMonthThu
            {
                get { return StartDateOfWeek.AddDays(4).Month == SelectedMonth; }
            }
            public bool IsSelectedMonthFri
            {
                get { return StartDateOfWeek.AddDays(5).Month == SelectedMonth; }
            }
            public bool IsSelectedMonthSat
            {
                get { return StartDateOfWeek.AddDays(6).Month == SelectedMonth; }
            }


            public Record SunRecord
            {
                get { return DayRecords.FirstOrDefault(d => d.RecordTime.DayOfWeek == DayOfWeek.Sunday); }
            }
            public Record MonRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Monday); }
            }
            public Record TueRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Tuesday); }
            }
            public Record WedRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Wednesday); }
            }
            public Record ThuRecord
            {
                get { return DayRecords.FirstOrDefault(d => d.RecordTime.DayOfWeek == DayOfWeek.Thursday); }
            }
            public Record FriRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Friday); }
            }
            public Record SatRecord
            {
                get { return DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Saturday); }
            }

            public bool IsNotNullSun
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Sunday); }
            }
            public bool IsNotNullMon
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Monday); }
            }
            public bool IsNotNullTue
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Tuesday); }
            }
            public bool IsNotNullWed
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Wednesday); }
            }
            public bool IsNotNullThu
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Thursday); }
            }
            public bool IsNotNullFri
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Friday); }
            }
            public bool IsNotNullSat
            {
                get { return DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Saturday); }
            }


            public bool IsSuccessSun
            {
                get { return SunRecord != null ? SunRecord.IsSuccess : false; }
            }                      
            public bool IsSuccessMon
            {
                get { return MonRecord != null ? MonRecord.IsSuccess : false; }
            }            
            public bool IsSuccessTue
            {
                get { return TueRecord != null ? TueRecord.IsSuccess : false; }
            }          
            public bool IsSuccessWed
            {
                get { return WedRecord != null ? WedRecord.IsSuccess : false; }
            }            
            public bool IsSuccessThu
            {
                get { return ThuRecord != null ? ThuRecord.IsSuccess : false; }
            }           
            public bool IsSuccessFri
            {
                get { return FriRecord != null ? FriRecord.IsSuccess : false; }
            }            
            public bool IsSuccessSat
            {
                get { return SatRecord != null ? SatRecord.IsSuccess : false; }
            }

            public Color ColorOfSun
            {
                get
                {
                    if (StartDateOfWeek.Date <= DateTime.Now.Date)
                    {
                        if (DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Sunday) && DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Sunday).IsStartByNotify)
                        {
                            return (Color)App.Current.Resources["DynamicPrimaryLightColor"];
                        }
                        else
                        {
                            return (Color)App.Current.Resources["DynamicSecondaryBackgroundColor"];
                        }
                    }
                    else
                    {
                        return (Color)App.Current.Resources["DynamicBackgroundColor"];
                    }
                }
            }
            public Color ColorOfMon
            {
                get
                {
                    if (StartDateOfWeek.AddDays(1).Date <= DateTime.Now.Date)
                    {

                        if (DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Monday) && DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Monday).IsStartByNotify)
                        {
                            return (Color)App.Current.Resources["DynamicPrimaryLightColor"];
                        }
                        else
                        {
                            return (Color)App.Current.Resources["DynamicSecondaryBackgroundColor"];
                        }
                    }
                    else
                    {
                        return (Color)App.Current.Resources["DynamicBackgroundColor"];
                    }
                }
            }
            public Color ColorOfTue
            {
                get
                {
                    if (StartDateOfWeek.AddDays(2).Date <= DateTime.Now.Date)
                    {

                        if (DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Tuesday) && DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Tuesday).IsStartByNotify)
                        {
                            return (Color)App.Current.Resources["DynamicPrimaryLightColor"];
                        }
                        else
                        {
                            return (Color)App.Current.Resources["DynamicSecondaryBackgroundColor"];
                        }
                    }
                    else
                    {
                        return (Color)App.Current.Resources["DynamicBackgroundColor"];
                    }
                }
            }
            public Color ColorOfWed
            {
                get
                {
                    if (StartDateOfWeek.AddDays(3).Date <= DateTime.Now.Date)
                    {
                        if (DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Wednesday) && DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Wednesday).IsStartByNotify)
                        {
                            return (Color)App.Current.Resources["DynamicPrimaryLightColor"];
                        }
                        else
                        {
                            return (Color)App.Current.Resources["DynamicSecondaryBackgroundColor"];
                        }
                    }
                    else
                    {
                        return (Color)App.Current.Resources["DynamicBackgroundColor"];
                    }
                }
            }
            public Color ColorOfThu
            {
                get
                {
                    if (StartDateOfWeek.AddDays(4).Date <= DateTime.Now.Date)
                    {
                        if (DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Thursday) && DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Thursday).IsStartByNotify)
                        {
                            return (Color)App.Current.Resources["DynamicPrimaryLightColor"];
                        }
                        else
                        {
                            return (Color)App.Current.Resources["DynamicSecondaryBackgroundColor"];
                        }
                    }
                    else
                    {
                        return (Color)App.Current.Resources["DynamicBackgroundColor"];
                    }
                }
            }
            public Color ColorOfFri
            {
                get
                {
                    if (StartDateOfWeek.AddDays(5).Date <= DateTime.Now.Date)
                    {
                        if (DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Friday) && DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Friday).IsStartByNotify)
                        {
                            return (Color)App.Current.Resources["DynamicPrimaryLightColor"];
                        }
                        else
                        {
                            return (Color)App.Current.Resources["DynamicSecondaryBackgroundColor"];
                        }
                    }
                    else
                    {
                        return (Color)App.Current.Resources["DynamicBackgroundColor"];
                    }
                }
            }
            public Color ColorOfSat
            {
                get
                {
                    if (StartDateOfWeek.AddDays(6).Date <= DateTime.Now.Date)
                    {
                        if (DayRecords.Exists(d => d.RecordTime.DayOfWeek == DayOfWeek.Saturday) && DayRecords.Find(d => d.RecordTime.DayOfWeek == DayOfWeek.Saturday).IsStartByNotify)
                        {
                            return (Color)App.Current.Resources["DynamicPrimaryLightColor"];
                        }
                        else
                        {
                            return (Color)App.Current.Resources["DynamicSecondaryBackgroundColor"];
                        }
                    }
                    else
                    {
                        return (Color)App.Current.Resources["DynamicBackgroundColor"];
                    }
                }
            }
        }
        #endregion
    }
}
