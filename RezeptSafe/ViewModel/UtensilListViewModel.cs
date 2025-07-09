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
    public partial class UtensilListViewModel : BaseViewModel
    {
        IRezeptService _rezeptService;

        [ObservableProperty]
        ObservableCollection<Utensil> _allUtensils = new ObservableCollection<Utensil>();
        [ObservableProperty]
        ObservableCollection<Utensil> _filteredUtensils = new ObservableCollection<Utensil>();
        [ObservableProperty]
        string utensilSearchtext = string.Empty;

        public UtensilListViewModel(IAlertService alertService, IRezeptService rezeptService) : base(alertService)
        {
            this.Title = "Utensilienliste verwalten";
            this._rezeptService = rezeptService;
        }
        partial void OnUtensilSearchtextChanged(string value)
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
                foreach (var ingredient in this.AllUtensils)
                {
                    this.FilteredUtensils.Add(ingredient);
                }
            }
            this.IsBusy = false;
        }
        public async Task QueryAllUtensilsAsync()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var utensils = await this._rezeptService.GetAllUtensilsAsync();

            if (this.AllUtensils.Count != 0)
            {
                this.AllUtensils.Clear();
            }

            foreach (var utensil in utensils)
            {
                this.AllUtensils?.Add(utensil);
            }

            this.OnUtensilSearchtextChanged("");

            this.IsBusy = false;
        }
        [RelayCommand]
        async Task RemoveUtensilAsync(Utensil utensil)
        {
            if (await this._alertService.ShowAlertWithChoiceAsync("Achtung", $"Wollen sie {utensil.NAME} wirklich löschen?", "Ja", "Nein"))
            {
                var recipes = await this._rezeptService.GetAllRecipesWithUtensilAsync(utensil);

                if (recipes is null)
                {
                    await this._alertService.ShowAlertAsync("Error", $"Beim abfragen der Rezepte ist ein Fehler aufgetreten");
                    return;
                }

                if (recipes.Count > 0)
                {
                    await this._alertService.ShowAlertAsync("Error", $"Das Utensil kann nicht gelöscht werden da sie in {recipes.Count} Rezepten verwendet wird");
                    return;
                }

                await this._rezeptService.DeleteUtensilAsync(utensil.ID);

                if (await this._rezeptService.GetUtensilByID(utensil.ID) is not null)
                {
                    await this._alertService.ShowAlertAsync("Info", "Beim entfernen der Zutat ist ein Fehler aufgetreten");
                    return;
                }

                await this.QueryAllUtensilsAsync();
                this.UtensilSearchtext = "";
                await this._alertService.ShowAlertAsync("Info", "Das Utensil wurde erfolgreich entfernt");
            }
        }
        [RelayCommand]
        async Task CreateNewUtensilAsync()
        {
            Utensil newUtensil = new Utensil();
            newUtensil.NAME = await this._alertService.ShowPromptAsync("Neues Utensil erstellen", "Name des Utensils");

            if (!string.IsNullOrWhiteSpace(newUtensil.NAME))
            {
                if (await this._rezeptService.AddUtensilAsync(newUtensil) == 1)
                {
                    await this._alertService.ShowAlertAsync("Info", $"Das Utensil {newUtensil.NAME} wurde erfolgreich erstellt");
                    await this.QueryAllUtensilsAsync();
                    return;
                }
                else
                {
                    await this._alertService.ShowAlertAsync("Fehler", $"Das Utensil {newUtensil.NAME} wurde nicht erfolgreich erstellt");
                    return;
                }
            }

            await this._alertService.ShowAlertAsync("Fehler", "Der Utensilienname darf nicht leer sein");
        }
    }
}
