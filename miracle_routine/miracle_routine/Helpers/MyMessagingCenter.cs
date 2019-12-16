using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Helpers
{
    public class MyMessagingCenter
    {
        public void SendChangeHabitMessage(Habit habit)
        {
            MessagingCenter.Send(this, "changeHabit", habit);
        }

        public void SendChangeRoutineMessage()
        {
            MessagingCenter.Send(this, "changeRoutine");
        }
        public void SendShowRoutineMessage(Routine routine)
        {
            MessagingCenter.Send(this, "showRoutine", routine);
        }
    }
}
