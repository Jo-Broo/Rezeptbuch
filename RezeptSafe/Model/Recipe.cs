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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Instructions { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Time { get; set; }

        public string Username { get; set; } = string.Empty;

        [Ignore]
        public List<Utensil> Utensils { get; set; } = new List<Utensil>();
        
        [Ignore]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        [Ignore]
        public string UtensilienLabel => $"{this.Utensils.Count} Utensil{((this.Utensils.Count > 1) ? "ien" : "") }";

        [Ignore]
        public string ZutatenLabel => $"{this.Utensils.Count} Zutat{((this.Utensils.Count > 1) ? "en" : "")}";

        [Ignore]
        public string ZeitLabel => $"ca. {this.Time} minuten";
    }
}