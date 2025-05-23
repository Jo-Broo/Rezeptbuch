using Kotlin;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Model
{
    public class Utensil
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    // Beziehungstabelle: Recipe ↔ Utensil
    public class RecipeUtensil
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public int UtensilId { get; set; }
        public int Amount { get; set; }
    }

    // DTO-Class
    public class UtensilWithAmount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Amount { get; set; }
        public override string ToString()
        {
            return $"{Amount}x {Name}";
        }
    }
}
