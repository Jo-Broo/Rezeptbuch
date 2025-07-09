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
    public partial class UnitListViewModel : BaseViewModel
    {
        IRezeptService _rezeptService;

        [ObservableProperty]
        ObservableCollection<Unit> _allUnits = new ObservableCollection<Unit>();
        [ObservableProperty]
        ObservableCollection<Unit> _filteredUnits = new ObservableCollection<Unit>();
        [ObservableProperty]
        string unitSearchtext = string.Empty;

        public UnitListViewModel(IAlertService alertService, IRezeptService rezeptService) : base(alertService)
        {
            this.Title = "Mengenangabenliste verwalten";
            this._rezeptService = rezeptService;
        }

        partial void OnUnitSearchtextChanged(string value)
        {
            this.IsBusy = true;
            this.FilteredUnits.Clear();

            if (!string.IsNullOrWhiteSpace(value))
            {
                Regex regex = new Regex($"^{Regex.Escape(value)}.*$", RegexOptions.IgnoreCase);

                this.FilteredUnits = new ObservableCollection<Unit>(this.AllUnits.Where(i => regex.IsMatch(i.UNIT)));
            }
            else
            {
                foreach (var ingredient in this.AllUnits)
                {
                    this.FilteredUnits.Add(ingredient);
                }
            }
            this.IsBusy = false;
        }
        public async Task QueryAllUnitsAsync()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var units = await this._rezeptService.GetAllUnitsAsync();

            if (this.AllUnits.Count != 0)
            {
                this.AllUnits.Clear();
            }

            foreach (var unit in units)
            {
                this.AllUnits?.Add(unit);
            }

            this.OnUnitSearchtextChanged("");

            this.IsBusy = false;
        }
        [RelayCommand]
        async Task RemoveUnitAsync(Unit unit)
        {
            if (await this._alertService.ShowAlertWithChoiceAsync("Achtung", $"Wollen sie {unit.UNIT} wirklich löschen?", "Ja", "Nein"))
            {
                var recipes = await this._rezeptService.GetAllRecipesWithUnitAsync(unit);

                if (recipes is null)
                {
                    await this._alertService.ShowAlertAsync("Error", $"Beim abfragen der Rezepte ist ein Fehler aufgetreten");
                    return;
                }

                if (recipes.Count > 0)
                {
                    await this._alertService.ShowAlertAsync("Error", $"Die Einheit kann nicht gelöscht werden da sie in {recipes.Count} Rezepten verwendet wird");
                    return;
                }

                await this._rezeptService.DeleteUnitAsync(unit.ID);

                if (await this._rezeptService.GetUnitByIDAsync(unit.ID) is not null)
                {
                    await this._alertService.ShowAlertAsync("Info", "Beim entfernen der Zutat ist ein Fehler aufgetreten");
                    return;
                }

                await this.QueryAllUnitsAsync();
                this.UnitSearchtext = "";
                await this._alertService.ShowAlertAsync("Info", "Die Einheit wurde erfolgreich entfernt");
            }
        }
        [RelayCommand]
        async Task CreateNewUnitAsync()
        {
            Unit newUnit = new Unit();
            newUnit.UNIT = await this._alertService.ShowPromptAsync("Neue Einheit erstellen", "Name der Einheit");

            if (!string.IsNullOrWhiteSpace(newUnit.UNIT))
            {
                if (await this._rezeptService.AddUnitAsync(newUnit) == 1)
                {
                    await this._alertService.ShowAlertAsync("Info", $"Die Einheit {newUnit.UNIT} wurde erfolgreich erstellt");
                    await this.QueryAllUnitsAsync();
                    return;
                }
                else
                {
                    await this._alertService.ShowAlertAsync("Fehler", $"Die Einheit {newUnit.UNIT} wurde nicht erfolgreich erstellt");
                    return;
                }
            }

            await this._alertService.ShowAlertAsync("Fehler", "Der Einheitenname darf nicht leer sein");
        }
    }
}
