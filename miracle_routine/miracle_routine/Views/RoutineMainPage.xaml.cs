using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using miracle_routine.Models;
using miracle_routine.Views;
using miracle_routine.ViewModels;

namespace miracle_routine.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RoutinesPage : ContentPage
    {
        RoutineMainViewModel viewModel;

        public RoutinesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RoutineMainViewModel(Navigation);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Routine;
            if (item == null)
                return;

            // Manually deselect item.
            RoutineListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.RefreshRoutines();
        }
    }
}