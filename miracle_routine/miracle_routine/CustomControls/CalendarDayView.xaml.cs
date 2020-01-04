using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace miracle_routine.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarDayView : ContentView
    {
        public CalendarDayView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty RoutineRecordProperty =
            BindableProperty.Create(nameof(RoutineRecord),
                typeof(RoutineRecord),
                typeof(CalendarDayView),
                null,
                BindingMode.TwoWay,
                propertyChanged: RoutineRecordOnChanged);

        public RoutineRecord RoutineRecord
        {
            get
            {
                return (RoutineRecord)GetValue(RoutineRecordProperty);
            }
            set
            {
                SetValue(RoutineRecordProperty, value);
            }
        }

        public event EventHandler RecordChanged;
        static void RoutineRecordOnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Property changed implementation goes here
            var button = (CalendarDayView)bindable;
            button.RecordChanged?.Invoke(button, null);
        }

        public bool IsNotNullRecord
        {
            get
            {
                if (RoutineRecord.Id != 0) return true;
                return false;
            }
        }

        public int Date
        {
            get { return RoutineRecord.RecordTime.Day; }
        }

        public bool IsSuccess
        {
            get { return RoutineRecord.IsSuccess; }
        }
    }
}