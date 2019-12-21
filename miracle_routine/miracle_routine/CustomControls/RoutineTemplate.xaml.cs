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
        Routine routine;
        public RoutineTemplate()
        {
            InitializeComponent();
        }
        private void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

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
            string[] actionSheetBtns = { StringResources.Modify, StringResources.Delete };

            string action = await DependencyService.Get<MessageBoxService>().ShowActionSheet($"{routine.Name} {StringResources.Menu}", StringResources.Cancel, null, actionSheetBtns);

            if (action != StringResources.Cancel && !string.IsNullOrEmpty(action))
            {
                ClickMenuAction(action);
            }
        }

        private void ClickMenuAction(string action)
        {
            if (action == StringResources.Modify)
            {
                App.MessagingCenter.SendShowRoutineMessage(routine);
            }
            else if (action == StringResources.Delete)
            {
                void deleteAction() => DeleteRoutineAndHabitList();
                DependencyService.Get<MessageBoxService>().ShowConfirm(StringResources.DeleteRoutine, StringResources.AskDeleteRoutine, null, deleteAction);
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

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            var menuBtn = sender as Button;
            routine = menuBtn.BindingContext as Routine;
            App.MessagingCenter.SendShowRoutineActionMessage(routine);
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