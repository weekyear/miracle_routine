using Microcharts;
using miracle_routine.Helpers;
using miracle_routine.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace miracle_routine.ViewModels
{
    public class RoutineRecordViewModel : BaseViewModel
    {
        private readonly int NumOfChartData = 5;
        private readonly int LabelFontSize = 10;
        private readonly int AnimationSec = 1;
        public RoutineRecordViewModel(INavigation navigation, Routine routine) : base(navigation)
        {
            SelectedRoutine = new Routine(routine);
            ConstructCommand();
            SubscribeMessage();

            Title = $"<{routine.Name}> 루틴 기록";

            InitChart();
        }
        private void ConstructCommand()
        {
            NextRecordCommand = new Command(() => NextRecord());
            PreviousRecordCommand = new Command(() => PreviousRecord());
        }

        private void SubscribeMessage()
        {
        }

        private void InitChart()
        {
            //if (RecordTotalCount == 0)
            //{
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 15, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 01), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 13, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 02), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 11, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 03), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 16, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 04), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 22, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 05), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 18, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 06), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 12, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 07), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 19, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 08), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 20, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 09), RoutineId = SelectedRoutine.Id });
            //    App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 11, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 10), RoutineId = SelectedRoutine.Id });
            //}

            RefreshRecordChart();
        }

        #region Property

        public Routine SelectedRoutine { get; set; }

        private List<RoutineRecord> routines;
        public List<RoutineRecord> RoutineRecords
        {
            get
            {
                return App.RecordRepo.RoutineRecordFromDB.Where(r => r.RoutineId == SelectedRoutine.Id).OrderBy(r => r.RecordTime).ToList();
            }
            set
            {
                if (routines == value) return;
                routines = value;
                OnPropertyChanged(nameof(RoutineRecords));
            }
        }

        public Chart ElapsedTimeChart { get; set; }
        public Chart TimeRemainingChart { get; set; }

        public int RecordTotalCount
        {
            get { return RoutineRecords.Count; }
        }

        private int recordStartPosition = -1;
        public int RecordStartPosition
        {
            get
            {
                if (recordStartPosition == -1) recordStartPosition = RecordEndPosition - (NumOfChartData - 1);
                if (recordStartPosition < 1) recordStartPosition = 1;
                return recordStartPosition;
            }
            set
            {
                if (recordStartPosition == value) return;
                recordStartPosition = value;
                OnPropertyChanged(nameof(RecordStartPosition));
            }
        }
        private int recordEndPosition = -1;
        public int RecordEndPosition
        {
            get
            {
                if (recordEndPosition == -1) recordEndPosition = RecordTotalCount;
                if (recordEndPosition > RecordTotalCount) recordEndPosition = RecordTotalCount;
                return recordEndPosition;
            }
            set
            {
                if (recordEndPosition == value) return;
                recordEndPosition = value;
                OnPropertyChanged(nameof(RecordEndPosition));
            }
        }

        public bool HasRecord
        {
            get 
            {
                if (RoutineRecords.Count == 0) return false;
                return true;
            }
        }

        public bool HasNoRecord
        {
            get { return !HasRecord; }
        }


        public Command NextRecordCommand { get; set; }
        public Command PreviousRecordCommand { get; set; }

        #endregion

        #region Method

        private void PreviousRecord()
        {
            if (RecordStartPosition <= 1) return;

            RecordEndPosition -= NumOfChartData;
            RecordStartPosition = RecordEndPosition - (NumOfChartData - 1);

            RefreshRecordChart();
        }
        
        private void NextRecord()
        {
            if (RecordEndPosition >= RecordTotalCount) return;

            RecordEndPosition += NumOfChartData;
            RecordStartPosition = RecordEndPosition - (NumOfChartData - 1);

            RefreshRecordChart();
        }

        private void RefreshRecordChart()
        {
            var elapsedEntries = new List<Entry>();
            var overEntries = new List<Entry>();

            for (int i = RecordStartPosition; i <= RecordEndPosition; i++)
            {
                var routine = RoutineRecords[i - 1];

                var entry = new Entry((float)routine.ElapsedTime.TotalSeconds)
                {
                    Label = $"{routine.RecordTime.Month}/{routine.RecordTime.Day} ({CreateTimeToString.ConvertDayOfWeekToKorDayOfWeek(routine.RecordTime)})",
                    Color = SKColor.Parse("#1565c0"),
                    ValueLabel = CreateTimeToString.TakenTimeToString_en(routine.ElapsedTime)
                };
                elapsedEntries.Add(entry);

                entry = new Entry((float)routine.TimeRemaining.TotalSeconds)
                {
                    Label = $"{routine.RecordTime.Month}/{routine.RecordTime.Day} ({CreateTimeToString.ConvertDayOfWeekToKorDayOfWeek(routine.RecordTime)})",
                    Color = SKColor.Parse("#90caf9"),
                    ValueLabel = CreateTimeToString.TakenTimeToString_en(routine.TimeRemaining)
                };
                overEntries.Add(entry);
            }

            ElapsedTimeChart = new BarChart()
            {
                Entries = elapsedEntries,
                LabelTextSize = DependencyService.Get<INativeFont>().GetNativeSize(LabelFontSize),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                AnimationDuration = TimeSpan.FromSeconds(AnimationSec)
            };

            TimeRemainingChart = new BarChart()
            {
                Entries = overEntries,
                LabelTextSize = DependencyService.Get<INativeFont>().GetNativeSize(LabelFontSize),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                AnimationDuration = TimeSpan.FromSeconds(AnimationSec)
            };
        }

        #endregion
    }
}
