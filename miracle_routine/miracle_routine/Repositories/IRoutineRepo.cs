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

        // Routine
        Routine GetRoutine(int id);
        IEnumerable<Routine> GetRoutines();

        int SaveRoutine(Routine routine);

        int DeleteRoutine(int id);
        void DeleteAllRoutines();
    }
}
