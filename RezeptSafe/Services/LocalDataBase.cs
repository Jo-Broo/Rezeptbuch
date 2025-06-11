using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Services
{
    public class LocalDataBase : IRezeptService
    {
        #region Attribute
        private SQLiteAsyncConnection _connection;
        public bool DBFileFound { get; private set; }
        #endregion

        public LocalDataBase() 
        {
            this.DBFileFound = File.Exists(DBConstants.DbPath);
            if (!this.DBFileFound)
            {
                Task.Run(async () => {await this.InitializeDataBase();});
            }
        }

        public SQLiteAsyncConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SQLiteAsyncConnection(DBConstants.DbPath);
            }

            return _connection;
        }
        public async Task CloseConnection()
        {
            await this._connection.CloseAsync();
        }
        public async Task<int> InitializeDataBase()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"CREATE TABLE IF NOT EXISTS RECIPE (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    TITLE VARCHAR(100),
                                                    DESCRIPTION VARCHAR,
                                                    INSTRUCTIONS VARCHAR,
                                                    CREATEDAT BIGINT,
                                                    TIME INTEGER,
                                                    USERNAME VARCHAR,
                                                    IMAGEPATH VARCHAR -- Pfad zum Bild
                                                );";
                await conn.ExecuteAsync(sql);

                sql = @"CREATE TABLE IF NOT EXISTS INGREDIENT (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    NAME VARCHAR(100),
                                                    DESCRIPTION VARCHAR
                                                );";
                await conn.ExecuteAsync(sql);

                sql = @"CREATE TABLE IF NOT EXISTS UTENSIL (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    NAME VARCHAR(100),
                                                    DESCRIPTION VARCHAR
                                                );";
                await conn.ExecuteAsync(sql);

                sql = @"CREATE TABLE IF NOT EXISTS RECIPEINGREDIENT (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    RECIPEID INTEGER,
                                                    INGREDIENTID INTEGER,
                                                    AMOUNT FLOAT,
                                                    UNIT VARCHAR
                                                );";
                await conn.ExecuteAsync(sql);

                sql = @"CREATE TABLE IF NOT EXISTS RECIPEUTENSIL (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    RECIPEID INTEGER,
                                                    UTENSILID INTEGER,
                                                    AMOUNT INTEGER
                                                );";
                await conn.ExecuteAsync(sql);

                sql = @"INSERT INTO Utensil (name) VALUES
                                                ('Schneidebrett'),
                                                ('Kochmesser'),
                                                ('Gemüsemesser'),
                                                ('Brotmesser'),
                                                ('Schälmesser'),
                                                ('Küchenschere'),
                                                ('Kochlöffel'),
                                                ('Schöpfkelle'),
                                                ('Pfannenwender'),
                                                ('Teigschaber'),
                                                ('Schneebesen'),
                                                ('Sieb'),
                                                ('Salatschleuder'),
                                                ('Sparschäler'),
                                                ('Reibe'),
                                                ('Knoblauchpresse'),
                                                ('Zitruspresse'),
                                                ('Messbecher'),
                                                ('Messlöffel'),
                                                ('Küchenwaage'),
                                                ('Thermometer'),
                                                ('Backpinsel'),
                                                ('Backform'),
                                                ('Springform'),
                                                ('Kastenform'),
                                                ('Muffinblech'),
                                                ('Backblech'),
                                                ('Backpapier'),
                                                ('Rührschüssel'),
                                                ('Mixschüssel'),
                                                ('Teigrolle'),
                                                ('Nudelholz'),
                                                ('Küchenmaschine'),
                                                ('Handmixer'),
                                                ('Stabmixer'),
                                                ('Standmixer'),
                                                ('Zerkleinerer'),
                                                ('Küchenreibe'),
                                                ('Dosenöffner'),
                                                ('Flaschenöffner'),
                                                ('Korkenzieher'),
                                                ('Trichter'),
                                                ('Küchensieb'),
                                                ('Spätzlehobel'),
                                                ('Eierschneider'),
                                                ('Eieruhr'),
                                                ('Eisportionierer'),
                                                ('Pizzaroller'),
                                                ('Kochzange'),
                                                ('Grillzange'),
                                                ('Schmorpfanne'),
                                                ('Bratpfanne'),
                                                ('Kochtopf'),
                                                ('Schnellkochtopf'),
                                                ('Dampfgarer'),
                                                ('Wok'),
                                                ('Römertopf'),
                                                ('Auflaufform'),
                                                ('Kasserolle'),
                                                ('Milchtopf'),
                                                ('Wasserkocher'),
                                                ('Kaffeemaschine'),
                                                ('Espressokocher'),
                                                ('Teekanne'),
                                                ('Thermoskanne'),
                                                ('Küchenthermometer'),
                                                ('Herd'),
                                                ('Backofen'),
                                                ('Mikrowelle'),
                                                ('Toaster'),
                                                ('Mixeraufsatz'),
                                                ('Eismaschine'),
                                                ('Brotschneidemaschine'),
                                                ('Küchentimer'),
                                                ('Pfannenuntersetzer'),
                                                ('Topflappen'),
                                                ('Ofenhandschuhe'),
                                                ('Küchentuch'),
                                                ('Frischhaltefolie'),
                                                ('Alufolie'),
                                                ('Gefrierbeutel'),
                                                ('Vakuumierer'),
                                                ('Aufbewahrungsbox'),
                                                ('Gewürzregal'),
                                                ('Ölspender'),
                                                ('Essigspender'),
                                                ('Seiher'),
                                                ('Tortenheber'),
                                                ('Küchenbrenner'),
                                                ('Silikonmatte'),
                                                ('Pinsel'),
                                                ('Spatel'),
                                                ('Schüsselset'),
                                                ('Abtropfgestell'),
                                                ('Küchenrolle'),
                                                ('Küchensieb fein'),
                                                ('Passiersieb'),
                                                ('Mörser und Stößel'),
                                                ('Reiskocher'),
                                                ('Sous-Vide-Gerät'),
                                                ('Induktionsplatte'),
                                                ('Sandwichmaker'),
                                                ('Waffeleisen');";
                await conn.ExecuteAsync(sql);

                sql = @"INSERT INTO Ingredient (name) VALUES
                                                ('Salz'),
                                                ('Zucker'),
                                                ('Wasser'),
                                                ('Mehl'),
                                                ('Butter'),
                                                ('Eier'),
                                                ('Milch'),
                                                ('Pfeffer'),
                                                ('Knoblauch'),
                                                ('Zwiebeln'),
                                                ('Olivenöl'),
                                                ('Tomaten'),
                                                ('Kartoffeln'),
                                                ('Hefe'),
                                                ('Backpulver'),
                                                ('Essig'),
                                                ('Zitronensaft'),
                                                ('Petersilie'),
                                                ('Sahne'),
                                                ('Käse'),
                                                ('Paprika'),
                                                ('Karotten'),
                                                ('Sellerie'),
                                                ('Lauch'),
                                                ('Chili'),
                                                ('Basilikum'),
                                                ('Oregano'),
                                                ('Thymian'),
                                                ('Rosmarin'),
                                                ('Ingwer'),
                                                ('Sojasauce'),
                                                ('Honig'),
                                                ('Senf'),
                                                ('Koriander'),
                                                ('Spinat'),
                                                ('Rinderhackfleisch'),
                                                ('Hähnchenbrust'),
                                                ('Fisch'),
                                                ('Linsen'),
                                                ('Reis'),
                                                ('Nudeln'),
                                                ('Mais'),
                                                ('Erbsen'),
                                                ('Brokkoli'),
                                                ('Blumenkohl'),
                                                ('Zucchini'),
                                                ('Aubergine'),
                                                ('Gurke'),
                                                ('Apfel'),
                                                ('Banane'),
                                                ('Orange'),
                                                ('Beeren'),
                                                ('Vanille'),
                                                ('Kakao'),
                                                ('Schokolade'),
                                                ('Mandel'),
                                                ('Haselnuss'),
                                                ('Walnuss'),
                                                ('Cashewkerne'),
                                                ('Kokosnuss'),
                                                ('Kokosmilch'),
                                                ('Joghurt'),
                                                ('Quark'),
                                                ('Crème fraîche'),
                                                ('Schmand'),
                                                ('Speisestärke'),
                                                ('Gelatine'),
                                                ('Agar-Agar'),
                                                ('Currypulver'),
                                                ('Kreuzkümmel'),
                                                ('Muskatnuss'),
                                                ('Nelken'),
                                                ('Zimt'),
                                                ('Lorbeerblatt'),
                                                ('Wacholderbeeren'),
                                                ('Bohnen'),
                                                ('Kichererbsen'),
                                                ('Tofu'),
                                                ('Tempeh'),
                                                ('Speck'),
                                                ('Schinken'),
                                                ('Wurst'),
                                                ('Rindfleisch'),
                                                ('Schweinefleisch'),
                                                ('Lammfleisch'),
                                                ('Wild'),
                                                ('Eiweiß'),
                                                ('Eigelb'),
                                                ('Rapsöl'),
                                                ('Sonnenblumenöl'),
                                                ('Sesamöl'),
                                                ('Weißwein'),
                                                ('Rotwein'),
                                                ('Brühe'),
                                                ('Fond'),
                                                ('Paniermehl'),
                                                ('Haferflocken'),
                                                ('Couscous'),
                                                ('Bulgur'),
                                                ('Quinoa'),
                                                ('Chiasamen'),
                                                ('Leinsamen'),
                                                ('Mohn'),
                                                ('Rucola'),
                                                ('Feldsalat');";
                await conn.ExecuteAsync(sql);

                return 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT * FROM RECIPE";
                var result = await conn.QueryAsync<Recipe>(sql);

                if (result.Count == 0)
                    throw new Exception("Keine Rezepte gefunden.");

                foreach (Recipe recipe in result)
                {
                    recipe.Ingredients = await this.GetIngredientsForRecipeAsync(recipe.Id);
                    recipe.Utensils = await this.GetUtensilsForRecipeAsync(recipe.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Recipe>();
            }
        }

        public async Task<Recipe> GetRecipeAsync(int id)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT * FROM RECIPE WHERE ID = ?";
                var result = await conn.QueryAsync<Recipe>(sql);

                var recipe = result.FirstOrDefault();

                if (recipe == null)
                    throw new Exception($"Keine Rezepte mit der ID=[{id}] gefunden.");

                recipe.Ingredients = await this.GetIngredientsForRecipeAsync(id);
                recipe.Utensils = await this.GetUtensilsForRecipeAsync(id);

                return recipe;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new Recipe();
            }
        }

        public Task<int> AddRecipeAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRecipeAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRecipeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetLastRecipeIDAsync()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT MAX(ID) AS ID FROM RECIPE";
                var result = await conn.ExecuteScalarAsync<int?>(sql);

                if (result == null)
                    throw new Exception("Keine Rezepte vorhanden.");

                return result.Value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT * FROM INGREDIENT";

                return await conn.QueryAsync<Ingredient>(sql);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Ingredient>();
            }
        }

        public async Task<int> AddIngredientAsync(Ingredient ingredient)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"INSERT INTO INGREDIENT (NAME, DESCRIPTION ) VALUES (?, ?)";

                return await conn.ExecuteAsync(sql, ingredient.Name, ingredient.Description);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<int> DeleteIngredientAsync(int id)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"DELETE * FROM INGREDIENT WHERE ID = ?";

                return await conn.ExecuteAsync(sql, id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<List<Ingredient>> GetIngredientsForRecipeAsync(int recipeId)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT
                                    i.*,
                                    ri.UNIT,
                                    ri.AMOUNT
                                FROM
                                    RECIPEINGREDIENT ri
                                    LEFT JOIN INGREDIENT i ON i.ID = ri.INGREDIENTID
                                WHERE
                                    ri.RECIPEID = ?;
                                ";

                return await conn.QueryAsync<Ingredient>(sql, recipeId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Ingredient>();
            }
        }

        public async Task<int> AddIngredientToRecipeAsync(int recipeId, Ingredient ingredient)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"INSERT INTO RECIPEINGREDIENT(RECIPEID,INGREDIENTID,AMOUNT,UNIT) VALUES (?,?,?,?)";

                return await conn.ExecuteAsync(sql, recipeId,ingredient.Id,ingredient.Amount,ingredient.Unit);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<int> RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"DELETE * FROM RECIPEINGREDIENT WHERE RECIPEID = ? AND INGREDIENTID = ?";

                return await conn.ExecuteAsync(sql, recipeId, ingredientId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<List<Utensil>> GetAllUtensilsAsync()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT * FROM UTENSIL";

                return await conn.QueryAsync<Utensil>(sql);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Utensil>();
            }
        }

        public async Task<int> AddUtensilAsync(Utensil utensil)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"INSERT INTO UTENSIL (NAME, DESCRIPTION) VALUES (?, ?)";

                return await conn.ExecuteAsync(sql, utensil.Name, utensil.Description);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<int> DeleteUtensilAsync(int id)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"DELETE * FROM UTENSIL WHERE ID = ?";

                return await conn.ExecuteAsync(sql, id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<List<Utensil>> GetUtensilsForRecipeAsync(int recipeId)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT
                                    u.*,
                                    ru.AMOUNT
                                FROM
                                    RECIPEUTENSIL ru
                                    LEFT JOIN UTENSIL u ON u.ID = ru.UTENSILID
                                WHERE
                                    ru.RECIPEID = ?
                                    ";

                return await conn.QueryAsync<Utensil>(sql, recipeId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Utensil>();
            }
        }

        public async Task<int> AddUtensilToRecipeAsync(int recipeId, Utensil utensil)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"INSERT INTO RECIPEUTENSIL(RECIPEID,UTENSILID,AMOUNT) VALUES (?,?,?)";

                return await conn.ExecuteAsync(sql, recipeId, utensil.Id, utensil.Amount);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<int> RemoveUtensilFromRecipeAsync(int recipeId, int utensilId)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"DELETE * FROM RECIPEUTENSIL WHERE RECIPEID = ? AND UTENSILID = ?";

                return await conn.ExecuteAsync(sql, recipeId, utensilId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        
    }
}
