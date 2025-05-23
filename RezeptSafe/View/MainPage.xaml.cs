using RezeptSafe.Services;
using RezeptSafe.Model;
using System.Collections.ObjectModel;
using RezeptSafe.ViewModel;

namespace RezeptSafe.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(RecipesViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}