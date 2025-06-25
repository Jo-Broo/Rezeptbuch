using RezeptSafe.Model;
using SQLite;

namespace RezeptSafe.Interfaces
{
    public interface IRezeptService
    {
        SQLiteAsyncConnection GetConnection(string databaseFile = "");
        Task<int> InitializeDataBase();
        Task CloseConnection();

        // Rezepte
        Task<List<Recipe>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeAsync(int recipeID);
        Task<int> AddRecipeAsync(Recipe recipe);
        Task<int> UpdateRecipeAsync(Recipe recipe);
        Task<int> DeleteRecipeAsync(int recipeID);
        Task<int> GetLastRecipeIDAsync();
        Task<int> AddExternalRecipeAsync(Recipe recipe);

        // Zutaten
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<int> AddIngredientAsync(Ingredient ingredient);
        Task<int> DeleteIngredientAsync(int ingredientID);
        Task<List<Ingredient>> GetIngredientsForRecipeAsync(int recipeID);
        Task<int> AddIngredientToRecipeAsync(int recipeID, Ingredient ingredient);
        Task<int> RemoveIngredientFromRecipeAsync(int recipeID, int ingredientID);
        Task<int> RemoveAllIngredientsFromRecipeAsync(int recipeID);
        Task<Ingredient?> IngredientPresentInDatabase(Ingredient ingredient);
        Task<int> GetLastIngredientIDAsync();
 
        // Utensilien
        Task<List<Utensil>> GetAllUtensilsAsync();
        Task<int> AddUtensilAsync(Utensil utensil);
        Task<int> DeleteUtensilAsync(int utensilID);
        Task<List<Utensil>> GetUtensilsForRecipeAsync(int recipeID);
        Task<int> AddUtensilToRecipeAsync(int recipeID, Utensil utensil);
        Task<int> RemoveUtensilFromRecipeAsync(int recipeID, int utensilID);
        Task<int> RemoveAllUtensilsFromRecipeAsync(int recipeID);
        Task<Utensil?> UtensilPresentInDatabase(Utensil utensil);
        Task<int> GetLastUtensilIDAsync();
    }

    public static class DBConstants
    {
        public static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "Rezepte.db");
    }
}
