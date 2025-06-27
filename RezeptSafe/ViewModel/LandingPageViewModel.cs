using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class LandingPageViewModel : BaseViewModel
    {
        public LandingPageViewModel(IAlertService alertService) : base(alertService)
        {
        }

        [RelayCommand]
        public async Task NavigateToListRecipesAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(ListRecipesPage)}", true);
        }

        [RelayCommand]
        public async Task NavigateToCreateRecipeAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(CreateRecipePage)}", true);
        }

        [RelayCommand]
        public async Task NavigateToSettingsAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}", true);
        }

        [RelayCommand]
        public async Task NavigateToProfileAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(ProfilPage)}", true);
        }
    }
}
