using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;

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
                Task.Run(async () => {await this.InitializeDataBase(true);});
            }
        }

        public SQLiteAsyncConnection GetConnection(string databaseFile = "")
        {
            if (this._connection == null)
            {
                if(databaseFile == "")
                {
                    this._connection = new SQLiteAsyncConnection(DBConstants.DbPath);
                }
                else
                {
                    this._connection = new SQLiteAsyncConnection(databaseFile);
                }
            }

            return this._connection;
        }
        public async Task CloseConnection()
        {
            if(this._connection is null)
            {
                return;
            }
            
            await this._connection.CloseAsync();
        }
        public async Task<int> InitializeDataBase(bool prePopulate)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"CREATE TABLE IF NOT EXISTS RECIPE (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    TITLE VARCHAR(100),
                                                    DESCRIPTION VARCHAR,
                                                    CREATEDAT DATETIME DEFAULT CURRENT_TIMESTAMP,
                                                    TIME INTEGER,
                                                    USERNAME VARCHAR,
                                                    IMAGEPATH VARCHAR
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
                                                    UNITID INTEGER
                                                );";
                await conn.ExecuteAsync(sql);

                sql = @"CREATE TABLE IF NOT EXISTS RECIPEUTENSIL (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    RECIPEID INTEGER,
                                                    UTENSILID INTEGER,
                                                    AMOUNT INTEGER
                                                );";
                await conn.ExecuteAsync(sql);

                sql = @"CREATE TABLE IF NOT EXISTS UNIT (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                                    UNIT VARCHAR);";

                await conn.ExecuteAsync(sql);

                if (prePopulate)
                {
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
                    
                }
                sql = @"INSERT INTO UNIT (UNIT) VALUES 
                            ('ml'),
                            ('l'),
                            ('TL'),
                            ('EL'),
                            ('Prise'),
                            ('Schuss'),
                            ('Spritzer'),
                            ('g'),
                            ('kg'),
                            ('Stück'),
                            ('Bund'),
                            ('Becher'),
                            ('Tasse'),
                            ('Dose'),
                            ('Päckchen'),
                            ('Würfel');
                            ";
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
                    recipe.Ingredients = await this.GetIngredientsForRecipeAsync(recipe.ID);
                    recipe.Utensils = await this.GetUtensilsForRecipeAsync(recipe.ID);
                }

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Recipe>();
            }
        }
        public async Task<Recipe?> GetRecipeAsync(int id)
        {
            Recipe? recipe = null;
            
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT * FROM RECIPE WHERE ID = ?";
                var result = await conn.QueryAsync<Recipe>(sql);

                recipe = result.FirstOrDefault();

                if (recipe == null)
                    throw new Exception($"Keine Rezepte mit der ID=[{id}] gefunden.");

                recipe.Ingredients = await this.GetIngredientsForRecipeAsync(id);
                recipe.Utensils = await this.GetUtensilsForRecipeAsync(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return recipe;
        }
        public async Task<int> AddRecipeAsync(Recipe recipe)
        {
            try
            {
                var conn = this.GetConnection();
                string sql = @"INSERT INTO RECIPE(TITLE,DESCRIPTION,TIME,USERNAME,IMAGEPATH) 
                               VALUES
                                (?,?,?,?,?)";
                var result = await conn.ExecuteAsync(sql,recipe.TITLE,recipe.DESCRIPTION,recipe.TIME,recipe.USERNAME,recipe.IMAGEPATH);

                if(result == 1)
                {
                    recipe.ID = await this.GetLastRecipeIDAsync();

                    var tasks = new List<Task>();

                    foreach (var ingredient in recipe.Ingredients)
                    {
                        if(await this.AddIngredientToRecipeAsync(recipe.ID, ingredient) == -1)
                        {
                            throw new Exception("Beim einfügen in die RecipeIngredient Tabelle ist ein Fehler aufgetreten");
                        }
                    }

                    foreach (var utensil in recipe.Utensils)
                    {
                        if(await this.AddUtensilToRecipeAsync(recipe.ID, utensil) == -1)
                        {
                            throw new Exception("Beim einfügen in die RecipeUtensil Tabelle ist ein Fehler aufgetreten");
                        }
                    }

                    return 1;
                }

                throw new Exception("Beim einfügen des Rezeptes ist ein Fehler aufgetreten");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }
        public async Task<int> UpdateRecipeAsync(Recipe recipe)
        {
            try
            {
                var conn = this.GetConnection();
                string sql = @"UPDATE RECIPE SET 
                                TITLE = ?,
                                DESCRIPTION = ?,
                                TIME = ?,
                                USERNAME = ?,
                                IMAGEPATH = ?
                               WHERE
                                ID = ?";
                var result = await conn.ExecuteAsync(sql, recipe.TITLE, recipe.DESCRIPTION, recipe.TIME, recipe.USERNAME, recipe.IMAGEPATH, recipe.ID);

                if (result == 1)
                {
                    // Zutaten und Utensilien werden einfach gelöscht und neu angelegt
                    // wie viele können das schon sein

                    await this.RemoveAllIngredientsFromRecipeAsync(recipe.ID);

                    await this.RemoveAllUtensilsFromRecipeAsync(recipe.ID);

                    var tasks = new List<Task>();

                    foreach (var ingredient in recipe.Ingredients)
                    {
                        tasks.Add(this.AddIngredientToRecipeAsync(recipe.ID, ingredient));
                    }

                    foreach (var utensil in recipe.Utensils)
                    {
                        tasks.Add(this.AddUtensilToRecipeAsync(recipe.ID, utensil));
                    }

                    await Task.WhenAll(tasks);

                    return 1;
                }

                throw new Exception("Beim einfügen des Rezeptes ist ein Fehler aufgetreten");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }
        public async Task<int> DeleteRecipeAsync(int recipeID)
        {
            try
            {
                var conn = this.GetConnection();
                string sql = @"DELETE FROM RECIPE WHERE ID = ?";

                var result = await conn.ExecuteAsync(sql, recipeID);
                if (result == 1)
                {
                    await this.RemoveAllIngredientsFromRecipeAsync(recipeID);

                    await this.RemoveAllUtensilsFromRecipeAsync(recipeID);
                }

                return 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
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
        public async Task<int> AddExternalRecipeAsync(Recipe recipe)
        {
            try
            {
                List<Ingredient> internalIngredients = new List<Ingredient>();
                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    if(string.IsNullOrWhiteSpace(ingredient.NAME) || ingredient.AMOUNT <= 0)
                    {
                        throw new InvalidOperationException("Non Valid Ingredient found");
                    }
                    
                    Ingredient? tmp = await this.IngredientPresentInDatabase(ingredient);
                    if (tmp is null)
                    {
                        // Zutat muss noch erstellt werden
                        await this.AddIngredientAsync(ingredient);
                        ingredient.Id = await this.GetLastIngredientIDAsync();
                        tmp = ingredient;
                    }
                    // Zutat ist bereits vorhanden
                    tmp.AMOUNT = ingredient.AMOUNT;
                    tmp.UNIT = ingredient.UNIT;
                    tmp.UNITID = ingredient.UNITID;
                    
                    internalIngredients.Add(tmp);
                }

                List<Utensil> internalUtensils = new List<Utensil>();
                foreach (Utensil utensil in recipe.Utensils)
                {
                    if (string.IsNullOrWhiteSpace(utensil.NAME) || utensil.AMOUNT <= 0)
                    {
                        throw new InvalidOperationException("Non Valid Utensil found");
                    }

                    Utensil? tmp = await this.UtensilPresentInDatabase(utensil);
                    if (tmp is null)
                    {
                        // Utensil muss noch erstellt werden
                        await this.AddUtensilAsync(utensil);
                        utensil.ID = await this.GetLastUtensilIDAsync();
                        tmp = utensil;
                    }
                    // Utensil ist bereits vorhanden
                    tmp.AMOUNT = utensil.AMOUNT;
                    internalUtensils.Add(tmp);
                }

                recipe.Ingredients = internalIngredients;
                recipe.Utensils = internalUtensils;

                return await this.AddRecipeAsync(recipe);
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

                return await conn.ExecuteAsync(sql, ingredient.NAME, ingredient.DESCRIPTION);
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
                                    u.ID AS UNITID,
                                    u.UNIT,
                                    ri.AMOUNT
                                FROM
                                    RECIPEINGREDIENT ri
                                    LEFT JOIN INGREDIENT i ON i.ID = ri.INGREDIENTID
                                    LEFT JOIN UNIT u ON u.ID = ri.UNITID 
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

                var unit = await this.UnitPresentInDatabase(ingredient.SelectedUnit);

                if(unit is null || unit.ID == 0 || string.IsNullOrEmpty(unit.UNIT))
                {
                    unit = await this.UnitPresentInDatabase(new Unit() { UNIT=ingredient.UNIT });

                    if(unit is null)
                    {
                        throw new Exception("Keine Einheit ausgewählt");

                    }
                }

                string sql = @"INSERT INTO RECIPEINGREDIENT(RECIPEID,INGREDIENTID,AMOUNT,UNITID) VALUES (?,?,?,?)";

                return await conn.ExecuteAsync(sql, recipeId,ingredient.Id,ingredient.AMOUNT,unit.ID);
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
        public async Task<int> RemoveAllIngredientsFromRecipeAsync(int recipeID)
        {
            try
            {
                var conn = this.GetConnection();
                string sql = @"DELETE FROM RECIPEINGREDIENT WHERE RECIPEID = ?";

                return await conn.ExecuteScalarAsync<int>(sql, recipeID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }
        public async Task<Ingredient?> IngredientPresentInDatabase(Ingredient ingredient)
        {
            Ingredient? result = new Ingredient();
            try
            {
                var conn = this.GetConnection();
                string sql = @"SELECT * FROM INGREDIENT WHERE LOWER(NAME) LIKE LOWER(?)";
                result = (await conn.QueryAsync<Ingredient>(sql, ingredient.NAME)).FirstOrDefault();

                if(result == null)
                {
                    throw new Exception($"Keine Zutat mit dem Namen [{ingredient.NAME}] gefunden");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }
        public async Task<int> GetLastIngredientIDAsync()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT MAX(ID) AS ID FROM INGREDIENT";
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
        public async Task<List<Unit>> GetAllUnitsAsync()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT ID,UNIT FROM UNIT";
                var result = await conn.QueryAsync<Unit>(sql);

                if (result == null)
                    throw new Exception("Keine Einheiten gefunden");

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Unit>();
            }
        }
        public async Task<Unit?> UnitPresentInDatabase(Unit unit)
        {
            Unit? result = new Unit();
            try
            {
                var connection = this.GetConnection();

                string sql = @"SELECT * FROM UNIT WHERE LOWER(UNIT) LIKE LOWER(?)";

                result = (await connection.QueryAsync<Unit>(sql, unit.UNIT)).FirstOrDefault();

                if(result == null)
                {
                    throw new Exception($"Keine Einheit mit dem Namen [{unit.UNIT}] gefunden");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<int> AddUtensilAsync(Utensil utensil)
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"INSERT INTO UTENSIL (NAME, DESCRIPTION) VALUES (?, ?)";

                return await conn.ExecuteAsync(sql, utensil.NAME, utensil.DESRIPTION);
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

                return await conn.ExecuteAsync(sql, recipeId, utensil.ID, utensil.AMOUNT);
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
        public async Task<int> RemoveAllUtensilsFromRecipeAsync(int recipeID)
        {
            try
            {
                var conn = this.GetConnection();
                string sql = @"DELETE FROM RECIPEUTENSIL WHERE RECIPEID = ?";

                return await conn.ExecuteScalarAsync<int>(sql, recipeID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }
        public async Task<Utensil?> UtensilPresentInDatabase(Utensil utensil)
        {
            Utensil? result = new Utensil();
            try
            {
                var conn = this.GetConnection();
                string sql = @"SELECT * FROM UTENSIL WHERE LOWER(NAME) LIKE LOWER(?)";
                result = (await conn.QueryAsync<Utensil>(sql, utensil.NAME)).FirstOrDefault();

                if (result == null)
                {
                    throw new Exception($"Kein Utensil mit dem Namen [{utensil.NAME}] gefunden");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }
        public async Task<int> GetLastUtensilIDAsync()
        {
            try
            {
                var conn = this.GetConnection();

                string sql = @"SELECT MAX(ID) AS ID FROM UTENSIL";
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

        
    }
}
