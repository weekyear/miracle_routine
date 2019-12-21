using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace miracle_routine.Helpers
{
    public class OrderableCollection<T> : ObservableCollection<T>, IOrderable
    {
        public OrderableCollection() : base()
        {
        }

        public event EventHandler OrderChanged;

        public void ChangeOrdinal(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || newIndex < 0) return;

            var priorIndex = oldIndex;
            var latterIndex = newIndex;

            var changedItem = Items[oldIndex];
            if (newIndex < oldIndex)
            {
                // add one to where we delete, because we're increasing the index by inserting
                priorIndex += 1;
            }
            else
            {
                // add one to where we insert, because we haven't deleted the original yet
                latterIndex += 1;
            }

            Items.Insert(latterIndex, changedItem);
            Items.RemoveAt(priorIndex);

            OrderChanged?.Invoke(this, EventArgs.Empty);

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    changedItem,
                    newIndex,
                    oldIndex));

            if (Items is IEnumerable<Habit> habits)
            {
                if (habits != null)
                {
                    int i = 0;
                    foreach (var toDo in habits)
                    {
                        toDo.Index = i++;
                    }
                }

                App.HabitService.SaveHabits(habits);
            }
        }
    }
}
