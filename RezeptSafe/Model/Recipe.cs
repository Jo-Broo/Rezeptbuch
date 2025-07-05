using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Model
{
    public class Recipe
    {
        public int ID { get; set; }
        public string TITLE { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;
        public DateTime CREATEDAT { get; set; } = DateTime.Now;
        public int TIME { get; set; }
        public string USERNAME { get; set; } = string.Empty;
        public string IMAGEPATH { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<Utensil> Utensils { get; set; } = new List<Utensil>();
        public string UtensilienLabel => $"{this.Utensils.Count} Utensil{((this.Utensils.Count != 1) ? "ien" : "")}";
        public string ZutatenLabel => $"{this.Ingredients.Count} Zutat{((this.Ingredients.Count != 1) ? "en" : "")}";
        public string ZeitLabel => $"ca. {this.TIME} minute{((this.TIME > 1) ? "n":"")}";
        public string UsernameLabel => $"Erstellt von: {this.USERNAME}";
        public bool IsValidRecipe(out Tuple<Step, string> error)
        {
            error = new Tuple<Step, string>(Step.None, "");

            if (string.IsNullOrWhiteSpace(this.TITLE))
            {
                error = new Tuple<Step, string>(Step.Title,"Bitte gib einen Titel für das Rezept ein.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.DESCRIPTION))
            {
                error = new Tuple<Step, string>(Step.Title,"Bitte gib eine Beschreibung für das Rezept ein.");
                return false;
            }

            if (this.Ingredients.Count == 0)
            {
                error = new Tuple<Step, string>(Step.Ingredients, "Füge mindestens eine Zutat hinzu.");
                return false;
            }

            if (this.Ingredients.Any(x => x.AMOUNT <= 0))
            {
                error = new Tuple<Step, string>(Step.Ingredients, "Jede Zutat muss eine Mengenangabe größer als 0 haben.");
                return false;
            }

            if (this.Ingredients.Any(x => x.SelectedUnit.ID == 0 || string.IsNullOrWhiteSpace(x.SelectedUnit.UNIT)))
            {
                error = new Tuple<Step, string>(Step.Ingredients, "Jede Zutat muss eine gültige Einheit besitzen.");
                return false;
            }

            if (this.Utensils.Any(x => x.AMOUNT <= 0))
            {
                error = new Tuple<Step, string>(Step.Utensils, "Jedes Utensil muss eine Mengenangabe größer als 0 haben.");
                return false;
            }

            if (this.TIME <= 0)
            {
                error = new Tuple<Step, string>(Step.Time, "Bitte gib eine sinnvolle Zubereitungszeit in Minuten an.");
                return false;
            }

            return true;
        }

        public enum Step
        {
            None = 0,
            Title = 1,
            Ingredients = 2,
            Utensils = 3,
            Time = 4,
        }
    }
}