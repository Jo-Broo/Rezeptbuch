using CommunityToolkit.Mvvm.ComponentModel;

namespace Rezeptbuch.Model
{ 
    
    public partial class Utensil : ObservableObject
    {
        public int ID { get; set; }
        public string NAME { get; set; } = string.Empty;
        public string DESRIPTION { get; set; } = string.Empty;
        public int AMOUNT { get; set; }
        [ObservableProperty]
        bool isSelected;
        public override string ToString()
        {
            return $"{AMOUNT}x {NAME}";
        }
    }
}
