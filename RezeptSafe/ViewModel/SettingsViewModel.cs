using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using RezeptSafe.Services;
using RezeptSafe.View;

namespace RezeptSafe.ViewModel
{
    public partial class SettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool showAdvancedOptions;

        [ObservableProperty]
        string buttonText;

        IRezeptService _rezeptService;
        IPreferenceService _preferenceService;

        public SettingsViewModel(IRezeptService rezeptService, IAlertService alertService, IPreferenceService preferenceService): base(alertService)
        {
            this._rezeptService = rezeptService;
            this._preferenceService = preferenceService;
            this.UpdateButtonText();

            this.Title = "Einstellungen";
        }

        [RelayCommand]
        void OnToggleThemeClicked()
        {
            App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;

            this._preferenceService.SetPreference(Enum.RezeptbuchPreferences.AppTheme, App.Current.UserAppTheme.ToString());

            this.UpdateButtonText();
        }

        void UpdateButtonText()
        {
            this.ButtonText = App.Current.UserAppTheme == AppTheme.Dark
                    ? "Wechsle zu Hellmodus"
                    : "Wechsle zu Dunkelmodus";
        }

        // TODO: Refractor und Methode zerteilen
        [RelayCommand]
        async Task ImportDataBaseFromFile()
        {
            var popup = new ProgressPopup();

            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Datenbank Datei auswählen"
                });

                if (file != null)
                {
                    if (await this._alertService.ShowAlertWithChoiceAsync("Datenbank Import", "Prozess auswählen", "Vorhandene Datenbank ersetzen", "Datenbanken kombinieren"))
                    {
                        // Vorhandene Datenbank ersetzen
                        await this._rezeptService.CloseConnection();

                        File.Delete(DBConstants.DbPath);

                        using var stream = await file.OpenReadAsync();
                        using var destStream = File.Create(DBConstants.DbPath);
                        await stream.CopyToAsync(destStream);

                        await this._alertService.ShowAlertAsync("Info", "Datenbank wurde erfolgreich ersetzt");
                    }
                    else
                    {
                        // Datenbanken kombinieren

                        // TODO: aus Listen entfernen die als Iterator dienen ist böse

                        IRezeptService tempRezeptService = new LocalDataBase();
                        tempRezeptService.GetConnection(file.FullPath);

                        // Zutaten abfragen und einfügen
                        int addedIngredients = 0;
                        List<Ingredient> newIngredients = await tempRezeptService.GetAllIngredientsAsync();
                        popup.SetIngredientCount(newIngredients.Count);

                        // Utensilien abfragen
                        int addedUtensils = 0;
                        List<Utensil> newUtensils = await tempRezeptService.GetAllUtensilsAsync();
                        popup.SetUtensilCount(newUtensils.Count);

                        // Rezepte abfragen
                        // Die ID der zutaten und utensilien sind nicht mehr zutreffend
                        int addedRecipes = 0;
                        List<Recipe> newRecipes = await tempRezeptService.GetAllRecipesAsync();
                        popup.SetRecipeCount(newRecipes.Count);

                        popup.SetMaxValue(newIngredients.Count + newUtensils.Count + newRecipes.Count);
                        popup.SetTitleText("Kombinierungsvorgang wird gestartet");
                        Shell.Current.ShowPopup(popup);

                        // Der einfachheit halber wird jedes Rezept neu angelegt
                        foreach(var recipe in newRecipes)
                        {
                            popup.SetTitleText("Kombinierungsvorgang Zutaten");
                            foreach (var ingredient in recipe.Ingredients)
                            {
                                Ingredient availableIngredient = await this._rezeptService.IngredientPresentInDatabase(ingredient);
                                if(availableIngredient.Id > 0)
                                {
                                    // Zutat ist bereits vorhanden und kann aus der Liste der neuen Zutaten entfernt werden
                                    newIngredients.Remove(ingredient);
                                    // Die ID der Zutat muss jetzt noch aktualisiert werden
                                    ingredient.Id = availableIngredient.Id;
                                }
                                else
                                {
                                    await this._rezeptService.AddIngredientAsync(ingredient);
                                    ingredient.Id = await this._rezeptService.GetLastIngredientIDAsync();
                                }
                                await popup.PerformStep();
                            }

                            popup.SetTitleText("Kombinierungsvorgang Utensilien");
                            foreach (var utensil in recipe.Utensils)
                            {
                                Utensil availableUtensil = await this._rezeptService.UtensilPresentInDatabase(utensil);
                                if (availableUtensil.ID > 0)
                                {
                                    // Utensil ist bereits vorhanden und kann aus der Liste der neuen Utensilien entfernt werden
                                    newUtensils.Remove(utensil);
                                    // Die ID des Utensils muss jetzt noch aktualisiert werden
                                    utensil.ID = availableUtensil.ID;
                                }
                                else
                                {
                                    await this._rezeptService.AddUtensilAsync(utensil);
                                    utensil.ID = await this._rezeptService.GetLastUtensilIDAsync();
                                }
                                await popup.PerformStep();
                            }
                            if(recipe.IsValidRecipe(out _))
                            {
                                await this._rezeptService.AddRecipeAsync(recipe);
                                addedRecipes++;
                            }
                            await popup.PerformStep();
                        }

                        // Jede Zutat die in keinem Rezept vorkam muss jetzt noch eingefügt werden
                        foreach (var ingredient in newIngredients)
                        {
                            Ingredient availableIngredient = await this._rezeptService.IngredientPresentInDatabase(ingredient);
                            if (availableIngredient.Id == -1)
                            {
                                // Zutat muss eingefügt werden
                                await this._rezeptService.AddIngredientAsync(ingredient);
                                addedIngredients++;
                            }
                            // Zutat ist bereits vorhanden und muss nicht mehr hinzugefügt werden
                            await popup.PerformStep();
                        }

                        // Jedes Utensil was in keinem Rezept benutzt wurde muss jetzt noch eingefügt werden
                        foreach (var utensil in newUtensils)
                        {
                            Utensil availableUtensil = await this._rezeptService.UtensilPresentInDatabase(utensil);
                            if (availableUtensil.ID == -1)
                            {
                                // Utensil ist nicht in der Datenbank vorhanden und muss hinzugefügt werden
                                await this._rezeptService.AddUtensilAsync(utensil);
                                addedUtensils++;
                            }
                            // Utensil ist bereits vorhanden und muss nicht mehr hinzugefügt werden
                            await popup.PerformStep();
                        }

                        await tempRezeptService.CloseConnection();

                        await this._alertService.ShowAlertAsync("Info", $"Ergebnis:\n{addedIngredients} neue Zutaten\n{addedUtensils} neue Utensilien\n{addedRecipes} neue Rezepte");
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                await this._alertService.ShowAlertAsync("Error", ex.Message);
#else
                await this._alertService.CreateToastMessageAsync(ex.Message).Show();
#endif
            }
            finally
            {
                popup.Close();
            }
        }

        [RelayCommand]
        async Task ResetDataBase()
        {
            try
            {
                if (await this._alertService.ShowAlertWithChoiceAsync("Datenbank Zurücksetzen", "Diese Aktion kann nicht rückgängig gemacht werden."))
                {
                    await this._rezeptService.CloseConnection();

                    File.Delete(DBConstants.DbPath);

                    bool prePopulate = await this._alertService.ShowAlertWithChoiceAsync("Info", "Soll die Datenbank mit Standardmäßigen Zutaten und Utensilien gefüllt werden?", "Ja", "Nein");

                    if (await this._rezeptService.InitializeDataBase(prePopulate) == -1)
                    {
                        throw new Exception("Beim neu erstellen der Datenbank ist ein Fehler aufgetreten");
                    }

                    await this._alertService.ShowAlertAsync("Info", "Datenbank erfolgreich zurückgesetz");
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                await this._alertService.ShowAlertAsync("Error", ex.Message);
#else
                await this._alertService.CreateToastMessageAsync(ex.Message).Show();
#endif
            }
        }

        [RelayCommand]
        async Task ExportDataBaseToFile()
        {
            try
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Rezepte.db");
                string exportPath = Path.Combine(FileSystem.Current.CacheDirectory, $"Rezepte_{DateTime.Now.ToString("ddMMyyyyHHmm")}.db");

                File.Copy(dbPath, exportPath, overwrite: true);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "SQLite DB exportieren",
                    File = new ShareFile(exportPath)
                });

                await Toast.Make("Ihre Datenbank konnte erfolgreich exportiert werden", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            }
            catch (Exception ex)
            {
#if DEBUG
                await this._alertService.ShowAlertAsync("Error", ex.Message);
#else
                await this._alertService.CreateToastMessageAsync(ex.Message).Show();
#endif
            }
        }

        [RelayCommand]
        async Task NavigateToIngredientList() { await this._alertService.ShowAlertAsync("Info", "Coming Soon"); }
        [RelayCommand]
        async Task NavigateToUtensilList() { await this._alertService.ShowAlertAsync("Info", "Coming Soon"); }
        [RelayCommand]
        async Task NavigateToUnitList() { await this._alertService.ShowAlertAsync("Info", "Coming Soon"); }
    }
}
