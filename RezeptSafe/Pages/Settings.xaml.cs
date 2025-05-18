using RezeptSafe.Data;

namespace RezeptSafe.Pages;

public partial class Settings : ContentPage
{
	private IRecipeDatabase database;
	
	public Settings(IRecipeDatabase db)
	{
		InitializeComponent();
		this.database = db;
		UpdateThemeButtonText();
		GetDBInformations();
	}

	private void UpdateThemeButtonText()
	{
		ThemeToggleButton.Text = App.Current.UserAppTheme == AppTheme.Dark
					? "Wechsle zu Hellmodus"
					: "Wechsle zu Dunkelmodus";
	}

	private async void GetDBInformations()
	{
		DBStatus.Text = this.database.DBOnline()
			? "Status: Online"
			: "Status: Offline";

		var recipes = await this.database.GetAllRecipesAsync();
		var ingredients = await this.database.GetAllIngredientsAsync();
		var utensils = await this.database.GetAllUtensilsAsync();

		RecipeCount.Text = $"Rezeptanzahl: {recipes.Count}";
		IngredientCount.Text = $"Zutatenanzahl: {ingredients.Count}";
		UtensilCount.Text = $"Utensilienanzahl: {utensils.Count}";
	}

	private void OnToggleThemeClicked(object sender, EventArgs e)
	{
		App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
			? AppTheme.Light
			: AppTheme.Dark;

		Preferences.Set("AppTheme",App.Current.UserAppTheme.ToString());

		UpdateThemeButtonText();
	}

    private async void ExportDB(object sender, EventArgs e)
    {
		try
		{
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Rezepte.db");
            string exportPath = Path.Combine(FileSystem.Current.CacheDirectory, "Rezepte_export.db");

            File.Copy(dbPath, exportPath, overwrite: true);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "SQLite DB exportieren",
                File = new ShareFile(exportPath)
            });

            await DisplayAlert("Information", "Datenbank wurde erfolgreich exportiert", "OK");
        }
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
    }

    private async void ImportDB(object sender, EventArgs e)
	{
        string dbPath = RecipeDatabase.DbPath;

        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
			{
				PickerTitle = "Datenbank auswählen"
			});

            if (result != null)
            {
				using var stream = await result.OpenReadAsync();
                using var destStream = File.Create(dbPath);
                await stream.CopyToAsync(destStream);
            }

			database.ResetAndInitConnection();

            await DisplayAlert("Information", "Datenbank wurde erfolgreich importiert", "OK");
        }
		catch (Exception ex)
		{

            await DisplayAlert("Error", ex.Message, "OK");
        }
	}

	private async void DeleteDB(object sender, EventArgs e)
	{
		if(await DisplayAlert("! Achtung !", "Hiermit löschen sie alle ihre Rezepte, dieser Schritt kann nicht rückgängig gemacht werden", "Verstanden", "Abbrechen"))
		{
            try
            {
                string dbPath = RecipeDatabase.DbPath;

                database.CloseConnection();

                File.Delete(dbPath);

                if (!File.Exists(dbPath))
                {
                    await DisplayAlert("Info", "Datenbank wurde erfolgreich zurückgesetzt.", "OK");
                }

                database.ResetAndInitConnection();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}