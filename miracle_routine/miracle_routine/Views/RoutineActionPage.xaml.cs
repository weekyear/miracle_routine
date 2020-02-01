using miracle_routine.Models;
using miracle_routine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace miracle_routine.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutineActionPage : ContentPage
    {
        RoutineActionViewModel viewModel;
        public RoutineActionPage(Routine routine, int habitIndex, TimeSpan currentHabitTime = new TimeSpan())
        {
            InitializeComponent();
            BindingContext = viewModel = new RoutineActionViewModel(Navigation, routine, habitIndex, currentHabitTime);
        }

        protected override bool OnBackButtonPressed()
        {
            viewModel.Close();
            return true;
        }

        protected override void OnAppearing()
        {
            //ConstuctNotifictionCommand();
            base.OnAppearing();
        }
    }
}