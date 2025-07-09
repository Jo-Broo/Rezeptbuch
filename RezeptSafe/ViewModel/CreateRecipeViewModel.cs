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
using static RezeptSafe.Model.Recipe;

namespace RezeptSafe.ViewModel
{
    [QueryProperty("Recipe", "Recipe")]
    public partial class CreateRecipeViewModel : BaseViewModel
    {
        IRezeptService _rezeptService;
        IUserService _userService;
        IRezeptShareService _shareService;
        IPreferenceService _preferenceService;

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
        [NotifyPropertyChangedFor(nameof(IsMediaButtonVisible))]
        Step step;

        // Schritt 1: Titel und Beschreibung einfügen
        // Schritt 2: Zutaten auswählen
        // Schritt 3: Utensilien hinzufügen
        // Schritt 4: Zeitangabe machen und speichern
        const int maxSteps = (int)Step.Time;

        public bool IsBackVisible => ((int)this.Step > 1);

        public bool IsForwardVisible => ((int)this.Step < maxSteps);

        [ObservableProperty]
        string buttonText = string.Empty;

        bool _mediaPreference = false;

        public bool IsMediaButtonVisible => this.Step == Step.Title && this._mediaPreference && (this.Recipe.IMAGEPATH == "" && !this.Recipe.DeleteImageFlag);

        public bool IsImageDeleteVisible => this.Recipe.IMAGEPATH != "" && this.Recipe.DeleteImageFlag == false;

