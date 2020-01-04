using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static miracle_routine.CustomControls.CalendarView;
using static miracle_routine.ViewModels.RoutineRecordViewModel;

namespace miracle_routine.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarWeekView : ContentView
    {
        public CalendarWeekView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty WeekRecordProperty =
            BindableProperty.Create(nameof(WeekRecord),
                typeof(WeekRecord),
                typeof(CalendarWeekView),
                null,
                BindingMode.TwoWay);

        public WeekRecord WeekRecord
        {
            get
            {
                return (WeekRecord)GetValue(WeekRecordProperty);
            }
            set
            {
                SetValue(WeekRecordProperty, value);
            }
        }
    }
}