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
    public partial class HabitSettingPage : ContentPage
    {
        HabitSettingViewModel viewModel;
        public HabitSettingPage(Habit habit)
        {
            InitializeComponent();

            BindingContext = viewModel = new HabitSettingViewModel(Navigation, habit);
        }
    }
}