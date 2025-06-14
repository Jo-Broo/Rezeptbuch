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
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Time { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<Utensil> Utensils { get; set; } = new List<Utensil>();
        public string UtensilienLabel => $"{this.Utensils.Count} Utensil{((this.Utensils.Count > 1) ? "ien" : "")}";
        public string ZutatenLabel => $"{this.Utensils.Count} Zutat{((this.Utensils.Count > 1) ? "en" : "")}";
        public string ZeitLabel => $"ca. {this.Time} minute{((this.Time > 1) ? "n":"")}";
        public bool IsValidRecipe(out string error)
        {
            if (string.IsNullOrWhiteSpace(this.Title))
            {
                error = "Bitte gib einen Titel für das Rezept ein.";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.Description))
            {
                error = "Bitte gib eine Beschreibung für das Rezept ein.";
                return false;
            }
            else if (this.Ingredients.Count == 0)
            {
                error = "Füge mindestens eine Zutat hinzu.";
                return false;
            }
            else if (this.Ingredients.Any(x => x.Amount == 0))
            {
                error = "Überprüfe die Mengenangaben der Zutaten. Keine darf 0 sein.";
                return false;
            }
            else if (this.Utensils.Count == 0)
            {
                error = "Füge mindestens ein benötigtes Utensil hinzu.";
                return false;
            }
            else if (this.Utensils.Any(x => x.Amount == 0))
            {
                error = "Überprüfe die Mengenangaben der Utensilien. Keine darf 0 sein.";
                return false;
            }
            else if (this.Time <= 0)
            {
                error = "Gib eine sinnvolle Zubereitungszeit an.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}