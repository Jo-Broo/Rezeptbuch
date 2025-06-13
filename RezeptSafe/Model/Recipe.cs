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
        public string ZeitLabel => $"ca. {this.Time} minuten";
    }
}