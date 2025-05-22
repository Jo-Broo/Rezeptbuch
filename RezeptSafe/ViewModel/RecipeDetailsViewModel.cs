using CommunityToolkit.Mvvm.ComponentModel;
using RezeptSafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    [QueryProperty("Recipe","Recipe")]
    public partial class RecipeDetailsViewModel : BaseViewModel
    {
        public RecipeDetailsViewModel()
        {
            
        }

        [ObservableProperty]
        Recipe recipe;
    }
}
