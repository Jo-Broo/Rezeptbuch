using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Rezeptbuch.Model
{
    public partial class Ingredient : ObservableObject
    {
        public int ID { get; set; }
        public string NAME { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;
        public double AMOUNT { get; set; }
        public int UNITID { get; set; }
        public string UNIT { get; set; } = string.Empty;
        [ObservableProperty]
        Unit selectedUnit;
        [ObservableProperty]
        bool isSelected;
        [ObservableProperty]
        ObservableCollection<Unit> units;
        public override string ToString()
        {
            return $"{this.AMOUNT} {this.UNIT} {this.NAME}";
        }
    }
}