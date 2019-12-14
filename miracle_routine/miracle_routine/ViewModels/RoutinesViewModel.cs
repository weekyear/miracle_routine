using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using miracle_routine.Models;
using miracle_routine.Views;

namespace miracle_routine.ViewModels
{
    public class RoutinesViewModel : BaseViewModel
    {
        public ObservableCollection<Routine> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public RoutinesViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Routine>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<RoutinePage, Routine>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Routine;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}