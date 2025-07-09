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

                        IRezeptService external = new LocalDataBase(file.FullPath);

                        Shell.Current.ShowPopup(popup);

                        if(await this._rezeptService.MergeDatabases(external, popup))
                        {
                            await external.CloseConnection();

                            await this._alertService.ShowAlertAsync("Info", $"Der Kombinierungsvorgang wurde erfolgreich abgeschlossen");
                        }
                        else
                        {
                            throw new Exception("Beim kombinieren der Datenbanken ist ein Fehler aufgetreten");
                        }
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
        async Task NavigateToIngredientList() { await Shell.Current.GoToAsync(nameof(IngredientListPage), true); }
        [RelayCommand]
        async Task NavigateToUtensilList() { await Shell.Current.GoToAsync(nameof(UtensilListPage), true); }
        [RelayCommand]
        async Task NavigateToUnitList() { await Shell.Current.GoToAsync(nameof(UnitListPage), true); }
    }
}
