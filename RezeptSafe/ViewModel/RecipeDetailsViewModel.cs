using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Model;
using RezeptSafe.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    [QueryProperty("Recipe", "Recipe")]
    public partial class RecipeDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Recipe recipe;

        [RelayCommand]
        async void OnShowQrClickedAsync()
        {
            await Shell.Current.GoToAsync(nameof(QRCodePopup),true);
        }
    }
}