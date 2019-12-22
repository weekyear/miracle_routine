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
            MessagingCenter.Send(new MyMessagingCenter(), "addHabit", habit);
        }
        
        public void SendRemoveHabitMessage(Habit habit)
        {
            MessagingCenter.Send(new MyMessagingCenter(), "removeHabit", habit);
        }

        public void SendChangeRoutineMessage()
        {
            MessagingCenter.Send(new MyMessagingCenter(), "changeRoutine");
        }
        public void SendShowRoutineMessage(Routine routine)
        {
            MessagingCenter.Send(new MyMessagingCenter(), "showRoutine", routine);
        }

        public void SendShowRoutineActionMessage(Routine routine)
        {
            MessagingCenter.Send(new MyMessagingCenter(), "showRoutineAction", routine);
        }

        public void SendShowRoutineRecordMessage(Routine routine)
        {
            MessagingCenter.Send(new MyMessagingCenter(), "showRoutineRecord", routine);
        }
    }
}
