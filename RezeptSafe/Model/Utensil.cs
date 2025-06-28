using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Model
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
