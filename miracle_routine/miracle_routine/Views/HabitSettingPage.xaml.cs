using miracle_routine.CustomControls;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HabitImageGrid.OnHabitImageSeleted += HabitImageChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            HabitImageGrid.OnHabitImageSeleted -= HabitImageChanged;
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

        private void HabitImageChanged(string image_string)
        {
            viewModel.Image = image_string;
            ChangeImageFrameVisibleState();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ChangeImageFrameVisibleState();
        }

        private void ChangeImageFrameVisibleState()
        {
            ImageFrame.IsVisible = !ImageFrame.IsVisible;
        }

        private void HabitListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (HabitListView.SelectedItem != null || e.SelectedItem != null)
            {
                ((ListView)sender).SelectedItem = null;
            }
        }

        private void ImageListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ImageListView.SelectedItem != null || e.SelectedItem != null)
            {
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}