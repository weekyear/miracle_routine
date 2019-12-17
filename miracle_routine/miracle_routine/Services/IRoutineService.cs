using miracle_routine.Models;
using miracle_routine.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Services
{
    public interface IRoutineService
    {
        IRoutineRepo Repository { get; }
        List<Routine> Routines { get; }
        Routine GetRoutine(int id);
        int DeleteRoutine(Routine routine);
        int SaveRoutine(Routine routine);
        void DeleteAllRoutines();
        List<Routine> GetAllRoutines();
        void RefreshRoutines();
    }
}
