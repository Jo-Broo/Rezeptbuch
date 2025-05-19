using RezeptSafe.Data;
using RezeptSafe.Model;

namespace RezeptSafe.Pages;

public partial class CreateRecipe : ContentPage
{
    private readonly IRecipeDatabase database;
    private readonly IUser user;

    private List<UtensilWithAmount> allUtensils;
    private List<IngredientWithAmount> allIngredients;

    public CreateRecipe(IRecipeDatabase db, IUser user)
	{
		InitializeComponent();
		this.database = db;
        this.user = user;

        this.Username.Text = $"erstellt von {this.user.GetUsername()}";
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        allUtensils = (await database.GetAllUtensilsAsync())
                .Select(u => new UtensilWithAmount { Id = u.Id, Name = u.Name, Amount = 0 })
                .ToList();

        Utensils.ItemsSource = allUtensils;

        allIngredients = (await database.GetAllIngredientsAsync())
                            .Select(i => new IngredientWithAmount { Id = i.Id, Name = i.Name, Amount = 0 })
                            .ToList();

        Ingredients.ItemsSource = allIngredients;
    }

    private void OnUtensilsSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.ToLower() ?? "";
        Utensils.ItemsSource = allUtensils
            .Where(x => x.Name.ToLower().Contains(keyword)).ToList();
    }

    private void OnIngredientsSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.ToLower() ?? "";
        Ingredients.ItemsSource = allIngredients
            .Where(x => x.Name.ToLower().Contains(keyword)).ToList();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var recipe = new Recipe
        {
            Title = TitleEntry.Text,
            Description = DescriptionEditor.Text,
            Username = this.Username.Text,
        };

        // ID for the recipe gets updated here
        await database.AddRecipeAsync(recipe);

        foreach (var obj in this.Ingredients.SelectedItems)
        {
            IngredientWithAmount ingredient = (IngredientWithAmount)obj;

            await this.database.AddIngredientToRecipeAsync(recipe.Id, ingredient);
        }

        foreach (var obj in this.Utensils.SelectedItems)
        {
            UtensilWithAmount utensil = (UtensilWithAmount)obj;

            await this.database.AddUtensilToRecipeAsync(recipe.Id, utensil);
        }

        await Navigation.PopAsync(); // zurück zur Hauptseite
    }
}