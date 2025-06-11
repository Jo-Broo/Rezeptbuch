using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using RezeptSafe.Services;
using RezeptSafe.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class RecipesViewModel : BaseViewModel
    {
        IRezeptService rezeptService;
        public ObservableCollection<Recipe> Recipes{ get; } = new();

        public RecipesViewModel(IRezeptService rezeptService)
        {
            this.Title = "Rezeptübersicht";
            this.rezeptService = rezeptService;
        }

        [RelayCommand]
        async Task GoToDetailsAsync(Recipe recipe)
        {
            if(recipe is null)
            {
                return;
            }

            //await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?id={recipe.Id}", true);

            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
                new Dictionary<string, object>
                {
                    {"Recipe", recipe }
                });
        }

        [RelayCommand]
        async Task GoToCreateRecipeAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(CreateRecipePage)}", true);
        }

        [RelayCommand]
        async Task GetRecipesAsync()
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;
                var recipes = await this.rezeptService.GetAllRecipesAsync();

                if(this.Recipes.Count != 0)
                {
                    this.Recipes.Clear();
                }

                foreach (var recipe in recipes) 
                {
                    recipe.Ingredients = await this.rezeptService.GetIngredientsForRecipeAsync(recipe.Id);
                    recipe.Utensils = await this.rezeptService.GetUtensilsForRecipeAsync(recipe.Id);
                    this.Recipes.Add(recipe);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally 
            {
                this.IsBusy = false;
            }
        }
    }
}
