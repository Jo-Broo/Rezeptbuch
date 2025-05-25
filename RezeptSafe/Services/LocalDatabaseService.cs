using RezeptSafe.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Services
{
    public interface IRezeptService
    {
        // Utility
        /// <summary>
        /// Closes the current Connection and opens a new One while also Executing the Initialisation of all Tables
        /// </summary>
        Task ResetAndInitConnection();
        /// <summary>
        /// Closes the current Connection
        /// </summary>
        Task CloseConnection();

        // Rezepte
        Task<List<Recipe>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeAsync(int id);
        Task<int> AddRecipeAsync(Recipe recipe);
        Task<int> UpdateRecipeAsync(Recipe recipe);
        Task<int> DeleteRecipeAsync(int id);

        // Zutaten
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<int> AddIngredientAsync(Ingredient ingredient);
        Task<int> DeleteIngredientAsync(int id);

        Task<List<IngredientWithAmount>> GetIngredientsForRecipeAsync(int recipeId);
        Task AddIngredientToRecipeAsync(int recipeId, IngredientWithAmount ingredient);
        Task RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId);

        // Utensilien
        Task<List<Utensil>> GetAllUtensilsAsync();
        Task<int> AddUtensilAsync(Utensil utensil);
        Task<int> DeleteUtensilAsync(int id);

        Task<List<UtensilWithAmount>> GetUtensilsForRecipeAsync(int recipeId);
        Task AddUtensilToRecipeAsync(int recipeId, UtensilWithAmount utensil);
        Task RemoveUtensilFromRecipeAsync(int recipeId, int utensilId);
    }


    public class LocalDatabaseService : IRezeptService
    {
        #region Attribute
        private SQLiteAsyncConnection _connection;
        public static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "Rezepte.db");
        private bool DBFileFound = false;
        #endregion

        public LocalDatabaseService()
        {
            this.DBFileFound = File.Exists(DbPath);

            this.ResetAndInitConnection();
        }

        #region Methoden
        public async Task ResetAndInitConnection()
        {
            this._connection = new SQLiteAsyncConnection(DbPath);

            await this._connection.CreateTableAsync<Recipe>();
            await this._connection.CreateTableAsync<Ingredient>();
            await this._connection.CreateTableAsync<Utensil>();
            await this._connection.CreateTableAsync<RecipeIngredient>();
            await this._connection.CreateTableAsync<RecipeUtensil>();
        }

        public async Task CloseConnection()
        {
            if(this._connection != null)
            {
                await this._connection.CloseAsync();
            }
        }
        
        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await this._connection.Table<Recipe>().ToListAsync();
        }

        public async Task<Recipe> GetRecipeAsync(int id)
        {
            return await this._connection.Table<Recipe>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> AddRecipeAsync(Recipe recipe)
        {
            await this._connection.InsertAsync(recipe);

            foreach (IngredientWithAmount ingredient in recipe.Ingredients)
            {
                RecipeIngredient ri = new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = ingredient.Id
                };

                await this._connection.InsertAsync(ri);
            }

            foreach (UtensilWithAmount utensil in recipe.Utensils)
            {
                RecipeUtensil ru = new RecipeUtensil
                {
                    RecipeId = recipe.Id,
                    UtensilId = utensil.Id
                };

                await this._connection.InsertAsync(ru);
            }

            return recipe.Id;
        }

        public async Task<int> UpdateRecipeAsync(Recipe recipe)
        {
            return await this._connection.UpdateAsync(recipe);
        }

        public async Task<int> DeleteRecipeAsync(int id)
        {
            return await this._connection.DeleteAsync<Recipe>(id);
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            return await this._connection.Table<Ingredient>().ToListAsync();
        }

        public async Task<int> AddIngredientAsync(Ingredient ingredient)
        {
            return await this._connection.InsertAsync(ingredient);
        }

        public async Task<int> DeleteIngredientAsync(int id)
        {
            return await this._connection.DeleteAsync<Ingredient>(id);
        }

        public async Task<List<IngredientWithAmount>> GetIngredientsForRecipeAsync(int recipeId)
        {
            string query = @"
                SELECT i.*, ri.Amount, ri.Unit
                FROM Ingredient i
                INNER JOIN RecipeIngredient ri ON i.Id = ri.IngredientId
                WHERE ri.RecipeId = ?";

            return await _connection.QueryAsync<IngredientWithAmount>(query, recipeId);
        }

        public async Task AddIngredientToRecipeAsync(int recipeId, IngredientWithAmount ingredient)
        {
            var ri = new RecipeIngredient
            {
                RecipeId = recipeId,
                IngredientId = ingredient.Id,
                Amount = ingredient.Amount,
                Unit = ingredient.Unit
            };

            await this._connection.InsertAsync(ri);
        }

        public async Task RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId)
        {
            await _connection.ExecuteAsync("DELETE FROM RecipeIngredient WHERE RecipeId = ? AND IngredientId = ?", recipeId, ingredientId);
        }

        public async Task<List<Utensil>> GetAllUtensilsAsync()
        {
            return await this._connection.Table<Utensil>().ToListAsync();
        }

        public async Task<int> AddUtensilAsync(Utensil utensil)
        {
            return await this._connection.InsertAsync(utensil);
        }

        public async Task<int> DeleteUtensilAsync(int id)
        {
            return await this._connection.DeleteAsync<Utensil>(id);
        }

        public async Task<List<UtensilWithAmount>> GetUtensilsForRecipeAsync(int recipeId)
        {
            string query = @"
                SELECT u.*, ru.Amount
                FROM Utensil u
                INNER JOIN RecipeUtensil ru ON u.Id = ru.UtensilId
                WHERE ru.RecipeId = ?";

            return await _connection.QueryAsync<UtensilWithAmount>(query, recipeId);
        }

        public async Task AddUtensilToRecipeAsync(int recipeId, UtensilWithAmount utensil)
        {
            var ru = new RecipeUtensil
            {
                RecipeId = recipeId,
                UtensilId = utensil.Id,
                Amount = utensil.Amount
            };

            await this._connection.InsertAsync(ru);
        }

        public async Task RemoveUtensilFromRecipeAsync(int recipeId, int utensilId)
        {
            await _connection.ExecuteAsync("DELETE FROM RecipeUtensil WHERE RecipeId = ? AND UtensilId = ?", recipeId, utensilId);
        }
        #endregion
    }

}
