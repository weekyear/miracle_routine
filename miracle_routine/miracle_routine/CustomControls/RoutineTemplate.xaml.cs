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
        Routine routine;
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

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as ImageButton;
            routine = menuBtn.BindingContext as Routine;

            await ShowMenu();
        }
        private async Task ShowMenu()
        {
            string[] actionSheetBtns = { StringResources.Modify, StringResources.Delete, StringResources.RoutineRecord };

            string action = await DependencyService.Get<MessageBoxService>().ShowActionSheet($"{routine.Name} {StringResources.Menu}", StringResources.Cancel, null, actionSheetBtns);

            if (action != StringResources.Cancel && !string.IsNullOrEmpty(action))
            {
                await ClickMenuAction(action);
            }
        }

        private async Task ClickMenuAction(string action)
        {
            if (action == StringResources.Modify)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutineSettingPage(routine)));
            }
            else if (action == StringResources.Delete)
            {
                void deleteAction() => DeleteRoutineAndHabitList();
                DependencyService.Get<MessageBoxService>().ShowConfirm(StringResources.DeleteRoutine, StringResources.AskDeleteRoutine, null, deleteAction);
            }
            else if (action == StringResources.RoutineRecord)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RoutineRecordPage(routine)));
            }
        }

        private void DeleteRoutineAndHabitList()
        {
            foreach (var habit in routine.HabitList)
            {
                App.HabitService.DeleteHabit(habit.Id);
            }
            App.RoutineService.DeleteRoutine(routine);
            App.MessagingCenter.SendChangeRoutineMessage();
        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as Button;
            routine = menuBtn.BindingContext as Routine;

            await App.Current.MainPage.Navigation.PushAsync(new RoutineActionPage(routine, null), true);
        }
    }
}