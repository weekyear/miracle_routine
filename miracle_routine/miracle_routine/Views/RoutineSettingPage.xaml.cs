using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using miracle_routine.Models;
using miracle_routine.ViewModels;
using Plugin.SharedTransitions;

namespace miracle_routine.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RoutineSettingPage : ContentPage
    {
        public Routine Item { get; set; }
        private RoutineSettingViewModel viewModel;

        public RoutineSettingPage(Routine routine)
        {
            InitializeComponent();

            BindingContext = viewModel = new RoutineSettingViewModel(Navigation, routine);
        }

        protected override void OnAppearing()
        {
            viewModel.RefreshHabits();
            base.OnAppearing();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var habit = e.Item as Habit;
            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new HabitSettingPage(new Habit(habit)))).ConfigureAwait(false);
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