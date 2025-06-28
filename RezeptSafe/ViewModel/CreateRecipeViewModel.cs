using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using RezeptSafe.Services;
using RezeptSafe.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class CreateRecipeViewModel : BaseViewModel
    {
        IRezeptService rezeptService;
        IUserService userService;
        IRezeptShareService shareService;

        [ObservableProperty]
        bool isScanning = false;

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

        [ObservableProperty]
        ObservableCollection<Unit> allUnits;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsBackVisible))]
        [NotifyPropertyChangedFor(nameof(IsForwardVisible))]
        int step;

        // Schritt 1: Titel und Beschreibung einfügen
        // Schritt 2: Zutaten auswählen
        // Schritt 3: Utensilien hinzufügen
        // Schritt 4: Zeitangabe machen und speichern
        const int maxSteps = 4;

        public bool IsBackVisible => (this.Step > 1);

        public bool IsForwardVisible => (this.Step < maxSteps);

        public CreateRecipeViewModel(IRezeptService rezeptservice, IUserService userService, IAlertService alertService, IRezeptShareService shareService) : base(alertService) 
        {
            this.rezeptService = rezeptservice;
            this.userService = userService;
            this.shareService = shareService;

            this.Recipe.USERNAME = this.userService.GetUsername();

            this.AllIngredients = new ObservableCollection<Ingredient>();
            this.FilteredIngredients = new ObservableCollection<Ingredient>();
            this.AllUtensils = new ObservableCollection<Utensil>();
            this.FilteredUtensils = new ObservableCollection<Utensil>();
            this.AllUnits = new ObservableCollection<Unit>();
            this.IngredientSearchText = string.Empty;
            this.UtensilSearchText = string.Empty;

            OnPropertyChanged(nameof(IsBackVisible));
        }

        partial void OnIngredientSearchTextChanged(string value)
        {
            FilterIngredients(value);
        }
        partial void OnUtensilSearchTextChanged(string value)
        {
            FilterUtensils(value);
        }

        void FilterIngredients(string query)
        {
            this.IsBusy = true;
            this.FilteredIngredients.Clear();

            if (!string.IsNullOrWhiteSpace(query))
            {
                Regex regex = new Regex($"^{Regex.Escape(query)}.*$", RegexOptions.IgnoreCase);

                this.FilteredIngredients = new ObservableCollection<Ingredient>(this.AllIngredients.Where(i => regex.IsMatch(i.NAME)));
            }
            else
            {
                foreach (var ingredient in this.AllIngredients.Where(i => i.IsSelected))
                {
                    this.FilteredIngredients.Add(ingredient);
                }
            }
            
            this.IsBusy = false;
        }
        void FilterUtensils(string query)
        {
            this.IsBusy = true;
            this.FilteredUtensils.Clear();

            if (!string.IsNullOrWhiteSpace(query))
            {
                Regex regex = new Regex($"^{Regex.Escape(query)}.*$", RegexOptions.IgnoreCase);

                this.FilteredUtensils = new ObservableCollection<Utensil>(this.AllUtensils.Where(i => regex.IsMatch(i.NAME)));
            }
            else
            {
                foreach (var utensil in this.AllUtensils.Where(i => i.IsSelected))
                {
                    this.FilteredUtensils.Add(utensil);
                }
            }

            this.IsBusy = false;
        }

        public async Task QueryAllIngredients()
        {
            var ingredients = await this.rezeptService.GetAllIngredientsAsync();

            if (this.FilteredIngredients?.Count != 0)
            {
                this.FilteredIngredients?.Clear();
            }

            foreach (var ingredient in ingredients)
            {
                this.AllIngredients?.Add(ingredient);
            }
        }

        public async Task QueryAllUtensils()
        {
            var utensils = await this.rezeptService.GetAllUtensilsAsync();

            if(this.FilteredUtensils.Count != 0)
            {
                this.FilteredUtensils?.Clear();
            }

            foreach (var utensil in utensils)
            {
                this.AllUtensils?.Add(utensil);
            }
        }

        public async Task QueryAllUnits()
        {
            var units = await this.rezeptService.GetAllUnitsAsync();

            if(this.AllUnits?.Count != 0)
            {
                this.AllUnits?.Clear();
            }

            foreach (var unit in units)
            {
                this.AllUnits?.Add(unit);
            }

            if(this.AllUnits is null)
            {
                return;
            }

            foreach (var ingredient in this.AllIngredients)
            {
                ingredient.Units = this.AllUnits;
            }
        }

        [RelayCommand]
        async Task OnScanQRCodeClicked()
        {
            try
            {
                this.IsScanning = true;

                await Shell.Current.GoToAsync(nameof(QRCodeScanner), true);

                string? qrcodecontent = await this.shareService.WaitForScanAsync();

                if (!string.IsNullOrEmpty(qrcodecontent))
                {
                    string recipeJSON = this.shareService.DecompressBase64ToJson(qrcodecontent);

                    Recipe? newRecipe = JsonSerializer.Deserialize<Recipe>(recipeJSON);

                    string error = string.Empty;

                    if (newRecipe != null && newRecipe.IsValidRecipe(out error))
                    {
                        if (await this.alertService.ShowAlertWithChoiceAsync($"Rezept {newRecipe.TITLE} erkannt", "Wollen sie das Rezept übernehmen", "Ja", "Nein"))
                        {
                            await this.rezeptService.AddExternalRecipeAsync(newRecipe);
                            await this.alertService.ShowAlertAsync("Rezept erfolgreich eingefügt", "");

                            // Wenn das Rezept hinzugefügt wurde dann geht es zurück auf die Startseite
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                    else
                    {
                        await this.alertService.ShowAlertAsync("Error", error);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                this.IsScanning = false;
            }
        }

        partial void OnIsScanningChanged(bool oldValue, bool newValue)
        {
            this.ScanQRCodeClickedCommand.NotifyCanExecuteChanged();
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

            if(await this.rezeptService.AddRecipeAsync(this.Recipe) == -1)
            {
                await this.alertService.ShowAlertAsync("Error", "Beim erstellen des Rezeptes ist ein Fehler aufgetreten");
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        async Task InitializeAsync()
        {
            if (this.IsBusy)
            {
                return;
            }

            try
            {
                this.IsBusy = true;

                this.Step = 1;
                this.Title = $"Schritt: 1/{CreateRecipeViewModel.maxSteps}";

                await this.QueryAllIngredients();
                await this.QueryAllUtensils();
                await this.QueryAllUnits();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        [RelayCommand]
        void StepVorwardAsync() 
        {
            this.Step++;
            this.Title = $"Schritt: {this.Step}/{CreateRecipeViewModel.maxSteps}";
        }
        
        [RelayCommand]
        void StepBackAsync() 
        {
            this.Step--;
            this.Title = $"Schritt: {this.Step}/{CreateRecipeViewModel.maxSteps}";
        }
    }
}
