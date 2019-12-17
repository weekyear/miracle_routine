using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public interface IRoutineRepo
    {
        // getters
        List<Routine> RoutinesFromDB { get; }
        IEnumerable<DaysOfWeek> DaysOfWeeksFromDB { get; }

        // Routine
        Routine GetRoutine(int id);
        IEnumerable<Routine> GetRoutines();

        int SaveRoutine(Routine routine);

        int DeleteRoutine(int id);
        void DeleteAllRoutines();


        // DayOfWeek
        DaysOfWeek GetDaysOfWeek(int id);
        IEnumerable<DaysOfWeek> GetAllDaysOfWeeks();

        int SaveDaysOfWeek(DaysOfWeek daysOfWeek);

        int DeleteDaysOfWeek(int id);

        void DeleteAllDaysOfWeeks();
    }
}
