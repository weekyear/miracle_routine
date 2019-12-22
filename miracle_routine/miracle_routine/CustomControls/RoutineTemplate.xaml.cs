using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.Views;
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
        Routine habit;
        public RoutineTemplate()
        {
            InitializeComponent();
        }
        private void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var habit = e.Item as Habit;

            App.MessagingCenter.SendShowHabitRecordMessage(habit);
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
            habit = menuBtn.BindingContext as Routine;

            await ShowMenu();
        }
        private async Task ShowMenu()
        {
            string[] actionSheetBtns = { StringResources.Modify, StringResources.Delete, StringResources.RoutineRecord };

            string action = await DependencyService.Get<MessageBoxService>().ShowActionSheet($"{habit.Name} {StringResources.Menu}", StringResources.Cancel, null, actionSheetBtns);

            if (action != StringResources.Cancel && !string.IsNullOrEmpty(action))
            {
                ClickMenuAction(action);
            }
        }

        private void ClickMenuAction(string action)
        {
            if (action == StringResources.Modify)
            {
                App.MessagingCenter.SendShowRoutineMessage(habit);
            }
            else if (action == StringResources.Delete)
            {
                void deleteAction() => DeleteRoutineAndHabitList();
                DependencyService.Get<MessageBoxService>().ShowConfirm(StringResources.DeleteRoutine, StringResources.AskDeleteRoutine, null, deleteAction);
            }
            else if (action == StringResources.RoutineRecord)
            {
                App.MessagingCenter.SendShowRoutineRecordMessage(habit);
            }
        }

        private void DeleteRoutineAndHabitList()
        {
            foreach (var habit in habit.HabitList)
            {
                App.HabitService.DeleteHabit(habit.Id);
            }
            App.RoutineService.DeleteRoutine(habit);
            App.MessagingCenter.SendChangeRoutineMessage();
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as Button;
            habit = menuBtn.BindingContext as Routine;
            App.MessagingCenter.SendShowRoutineActionMessage(habit);
        }

        public void SendChangeRoutineMessage()
        {
            MessagingCenter.Send(typeof(Routine), "changeRoutine");
        }
        public void SendShowRoutineActionMessage(Routine routine)
        {
            MessagingCenter.Send(typeof(Routine), "showRoutineAction", routine);
        }
        public void SendShowRoutineMessage(Routine routine)
        {
            MessagingCenter.Send(typeof(Routine), "showRoutine", routine);
        }
    }
}