        public CreateRecipeViewModel(IRezeptService rezeptservice, IUserService userService, IAlertService alertService, IRezeptShareService shareService, IPreferenceService preferenceService) : base(alertService) 
        {
            this._rezeptService = rezeptservice;
            this._userService = userService;
            this._shareService = shareService;
            this._preferenceService = preferenceService;

            this.Recipe.USERNAME = this._userService.GetUsername();

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
            this.IsBusy = true;
            this.FilteredIngredients.Clear();

            if (!string.IsNullOrWhiteSpace(value))
            {
                Regex regex = new Regex($"^{Regex.Escape(value)}.*$", RegexOptions.IgnoreCase);

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

        partial void OnUtensilSearchTextChanged(string value)
        {
            this.IsBusy = true;
            this.FilteredUtensils.Clear();

            if (!string.IsNullOrWhiteSpace(value))
            {
                Regex regex = new Regex($"^{Regex.Escape(value)}.*$", RegexOptions.IgnoreCase);

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
            var ingredients = await this._rezeptService.GetAllIngredientsAsync();

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
            var utensils = await this._rezeptService.GetAllUtensilsAsync();

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
            var units = await this._rezeptService.GetAllUnitsAsync();

            if (units.Count == 0)
            {
                await this._alertService.ShowAlertAsync("Error", "Es konnten keine Eiheiten in der Datenbank gefunden werden");
                return;
            }

            if(this.AllUnits is null)
            {
                return;
            }

            if (this.AllUnits.Count != 0)
            {
                this.AllUnits.Clear();
            }

            foreach (var unit in units)
            {
                this.AllUnits.Add(unit);
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

                string? qrcodecontent = await this._shareService.WaitForScanAsync();

                if (!string.IsNullOrEmpty(qrcodecontent))
                {
                    string recipeJSON = this._shareService.DecompressBase64ToJson(qrcodecontent);

                    Recipe? newRecipe = JsonSerializer.Deserialize<Recipe>(recipeJSON);

                    Tuple<Step, string> error = new Tuple<Step, string>(Step.None, "");

                    if (newRecipe != null && newRecipe.IsValidRecipe(out error))
                    {
                        if (await this._alertService.ShowAlertWithChoiceAsync($"Rezept {newRecipe.TITLE} erkannt", "Wollen sie das Rezept übernehmen", "Ja", "Nein"))
                        {
                            await this._rezeptService.AddExternalRecipeAsync(newRecipe);
                            await this._alertService.ShowAlertAsync("Rezept erfolgreich eingefügt", "");

                            // Wenn das Rezept hinzugefügt wurde dann geht es zurück auf die Startseite
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                    else
                    {
                        await this._alertService.ShowAlertAsync("Error", error.Item2);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await this._alertService.ShowAlertAsync("Error", "Beim scannen ist ein Fehler aufgetreten");
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
            if(this.Recipe.ID >= 0)
            {
                this.Recipe.Ingredients = this.AllIngredients.Where(x => x.IsSelected == true).ToList();

                this.Recipe.Utensils = this.AllUtensils.Where(x => x.IsSelected == true).ToList();

                if (!this.Recipe.IsValidRecipe(out Tuple<Step, string> error))
                {
                    await this._alertService.ShowAlertAsync("Error", error.Item2);
                    this.Step = error.Item1;
                    this.Title = $"Schritt: {(int)this.Step}/{CreateRecipeViewModel.maxSteps}";
                    return;
                }

                if (this.Recipe.ID == 0)
                {
                    // Neues Rezept
                    if (await this._rezeptService.AddRecipeAsync(this.Recipe) == -1)
                    {
                        await this._alertService.ShowAlertAsync("Error", "Beim erstellen des Rezeptes ist ein Fehler aufgetreten");
                    }
                    else
                    {
                        if (this.Recipe.AttachImageFlag)
                        {
                            try
                            {
                                string sourcePath = this.Recipe.ImageSourcePath;

                                var appImageDir = Path.Combine(FileSystem.AppDataDirectory, "images");

                                if (!Directory.Exists(appImageDir))
                                    Directory.CreateDirectory(appImageDir);

                                File.Copy(sourcePath, this.Recipe.IMAGEPATH, overwrite: true);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                await this._alertService.ShowAlertAsync("Error", "Beim aktualisieren des Rezeptbildes ist ein Fehler aufgetreten");
                                return;
                            }
                        }

                        await Shell.Current.GoToAsync("..",true);
                        await Shell.Current.GoToAsync(nameof(ListRecipesPage), true);
                    }
                }
                else if(this.Recipe.ID > 0)
                {
                    // Rezept bearbeiten

                    if (this.Recipe.DeleteImageFlag)
                    {
                        try
                        {
                            if (File.Exists(this.Recipe.IMAGEPATH))
                            {
                                File.Delete(this.Recipe.IMAGEPATH);
                            }

                            this.Recipe.IMAGEPATH = "";
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                            await this._alertService.ShowAlertAsync("Error", "Beim aktualisieren des Rezeptes ist ein Fehler aufgetreten");
                            return;
                        }
                    }
                    else if (this.Recipe.AttachImageFlag)
                    {
                        try
                        {
                            string sourcePath = this.Recipe.ImageSourcePath;

                            var appImageDir = Path.Combine(FileSystem.AppDataDirectory, "images");

                            if (!Directory.Exists(appImageDir))
                                Directory.CreateDirectory(appImageDir);

                            File.Copy(sourcePath, this.Recipe.IMAGEPATH, overwrite: true);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                            await this._alertService.ShowAlertAsync("Error", "Beim aktualisieren des Rezeptbildes ist ein Fehler aufgetreten");
                            return;
                        }
                    }

                    if (await this._rezeptService.UpdateRecipeAsync(this.Recipe) == -1)
                    {
                        await this._alertService.ShowAlertAsync("Error", "Beim aktualisieren des Rezeptes ist ein Fehler aufgetreten");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("../..", true);
                    }
                }
            }
            else
            {
                await this._alertService.ShowAlertAsync("Error", "Ein unvorhergesehner Programmfehler ist aufgetreten, bitte dem Entwickler bescheid sagen");
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

                this.Step = Step.Title;
                this.Title = $"Schritt: 1/{CreateRecipeViewModel.maxSteps}";
                this.ButtonText = "Rezept speichern";

                string mediaPreference = this._preferenceService.GetPreference(Enum.RezeptbuchPreferences.MediaPermission);
                this._mediaPreference = string.IsNullOrWhiteSpace(mediaPreference) || mediaPreference.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                OnPropertyChanged(nameof(IsMediaButtonVisible));
                OnPropertyChanged(nameof(IsImageDeleteVisible));

                await this.QueryAllIngredients();
                await this.QueryAllUtensils();
                await this.QueryAllUnits();

                if(this.Recipe.ID > 0)
                {
                    this.ButtonText = "Rezept aktualisieren";

                    // Zutaten auswählen
                    foreach (var ingredient in this.Recipe.Ingredients)
                    {
                        var tempIngredient = this.AllIngredients.Where(x => x.NAME == ingredient.NAME).FirstOrDefault();
                        if(tempIngredient is not null)
                        {
                            tempIngredient.IsSelected = true;
                            tempIngredient.AMOUNT = ingredient.AMOUNT;

                            var tempUnit = this.AllUnits.Where(x => x.UNIT == ingredient.UNIT).FirstOrDefault();
                            if (tempUnit is not null)
                            {
                                tempIngredient.SelectedUnit = tempUnit;
                            }
                            else
                            {
                                throw new Exception($"Es konnte keine passende Einheit zur Zutat [{ingredient.NAME}] gefunden werden");
                            }
                        }
                    }

                    // Utensilien auswählen
                    foreach (var utensil in this.Recipe.Utensils)
                    {
                        var tempUtensil = this.AllUtensils.Where(x => x.NAME == utensil.NAME).FirstOrDefault();
                        if(tempUtensil is not null)
                        {
                            tempUtensil.IsSelected = true;
                        }
                    }

                    this.OnIngredientSearchTextChanged("");
                    this.OnUtensilSearchTextChanged("");
                }
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
            this.Title = $"Schritt: {(int)this.Step}/{CreateRecipeViewModel.maxSteps}";
        }
        
        [RelayCommand]
        void StepBackAsync() 
        {
            this.Step--;
            this.Title = $"Schritt: {(int)this.Step}/{CreateRecipeViewModel.maxSteps}";
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
                    await this.QueryAllIngredients();
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

        [RelayCommand]
        async Task CreateNewUtensilAsync()
        {
            Utensil newUtensil = new Utensil();
            newUtensil.NAME = await this._alertService.ShowPromptAsync("Neues Utensil erstellen", "Name der Utensilie");

            if (!string.IsNullOrWhiteSpace(newUtensil.NAME))
            {
                if (await this._rezeptService.AddUtensilAsync(newUtensil) == 1)
                {
                    await this._alertService.ShowAlertAsync("Info", $"Die Utensilie {newUtensil.NAME} wurde erfolgreich erstellt");
                    await this.QueryAllUtensils();
                    return;
                }
                else
                {
                    await this._alertService.ShowAlertAsync("Fehler", $"Die Utensilie {newUtensil.NAME} wurde nicht erfolgreich erstellt");
                    return;
                }
            }

            await this._alertService.ShowAlertAsync("Fehler", "Der Utensilienname darf nicht leer sein");
        }

        [RelayCommand]
        async Task ChooseRecipeImageAsync()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Media>();
            
            string mediaPreference = this._preferenceService.GetPreference(Enum.RezeptbuchPreferences.MediaPermission);

            bool allowMedia;
            
            if (string.IsNullOrWhiteSpace(mediaPreference))
            {
                // Keine Medienpräferenz gesetzt
                // deswegen setzt ich das default auf true damit ich die permissions abfragen kann
                allowMedia = await this._alertService.ShowAlertWithChoiceAsync("Info", "Darf die App Bilder aus deiner Galerie durchsuchen ?", "Ja", "Nein");

                if (allowMedia)
                {
                    this._preferenceService.SetPreference(Enum.RezeptbuchPreferences.MediaPermission, "true");
                }
                else
                {
                    await this._alertService.ShowAlertAsync("Info", "Die App wird dich nicht mehr nach Zugriff auf deine Bilder fragen");
                    return;
                }
            }
            else
            {
                mediaPreference = mediaPreference.Trim();
                allowMedia = mediaPreference.Equals("true", StringComparison.OrdinalIgnoreCase);
            }

            if (allowMedia)
            {
                if (permissionStatus != PermissionStatus.Granted)
                {
                    permissionStatus = await Permissions.RequestAsync<Permissions.Media>();

                    if (permissionStatus != PermissionStatus.Granted)
                    {
                        this._preferenceService.SetPreference(Enum.RezeptbuchPreferences.MediaPermission, "false");
                        await this._alertService.ShowAlertAsync("Info", "Sie können ab jetzt keine Bilder mehr zu ihren Rezepten speichern. Wenn sie doch wieder welche speichern wollen können sie das in den Einstellungen anpassen");
                        return;
                    }
                }

                try
                {
                    var imagefile = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                    {
                        Title = "Wählen sie ein Bild",
                    });

                    if(imagefile is not null)
                    {                        
                        string fileName = Guid.NewGuid().ToString() + ".jpg";
                        var appImageDir = Path.Combine(FileSystem.AppDataDirectory, "images");
                        var destPath = Path.Combine(appImageDir, fileName);

                        this.Recipe.ImageSourcePath = imagefile.FullPath;
                        this.Recipe.IMAGEPATH = destPath;
                        this.Recipe.AttachImageFlag = true;
                        this.Recipe.DeleteImageFlag = false;

                        await this._alertService.ShowAlertAsync("Info", "Die Änderung wird erst mit der aktualisierung angewendet");

                        OnPropertyChanged(nameof(IsMediaButtonVisible));
                        OnPropertyChanged(nameof(IsImageDeleteVisible));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    await this._alertService.ShowAlertAsync("Error", "Beim auswählen des Bildes ist ein Fehler aufgetreten");
                }
            }
        }

        [RelayCommand]
        async Task RemoveRecipeImageAsync()
        {
            if (await this._alertService.ShowAlertWithChoiceAsync("Warnung", "Wollen sie das Bild für dieses Rezept entfernen? Diese Aktion kann nicht rückgängig gemacht werden.", "Ja", "Nein"))
            {
                try
                {
                    this.Recipe.DeleteImageFlag = true;
                    this.Recipe.AttachImageFlag = false;

                    await this._alertService.ShowAlertAsync("Info", "Die Änderung wird erst mit der aktualisierung angewendet");

                    OnPropertyChanged(nameof(IsMediaButtonVisible));
                    OnPropertyChanged(nameof(IsImageDeleteVisible));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    await this._alertService.ShowAlertAsync("Error", "Beim entfernen des Bildes ist ein Fehler aufgetreten");
                }
            }
        }
    }
}