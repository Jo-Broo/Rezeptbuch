using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Interfaces
{
    public interface IProgressReporter
    {
        Task PerformStep(string status);
        void Initialize(int recipeCount, int ingredientCount, int unitCount, int utensilCount);
        void SetStatus(string status);
    }
}
