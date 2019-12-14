using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Helpers
{
    public interface IAlarmSetter
    {
        void SetNextNotifyFromVM();
        void CancelNotify();
    }
}
