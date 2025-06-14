using CommunityToolkit.Maui.Views;
using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class ProgressPopup : Popup
{
    int max = 100;
    int current = 0;

	public ProgressPopup()
	{
		InitializeComponent();
		this.CanBeDismissedByTappingOutsideOfPopup = false;
	}

    public void SetMaxValue(int maxValue)
    {
        this.max = maxValue;
        this.current = 0;
    }

    public async Task PerformStep()
    {
        if (current < max)
            current++;

        double progress = (double)current / max;
        await UpdateProgressAsync(progress, $"Verarbeite Schritt {current} von {max}...");
    }

    public void SetTitleText(string text)
    {
        this.TitleLabel.Text = text;
    }

    public async Task UpdateProgressAsync(double value, string status)
    {
        this.SetTitleText(status);
        this.PercentLabel.Text = $"{(int)(value * 100)}%";
        await this.Progressbar.ProgressTo(value, 50, Easing.CubicInOut);
    }

    public void SetRecipeCount(int count)
    {
        this.RecipeCount.Text = $"gefundene Rezepte: {count}";
    }

    public void SetIngredientCount(int count)
    {
        this.IngredientCount.Text = $"gefundene Zutaten: {count}";
    }

    public void SetUtensilCount(int count)
    {
        this.UtensilCount.Text = $"gefundene Utensilien: {count}";
    }
}