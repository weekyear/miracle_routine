using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Helpers
{
    public class MyMessagingCenter
    {
        public void SendAddHabitMessage(Habit habit)
        {
            MessagingCenter.Send(this, "addHabit", habit);
        }
        
        public void SendRemoveHabitMessage(Habit habit)
        {
            MessagingCenter.Send(this, "removeHabit", habit);
        }

        public void SendChangeRoutineMessage()
        {
            MessagingCenter.Send(this, "changeRoutine");
        }
        public void SendShowRoutineMessage(Routine routine)
        {
            MessagingCenter.Send(this, "showRoutine", routine);
        }

        public void SendShowRoutineActionMessage(Routine routine)
        {
            MessagingCenter.Send(this, "showRoutineAction", routine);
        }
    }
}
