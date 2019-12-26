using Microcharts;
using miracle_routine.Helpers;
using miracle_routine.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace miracle_routine.ViewModels
{
    public class HabitRecordViewModel : BaseViewModel
    {
        private readonly int NumOfChartData = 5;
        private readonly int LabelFontSize = 10;
        private readonly int AnimationSec = 1;
        public HabitRecordViewModel(INavigation navigation, Habit habit) : base(navigation)
        {
            SelectedHabit = new Habit(habit);
            ConstructCommand();
            SubscribeMessage();

            Title = $"<{habit.Name}> 습관 기록";

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
            //    App.RecordRepo.SaveHabitRecord(new HabitRecord() { ElapsedTime = new TimeSpan(0, 15, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 01), HabitId = 1, RoutineRecordId = 1 });
            //    App.RecordRepo.SaveHabitRecord(new HabitRecord() { ElapsedTime = new TimeSpan(0, 13, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 02), HabitId = 1, RoutineRecordId = 1 });
            //    App.RecordRepo.SaveHabitRecord(new HabitRecord() { ElapsedTime = new TimeSpan(0, 11, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 03), HabitId = 2, RoutineRecordId = 2 });
            //    App.RecordRepo.SaveHabitRecord(new HabitRecord() { ElapsedTime = new TimeSpan(0, 16, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 04), HabitId = 2, RoutineRecordId = 2 });
            //    App.RecordRepo.SaveHabitRecord(new HabitRecord() { ElapsedTime = new TimeSpan(0, 22, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 05), HabitId = 1, RoutineRecordId = 1 });
            //    App.RecordRepo.SaveHabitRecord(new HabitRecord() { ElapsedTime = new TimeSpan(0, 18, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 06), HabitId = 1, RoutineRecordId = 1 });
            //    App.RecordRepo.SaveHabitRecord(new HabitRecord() { ElapsedTime = new TimeSpan(0, 12, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 07), HabitId = 1, RoutineRecordId = 1 });
            //}

            //RecordEndPosition = RecordTotalCount;
            //RecordStartPosition = RecordEndPosition - NumOfChartData;

            RefreshRecordChart();
        }

        #region Property

        public Habit SelectedHabit { get; set; }


        public string Image
        {
            get { return SelectedHabit.Image; }
            set
            {
                if (SelectedHabit.Image == value) return;
                SelectedHabit.Image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private List<HabitRecord> habitRecords;
        public List<HabitRecord> HabitRecords
        {
            get
            {
                return App.RecordRepo.HabitRecordFromDB.Where(r => r.HabitId == SelectedHabit.Id).OrderBy(r => r.RecordTime).ToList();
            }
            set
            {
                if (habitRecords == value) return;
                habitRecords = value;
                OnPropertyChanged(nameof(HabitRecords));
            }
        }

        public Chart ElapsedTimeChart { get; set; }
        public Chart TimeRemainingChart { get; set; }

        public int RecordTotalCount
        {
            get { return HabitRecords.Count; }
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
                if (value < 1) value = 1;
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
                if (value > RecordTotalCount) value = RecordTotalCount;
                recordEndPosition = value;
                OnPropertyChanged(nameof(RecordEndPosition));
            }
        }

        public bool HasRecord
        {
            get
            {
                if (HabitRecords.Count == 0) return false;
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
            var remainingEntries = new List<Entry>();

            for (int i = RecordStartPosition; i <= RecordEndPosition; i++)
            {
                var habitRecord = HabitRecords[i - 1];

                var entry = new Entry((float)habitRecord.ElapsedTime.TotalSeconds)
                {
                    Label = $"{habitRecord.RecordTime.Month}/{habitRecord.RecordTime.Day} ({CreateTimeToString.ConvertDayOfWeekToKorDayOfWeek(habitRecord.RecordTime)})",
                    Color = SKColor.Parse("#1565c0"),
                    ValueLabel = CreateTimeToString.TakenTimeToString_en(habitRecord.ElapsedTime)
                };
                elapsedEntries.Add(entry);

                entry = new Entry((float)habitRecord.TimeRemaining.TotalSeconds)
                {
                    Label = $"{habitRecord.RecordTime.Month}/{habitRecord.RecordTime.Day} ({CreateTimeToString.ConvertDayOfWeekToKorDayOfWeek(habitRecord.RecordTime)})",
                    Color = SKColor.Parse("#90caf9"),
                    ValueLabel = CreateTimeToString.TakenTimeToString_en(habitRecord.TimeRemaining)
                };
                remainingEntries.Add(entry);
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
                Entries = remainingEntries,
                LabelTextSize = DependencyService.Get<INativeFont>().GetNativeSize(LabelFontSize),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                AnimationDuration = TimeSpan.FromSeconds(AnimationSec)
            };
        }
        #endregion
    }
}
