using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class SettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool showAdvancedOptions;

        [ObservableProperty]
        string buttonText;

        IRezeptService rezeptService;

        public SettingsViewModel(IRezeptService rezeptService)
        {
            this.rezeptService = rezeptService;
            this.UpdateButtonText();
        }

        [RelayCommand]
        async Task OnToggleThemeClicked()
        {
            App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;

            Preferences.Set("AppTheme", App.Current.UserAppTheme.ToString());

            this.UpdateButtonText();
        }

        void UpdateButtonText()
        {
            this.ButtonText = App.Current.UserAppTheme == AppTheme.Dark
                    ? "Wechsle zu Hellmodus"
                    : "Wechsle zu Dunkelmodus";
        }

        [RelayCommand]
        async Task ImportDataBaseFromFile()
        {
            await Toast.Make("Importieren Fehlgeschlagen", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
        }

        [RelayCommand]
        async Task ResetDataBase()
        {
            await Toast.Make("Zurücksetzen Fehlgeschlagen", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
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
                await Toast.Make($"Beim exportieren ihrer Datenbank ist folgender Fehler aufgetreten:[{ex}]", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
            }
        }
    }
}
