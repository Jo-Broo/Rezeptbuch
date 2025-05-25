using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        Recipe recipe = new Recipe();

        public ObservableCollection<Utensil> Utensils { get; private set; } = new ObservableCollection<Utensil>();
        List<Utensil> allUtensils = new List<Utensil>();

        [ObservableProperty]
        ObservableCollection<IngredientWithAmount> allIngredients;

        [ObservableProperty]
        ObservableCollection<IngredientWithAmount> filteredIngredients;

        [ObservableProperty]
        string ingredientSearchText;

        public CreateRecipeViewModel(IRezeptService rezeptservice, IUserService userService) 
        {
            this.rezeptService = rezeptservice;
            this.userService = userService;

            this.recipe.Username = this.userService.GetUsername();

            this.AllIngredients = new ObservableCollection<IngredientWithAmount>();
            this.FilteredIngredients = new ObservableCollection<IngredientWithAmount>();
            this.IngredientSearchText = string.Empty;

            this.QueryAllIngredients();
        }

        partial void OnIngredientSearchTextChanged(string value)
        {
            FilterIngredients(value);
        }

        void FilterIngredients(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                FilteredIngredients = new ObservableCollection<IngredientWithAmount>(AllIngredients);
            }
            else
            {
                FilteredIngredients = new ObservableCollection<IngredientWithAmount>(
                    AllIngredients.Where(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase)));
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
                var ingredientwithamount = new IngredientWithAmount
                {
                    Name = ingredient.Name,
                    Description = ingredient.Description
                };

                this.AllIngredients?.Add(ingredientwithamount);
                this.FilteredIngredients?.Add(ingredientwithamount);
            }
        }

        public async void QueryAllUtensils()
        {
            this.allUtensils = await this.rezeptService.GetAllUtensilsAsync();

            if(this.Utensils.Count != 0)
            {
                this.Utensils.Clear();
            }

            foreach (var utensil in this.allUtensils)
            {
                this.Utensils.Add(utensil);
            }
        }

    }
}
