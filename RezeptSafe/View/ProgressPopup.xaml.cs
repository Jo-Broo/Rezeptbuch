using CommunityToolkit.Maui.Views;
using Rezeptbuch.Interfaces;
using Rezeptbuch.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace Rezeptbuch.View;

public partial class ProgressPopup : Popup, IProgressReporter
{
    int max = 100;
    int current = 0;

	public ProgressPopup()
	{
		InitializeComponent();
		this.CanBeDismissedByTappingOutsideOfPopup = false;
	}

    public void Initialize(int recipeCount, int ingredientCount, int unitCount, int utensilCount)
    {
        this.RecipeCount.Text = $"Gefundene Rezepte: {recipeCount}";
        this.IngredientCount.Text = $"Gefundene Zutaten: {ingredientCount}";
        this.UnitCount.Text = $"Gefundene Einheiten: {unitCount}";
        this.UtensilCount.Text = $"Gefundene Utensilien: {utensilCount}";

        this.max = (recipeCount + ingredientCount + unitCount + utensilCount);
        this.current = 0;
    }

    public async Task PerformStep(string status)
    {
        if (current < max)
        {
            current++;
        }
        double progress = (double)current / max;

        if(status != "")
        {
            this.SetStatus(status);
        }

        this.PercentLabel.Text = $"{(int)(progress * 100)}%";
        await this.Progressbar.ProgressTo(progress, 50, Easing.CubicInOut);
    }

    public void SetStatus(string status)
    {
        this.StatusLabel.Text = status;
    }
}