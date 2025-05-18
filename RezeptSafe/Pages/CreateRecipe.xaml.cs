using RezeptSafe.Data;
using RezeptSafe.Model;

namespace RezeptSafe.Pages;

public partial class CreateRecipe : ContentPage
{
    private readonly IRecipeDatabase database;
    private readonly IUser user;

    public CreateRecipe(IRecipeDatabase db, IUser user)
	{
		InitializeComponent();
		this.database = db;
        this.user = user;

        this.Username.Text = $"erstellt von {this.user.GetUsername()}";
	}

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var recipe = new Recipe
        {
            Title = TitleEntry.Text,
            Description = DescriptionEditor.Text,
            Username = this.Username.Text,
            //Ingredients = IngredientsEntry.Text
            //    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            //    .Select(name => new Ingredient { Name = name }).ToList(),
            //Utensils = UtensilsEntry.Text
            //    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            //    .Select(name => new Utensil { Name = name }).ToList()
        };

        await database.AddRecipeAsync(recipe);

        await Navigation.PopAsync(); // zurück zur Hauptseite
    }
}