using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.Views;
using Plugin.SharedTransitions;
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
    public partial class RoutineTemplate : ContentView
    {
        public RoutineTemplate()
        {
            InitializeComponent();
        }

        private async void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var habit = e.Item as Habit;

            await App.Current.MainPage.Navigation.PushModalAsync(new SharedTransitionNavigationPage(new HabitRecordPage(habit)));

            //App.MessagingCenter.SendShowHabitRecordMessage(habit);
        }

        private void HabitListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (HabitListView.SelectedItem != null || e.SelectedItem != null)
            {
                ((ListView)sender).SelectedItem = null;
            }
        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as Button;
            var routine = menuBtn.BindingContext as Routine;

            await App.Current.MainPage.Navigation.PushAsync(new RoutineActionPage(routine, null), true);
        }

        private async void StatButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as ImageButton;
            var routine = menuBtn.BindingContext as Routine;

            await App.Current.MainPage.Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutineRecordPage(routine)));
        }
        
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as ImageButton;
            var routine = menuBtn.BindingContext as Routine;

            await App.Current.MainPage.Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutineSettingPage(routine)));
        }
    }
}