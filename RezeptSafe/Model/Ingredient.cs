using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Model
{
    public class Ingredient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    // Beziehungstabelle: Recipe ↔ Ingredient
    public class RecipeIngredient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    // DTO-Class
    public class IngredientWithAmount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"{Amount}{Unit} {Name}";
        }
    }

}
