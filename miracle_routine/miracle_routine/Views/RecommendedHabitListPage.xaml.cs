using miracle_routine.Models;
using miracle_routine.ViewModels;
using Plugin.SharedTransitions;
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
    public partial class RecommendedHabitListPage : ContentPage
    {
        RecommendedHabitListViewModel viewModel;

        public RecommendedHabitListPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RecommendedHabitListViewModel(Navigation);
        }

        private async void HabitListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedHabit = e.Item as RecommendedHabit;

            await Navigation.PushModalAsync(new SharedTransitionNavigationPage(new RecommendedHabitSettingPage(new RecommendedHabit(selectedHabit)))).ConfigureAwait(false);
        }

        private void HabitListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (HabitListView.SelectedItem != null || e.SelectedItem != null)
            {
                ((ListView)sender).SelectedItem = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.RefreshRecommendedHabitList();
        }
    }
}