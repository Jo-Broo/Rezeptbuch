using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class IngredientListViewModel : BaseViewModel
    {
        IRezeptService _rezeptService;

        [ObservableProperty]
        ObservableCollection<Ingredient> _allIngredients = new ObservableCollection<Ingredient>();
        [ObservableProperty]
        ObservableCollection<Ingredient> _filteredIngredients = new ObservableCollection<Ingredient>();
        [ObservableProperty]
        string ingredientSearchtext = string.Empty;

        public IngredientListViewModel(IAlertService alertService, IRezeptService rezeptService) : base(alertService)
        {
            this.Title = "Zutatenliste verwalten";
            this._rezeptService = rezeptService;
        }

        partial void OnIngredientSearchtextChanged(string value)
        {
            this.IsBusy = true;
            this.FilteredIngredients.Clear();

            if (!string.IsNullOrWhiteSpace(value))
            {
                Regex regex = new Regex($"^{Regex.Escape(value)}.*$", RegexOptions.IgnoreCase);

                this.FilteredIngredients = new ObservableCollection<Ingredient>(this.AllIngredients.Where(i => regex.IsMatch(i.NAME)));
            }
            else
            {
                foreach (var ingredient in this.AllIngredients)
                {
                    this.FilteredIngredients.Add(ingredient);
                }
            }
            this.IsBusy = false;
        }
        public async Task QueryAllIngredientsAsync()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var ingredients = await this._rezeptService.GetAllIngredientsAsync();

            if(this.AllIngredients.Count != 0)
            {
                this.AllIngredients.Clear();
            }

            foreach (var ingredient in ingredients)
            {
                this.AllIngredients?.Add(ingredient);
            }

            this.OnIngredientSearchtextChanged("");

            this.IsBusy = false;
        }
        [RelayCommand]
        async Task RemoveIngrdientAsync(Ingredient ingredient)
        {
            if (await this._alertService.ShowAlertWithChoiceAsync("Achtung", $"Wollen sie {ingredient.NAME} wirklich löschen?", "Ja", "Nein"))
            {
                var recipes = await this._rezeptService.GetAllRecipesWithIngredientAsync(ingredient);
                
                if(recipes is null)
                {
                    await this._alertService.ShowAlertAsync("Error", $"Beim abfragen der Rezepte ist ein Fehler aufgetreten");
                    return;
                }

                if (recipes.Count > 0)
                {
                    await this._alertService.ShowAlertAsync("Error", $"Die Zutat kann nicht gelöscht werden da sie in {recipes.Count} Rezepten verwendet wird");
                    return;
                }

                await this._rezeptService.DeleteIngredientAsync(ingredient.ID);

                if (await this._rezeptService.GetIngredientByIDAsync(ingredient.ID) is not null)
                {
                    await this._alertService.ShowAlertAsync("Info", "Beim entfernen der Zutat ist ein Fehler aufgetreten");
                    return;
                }

                await this.QueryAllIngredientsAsync();
                this.IngredientSearchtext = "";
                await this._alertService.ShowAlertAsync("Info", "Die Zutat wurde erfolgreich entfernt");
            }
        }
        [RelayCommand]
        async Task CreateNewIngredientAsync()
        {
            Ingredient newIngredient = new Ingredient();
            newIngredient.NAME = await this._alertService.ShowPromptAsync("Neue Zutat erstellen", "Name der Zutat");

            if (!string.IsNullOrWhiteSpace(newIngredient.NAME))
            {
                if (await this._rezeptService.AddIngredientAsync(newIngredient) == 1)
                {
                    await this._alertService.ShowAlertAsync("Info", $"Die Zutat {newIngredient.NAME} wurde erfolgreich erstellt");
                    await this.QueryAllIngredientsAsync();
                    return;
                }
                else
                {
                    await this._alertService.ShowAlertAsync("Fehler", $"Die Zutat {newIngredient.NAME} wurde nicht erfolgreich erstellt");
                    return;
                }
            }

            await this._alertService.ShowAlertAsync("Fehler", "Der Zutatenname darf nicht leer sein");
        }
    }
}
