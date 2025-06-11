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
        foreach (Ingredient removed in e.PreviousSelection)
            removed.IsSelected = false;

        foreach (Ingredient added in e.CurrentSelection)
            added.IsSelected = true;
    }

    private void OnUtensilsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (Utensil removed in e.PreviousSelection)
            removed.IsSelected = false;

        foreach (Utensil added in e.CurrentSelection)
            added.IsSelected = true;
    }

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