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
        public RoutineRecordViewModel(INavigation navigation, Routine routine) : base(navigation)
        {
            SelectedRoutine = new Routine(routine);
            ConstructCommand();
            SubscribeMessage();

            Title = $"{routine.Name} 기록";

            InitChart();
        }
        private void ConstructCommand()
        {
        }

        private void SubscribeMessage()
        {
        }

        private void InitChart()
        {
            var elapsedEntries = new List<Entry>();
            
            if (RoutineRecords.Count == 0)
            {
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 15, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 01), RoutineId = SelectedRoutine.Id });
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 13, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 02), RoutineId = SelectedRoutine.Id });
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 11, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 03), RoutineId = SelectedRoutine.Id });
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 16, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 04), RoutineId = SelectedRoutine.Id });
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 22, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 05), RoutineId = SelectedRoutine.Id });
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 18, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 06), RoutineId = SelectedRoutine.Id });
                App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 12, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 07), RoutineId = SelectedRoutine.Id });
                //App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 19, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 08), RoutineId = SelectedRoutine.Id });
                //App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 20, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 09), RoutineId = SelectedRoutine.Id });
                //App.RecordRepo.SaveRoutineRecord(new RoutineRecord() { ElapsedTime = new TimeSpan(0, 11, 0), TotalTime = new TimeSpan(0, 20, 0), RecordTime = new DateTime(2019, 12, 10), RoutineId = SelectedRoutine.Id });
            }

            foreach (var routine in RoutineRecords)
            {
                var entry = new Entry((float)routine.ElapsedTime.TotalSeconds)
                {
                    Label = $"{routine.RecordTime.Month}/{routine.RecordTime.Day}",
                    Color = SKColor.Parse("#90caf9"),
                    ValueLabel = CreateTimeToString.TakenTimeToString(routine.ElapsedTime)
                };
                elapsedEntries.Add(entry);
            }

            ElapsedTimeChart = new BarChart()
            {
                Entries = elapsedEntries,
                LabelTextSize = DependencyService.Get<INativeFont>().GetNativeSize(15),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
        }

        #region Property

        public Routine SelectedRoutine { get; set; }

        private List<RoutineRecord> routines;
        public List<RoutineRecord> RoutineRecords
        {
            get
            {
                //if (routines == null)
                //{
                //    var _routines = 
                //}
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

        #endregion

        #region Method

        #endregion
    }
}
