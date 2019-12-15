using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace miracle_routine.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutineTemplate : ContentView
    {
        public RoutineTemplate()
        {
            InitializeComponent();
        }
        private void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void HabitListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //if (HabitListView.SelectedItem != null || e.SelectedItem != null)
            //{
            //    ((ListView)sender).SelectedItem = null;
            //}
        }
    }
}