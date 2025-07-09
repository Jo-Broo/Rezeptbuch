using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rezeptbuch.Interfaces;
using Rezeptbuch.Model;
using Rezeptbuch.View;
using System.Text.Json;

namespace Rezeptbuch.ViewModel
{
    [QueryProperty("Recipe", "Recipe")]
    public partial class RecipeDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Recipe recipe;

        IRezeptShareService rezeptShareService;

        IRezeptService rezeptService;

        public RecipeDetailsViewModel(IAlertService alertService, IRezeptShareService rezeptShareService, IRezeptService rezeptService) : base(alertService)
        {
            this.rezeptShareService = rezeptShareService;
            this.rezeptService = rezeptService;
            this.Title = "Detailansicht";
        }

        [RelayCommand]
        void OnShowQRClickedAsync()
        {
            var popup = new QRCodePopup();
            
            popup.SetQRCodeValue(this.rezeptShareService.CompressJsonToBase64(JsonSerializer.Serialize(this.Recipe)));

            Shell.Current.ShowPopup(popup);
        }

        [RelayCommand]
        async Task OnDeleteRecipeAsync()
        {
            if(this.Recipe is not null)
            {
                if(await this._alertService.ShowAlertWithChoiceAsync("Warnung","Wollen sie das Rezept wirklich löschen?", "Ja", "Nein"))
                {
                    int x = await this.rezeptService.DeleteRecipeAsync(this.Recipe.ID);

                    var result = await this.rezeptService.GetRecipeAsync(this.Recipe.ID);

                    if (result is null && x == 1)
                    {
                        await this._alertService.ShowAlertAsync("Info", "Das Rezept wurde erfolgreich gelöscht");

                        await Shell.Current.GoToAsync("..");
                        return;
                    }
                }
                return;
            }
            await this._alertService.ShowAlertAsync("Error", "Das Rezept konnte nicht gelöscht werden");
            return;
        }

        [RelayCommand]
        async Task NavigateToEditRecipe()
        {
            if (this.Recipe is not null)
            {
                await Shell.Current.GoToAsync($"{nameof(CreateRecipePage)}", true,
                new Dictionary<string, object>
                {
                    {"Recipe", this.Recipe }
                });
            }
        }
    }
}