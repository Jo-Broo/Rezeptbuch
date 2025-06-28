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
        public int Id { get; set; }
        public string TITLE { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;
        public DateTime CREATEDAT { get; set; } = DateTime.Now;
        public int TIME { get; set; }
        public string USERNAME { get; set; } = string.Empty;
        public string IMAGEPATH { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<Utensil> Utensils { get; set; } = new List<Utensil>();
        public string UtensilienLabel => $"{this.Utensils.Count} Utensil{((this.Utensils.Count > 1) ? "ien" : "")}";
        public string ZutatenLabel => $"{this.Utensils.Count} Zutat{((this.Utensils.Count > 1) ? "en" : "")}";
        public string ZeitLabel => $"ca. {this.TIME} minute{((this.TIME > 1) ? "n":"")}";
        public string UsernameLabel => $"Erstellt von: {this.USERNAME}";
        public bool IsValidRecipe(out string error)
        {
            if (string.IsNullOrWhiteSpace(this.TITLE))
            {
                error = "Bitte gib einen Titel für das Rezept ein.";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.DESCRIPTION))
            {
                error = "Bitte gib eine Beschreibung für das Rezept ein.";
                return false;
            }
            else if (this.Ingredients.Count == 0)
            {
                error = "Füge mindestens eine Zutat hinzu.";
                return false;
            }
            else if (this.Ingredients.Any(x => x.AMOUNT == 0))
            {
                error = "Überprüfe die Mengenangaben der Zutaten. Keine darf 0 sein.";
                return false;
            }
            else if (this.Ingredients.Any(x => x.UNITID == 0) || this.Ingredients.Any(x => x.UNIT == string.Empty))
            {
                error = "Überprüfe die Mengenangaben bei den Zutaten";
            }
            else if (this.Utensils.Count == 0)
            {
                error = "Füge mindestens ein benötigtes Utensil hinzu.";
                return false;
            }
            else if (this.Utensils.Any(x => x.AMOUNT == 0))
            {
                error = "Überprüfe die Mengenangaben der Utensilien. Keine darf 0 sein.";
                return false;
            }
            else if (this.TIME <= 0)
            {
                error = "Gib eine sinnvolle Zubereitungszeit an.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}