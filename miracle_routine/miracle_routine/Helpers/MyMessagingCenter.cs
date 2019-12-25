using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Helpers
{
    public class MyMessagingCenter
    {
        public void SendChangeRoutineMessage()
        {
            MessagingCenter.Send(new MyMessagingCenter(), "changeRoutine");
        }
    }
}
