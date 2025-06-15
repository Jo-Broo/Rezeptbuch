using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using RezeptSafe.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    [QueryProperty("Recipe", "Recipe")]
    public partial class RecipeDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Recipe recipe;

        IRezeptShareService rezeptShareService;

        public RecipeDetailsViewModel(IAlertService alertService, IRezeptShareService rezeptShareService) : base(alertService)
        {
            this.rezeptShareService = rezeptShareService;
        }

        [RelayCommand]
        async Task OnShowQRClickedAsync()
        {
            var popup = new QRCodePopup();
            
            popup.SetQRCodeValue(this.rezeptShareService.CompressJsonToBase64(JsonSerializer.Serialize(this.Recipe)));

            Shell.Current.ShowPopup(popup);
        }
    }
}