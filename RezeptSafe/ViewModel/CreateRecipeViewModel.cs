using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using RezeptSafe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class CreateRecipeViewModel : BaseViewModel
    {
        IRezeptService rezeptService;
        IUserService userService;
        IAlertService alertService;

        [ObservableProperty]
        Recipe recipe = new Recipe();

        [ObservableProperty]
        ObservableCollection<Utensil> allUtensils;
        [ObservableProperty]
        ObservableCollection<Utensil> filteredUtensils;
        [ObservableProperty]
        string utensilSearchText;


        [ObservableProperty]
        ObservableCollection<Ingredient> allIngredients;
        [ObservableProperty]
        ObservableCollection<Ingredient> filteredIngredients;
        [ObservableProperty]
        string ingredientSearchText;

        public CreateRecipeViewModel(IRezeptService rezeptservice, IUserService userService, IAlertService alertService) 
        {
            this.rezeptService = rezeptservice;
            this.userService = userService;
            this.alertService = alertService;

            this.recipe.Username = this.userService.GetUsername();

            this.AllIngredients = new ObservableCollection<Ingredient>();
            this.FilteredIngredients = new ObservableCollection<Ingredient>();
            this.AllUtensils = new ObservableCollection<Utensil>();
            this.FilteredUtensils = new ObservableCollection<Utensil>();
            this.IngredientSearchText = string.Empty;
            this.UtensilSearchText = string.Empty;

            this.QueryAllIngredients();
            this.QueryAllUtensils();
        }

        partial void OnIngredientSearchTextChanged(string value)
        {
            FilterIngredients(value);
        }

        void FilterIngredients(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                FilteredIngredients = new ObservableCollection<Ingredient>(AllIngredients);
            }
            else
            {
                FilteredIngredients = new ObservableCollection<Ingredient>(
                    AllIngredients.Where(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase)));
            }
        }
        partial void OnUtensilSearchTextChanged(string value)
        {
            FilterUtensils(value);
        }

        void FilterUtensils(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                FilteredUtensils = new ObservableCollection<Utensil>(AllUtensils);
            }
            else
            {
                FilteredUtensils = new ObservableCollection<Utensil>(
                    AllUtensils.Where(i => i.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase)));
            }
        }

        public async void QueryAllIngredients()
        {
            var ingredients = await this.rezeptService.GetAllIngredientsAsync();

            if (this.FilteredIngredients?.Count != 0)
            {
                this.FilteredIngredients?.Clear();
            }

            foreach (var ingredient in ingredients)
            {
                this.AllIngredients?.Add(ingredient);
                this.FilteredIngredients?.Add(ingredient);
            }
        }

        public async void QueryAllUtensils()
        {
            var utensils = await this.rezeptService.GetAllUtensilsAsync();

            if(this.FilteredUtensils.Count != 0)
            {
                this.FilteredUtensils?.Clear();
            }

            foreach (var utensil in utensils)
            {
                this.AllUtensils?.Add(utensil);
                this.FilteredUtensils?.Add(utensil);
            }
        }

        [RelayCommand]
        async Task OnSaveClickedAsync()
        {
            this.Recipe.Ingredients = this.AllIngredients.Where(x => x.IsSelected == true).ToList();

            this.Recipe.Utensils = this.AllUtensils.Where(x => x.IsSelected == true).ToList();

            if(!this.Recipe.IsValidRecipe(out string error))
            {
                await this.alertService.ShowAlertAsync("Error", error);
                return;
            }

            await this.rezeptService.AddRecipeAsync(this.Recipe);

            await Shell.Current.GoToAsync("..");
        }
    }
}
