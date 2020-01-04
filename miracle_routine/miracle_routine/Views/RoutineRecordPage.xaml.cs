using miracle_routine.CustomControls;
using miracle_routine.Helpers;
using miracle_routine.Models;
using miracle_routine.Resources;
using miracle_routine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace miracle_routine.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutineRecordPage : ContentPage
    {
        private RoutineRecordViewModel viewModel;
        public RoutineRecordPage(Routine routine)
        {
            BindingContext = viewModel = new RoutineRecordViewModel(Navigation, routine);

            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            DependencyService.Get<IAdMobInterstitial>().Show();

            return base.OnBackButtonPressed();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (WeekListView.SelectedItem != null || e.SelectedItem != null)
            {
                ((NoRippleListView)sender).SelectedItem = null;
            }
        }
    }
}