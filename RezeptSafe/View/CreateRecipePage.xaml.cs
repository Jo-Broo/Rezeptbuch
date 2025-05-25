using RezeptSafe.Services;
using RezeptSafe.Model;
using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class CreateRecipePage : ContentPage
{
    public CreateRecipePage(CreateRecipeViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;
	}

    private void OnIngredientsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (IngredientWithAmount removed in e.PreviousSelection)
            removed.IsSelected = false;

        foreach (IngredientWithAmount added in e.CurrentSelection)
            added.IsSelected = true;
    }

    //private void OnUtensilsSearchTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    var keyword = e.NewTextValue?.ToLower() ?? "";
    //    Utensils.ItemsSource = allUtensils
    //        .Where(x => x.Name.ToLower().Contains(keyword)).ToList();
    //}

    //private void OnIngredientsSearchTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    var keyword = e.NewTextValue?.ToLower() ?? "";
    //    Ingredients.ItemsSource = allIngredients
    //        .Where(x => x.Name.ToLower().Contains(keyword)).ToList();
    //}

    //private async void OnSaveClicked(object sender, EventArgs e)
    //{
    //    var recipe = new Recipe
    //    {
    //        Title = TitleEntry.Text,
    //        Description = DescriptionEditor.Text,
    //        Username = this.Username.Text,
    //    };

    //    // ID for the recipe gets updated here
    //    await database.AddRecipeAsync(recipe);

    //    foreach (var obj in this.Ingredients.SelectedItems)
    //    {
    //        IngredientWithAmount ingredient = (IngredientWithAmount)obj;

    //        await this.database.AddIngredientToRecipeAsync(recipe.Id, ingredient);
    //    }

    //    foreach (var obj in this.Utensils.SelectedItems)
    //    {
    //        UtensilWithAmount utensil = (UtensilWithAmount)obj;

    //        await this.database.AddUtensilToRecipeAsync(recipe.Id, utensil);
    //    }

    //    await Navigation.PopAsync(); // zurück zur Hauptseite
    //}
}