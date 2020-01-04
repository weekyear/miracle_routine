using miracle_routine.Models;
using miracle_routine.Views;
using Plugin.SharedTransitions;
using System;
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

            await App.Current.MainPage.Navigation.PushAsync(new RoutineActionPage(routine, 0), true);
        }

        private async void StatButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as ImageButton;
            var routine = menuBtn.BindingContext as Routine;

            await App.Current.MainPage.Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RecordPage(routine)));
        }
        
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as ImageButton;
            var routine = menuBtn.BindingContext as Routine;

            await App.Current.MainPage.Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutineSettingPage(routine)));
        }
    }
}