using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Helpers
{
    public interface INotifySetter
    {
        void NotifyFinishHabit(Habit habit, string nextHabitName);
        void NotifyHabitCount(Habit habit, TimeSpan countDown);
        void CancelFinishHabitNotify();
        void CancelHabitCountNotify();
    }
}
