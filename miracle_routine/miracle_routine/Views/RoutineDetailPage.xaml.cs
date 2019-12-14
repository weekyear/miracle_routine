using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using miracle_routine.Models;
using miracle_routine.ViewModels;

namespace miracle_routine.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RoutineDetailPage : ContentPage
    {
        RoutineDetailViewModel viewModel;

        public RoutineDetailPage(RoutineDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public RoutineDetailPage()
        {
            InitializeComponent();

            var item = new Routine
            {
                Name = "Item 1",
            };

            viewModel = new RoutineDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}