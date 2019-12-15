using System;

using miracle_routine.Models;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RoutineDetailViewModel : BaseViewModel
    {
        public Routine Item { get; set; }
        public RoutineDetailViewModel(INavigation navigation, Routine routine = null) : base(navigation)
        {
            Title = routine?.Name;
            Item = routine;
        }
    }
}
