using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Helpers
{
    public interface IAlarmSetter
    {
        void SetRoutineAlarm(Routine routine);

        void DeleteRoutineAlarm(int id);

        void DeleteAllRoutineAlarms(IEnumerable<Routine> routines);
    }
}
