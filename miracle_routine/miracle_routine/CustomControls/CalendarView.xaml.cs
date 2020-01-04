using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static miracle_routine.ViewModels.RoutineRecordViewModel;

namespace miracle_routine.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        public CalendarView()
        {
            InitializeComponent();
        }


        public static readonly BindableProperty WeekRecordsProperty =
            BindableProperty.Create(nameof(WeekRecords),
                typeof(List<WeekRecord>),
                typeof(CalendarView),
                null,
                BindingMode.TwoWay);

        public List<WeekRecord> WeekRecords
        {
            get
            {
                var dd = (List<WeekRecord>)GetValue(WeekRecordsProperty);
                return dd;
            }
            set
            {
                SetValue(WeekRecordsProperty, value);
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (WeekListView.SelectedItem != null || e.SelectedItem != null)
            {
                ((NoRippleListView)sender).SelectedItem = null;
            }
        }
    }
}