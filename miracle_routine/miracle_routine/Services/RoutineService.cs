using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Services
{
    public class RoutineService : IRoutineService
    {
        public RoutineService(IRoutineRepo repository)
        {
            Repository = repository;

            Routines = GetAllRoutines();
        }

        public IRoutineRepo Repository { get; }

        public List<Routine> Routines { get; private set; } = new List<Routine>();


        public Routine GetRoutine(int id)
        {
            return Routines.FirstOrDefault(m => m.Id == id);
        }

        public List<Routine> GetAllRoutines()
        {
            return AssignDaysToRoutines().ToList();
        }

        private IEnumerable<Routine> AssignDaysToRoutines()
        {
            var routines = Repository.RoutinesFromDB;
            var daysOfWeeks = Repository.DaysOfWeeksFromDB;

            foreach (var days in daysOfWeeks)
            {
                foreach (var routine in routines)
                {
                    if (routine.DaysId == days.Id)
                    {
                        routine.Days = days;
                    }
                }
            }

            return routines;
        }

        public int SaveRoutine(Routine routine)
        {
            var id = SaveRoutineAtLocal(routine);
            DependencyService.Get<IAlarmSetter>().SetRoutineAlarm(routine);

            return id;
        }

        public int SaveRoutineAtLocal(Routine routine)
        {
            routine.DaysId = Repository.SaveDaysOfWeek(routine.Days);
            var id = Repository.SaveRoutine(routine);
            RefreshRoutines();

            return id;
        }

        public void RefreshRoutines()
        {
            Routines = GetAllRoutines();
        }

        public int DeleteRoutine(Routine routine)
        {
            DependencyService.Get<IAlarmSetter>().DeleteRoutineAlarm(routine.Id);
            var id = Repository.DeleteRoutine(routine.Id);
            Repository.DeleteDaysOfWeek(routine.DaysId);
            RefreshRoutines();

            return id;
        }

        public void DeleteAllRoutines()
        {
            foreach (var routine in Routines)
            {
                DeleteRoutine(routine);
            }
        }
    }
}
