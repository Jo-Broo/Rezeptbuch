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
}