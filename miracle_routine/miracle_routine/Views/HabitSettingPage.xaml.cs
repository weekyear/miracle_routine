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
        private void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedHabit = e.Item as Habit;
            viewModel.Image = selectedHabit.Image;
            viewModel.Name = selectedHabit.Name;
            viewModel.Minutes = selectedHabit.Minutes;
            viewModel.Seconds = selectedHabit.Seconds;
            viewModel.Description = selectedHabit.Description;
        }

        private void HabitListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (HabitListView.SelectedItem != null || e.SelectedItem != null)
            {
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}