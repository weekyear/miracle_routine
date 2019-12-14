using System;

using miracle_routine.Models;

namespace miracle_routine.ViewModels
{
    public class RoutineDetailViewModel : BaseViewModel
    {
        public Routine Item { get; set; }
        public RoutineDetailViewModel(Routine item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
