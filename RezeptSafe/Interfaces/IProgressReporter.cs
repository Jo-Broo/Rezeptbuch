namespace Rezeptbuch.Interfaces
{
    public interface IProgressReporter
    {
        Task PerformStep(string status);
        void Initialize(int recipeCount, int ingredientCount, int unitCount, int utensilCount);
        void SetStatus(string status);
    }
}
