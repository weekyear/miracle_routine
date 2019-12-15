using miracle_routine.Models;
using miracle_routine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return Repository.RoutinesFromDB;
        }

        public int SaveRoutine(Routine routine)
        {
            var id = Repository.SaveRoutine(routine);
            RefreshRoutines();
            //DependencyService.Get<MyMessagingCenter>().SendChangeRoutinesMessage();

            return id;
        }

        public void RefreshRoutines()
        {
            Routines = GetAllRoutines();
        }

        public int DeleteRoutine(int id)
        {
            var dailyId = Repository.DeleteRoutine(id);
            RefreshRoutines();

            return dailyId;
        }

        public void DeleteAllRoutines()
        {
            foreach (var dailyRecord in Routines)
            {
                DeleteRoutine(dailyRecord.Id);
            }
        }
    }
}
