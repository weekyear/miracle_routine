using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Helpers
{
    public interface INotifySetter
    {
        void NotifySoonFinishHabit(Habit habit, string nextHabitName);
        void NotifyFinishHabit(Habit habit, string nextHabitName);
        void NotifyFinishRoutine(Routine routine, TimeSpan ElapsedTime);
        void NotifyHabitCount(Habit habit, TimeSpan countDown, bool isPause, bool isLastHabit);
        void CancelFinishHabitNotify();
        void CancelHabitCountNotify();
    }
}
