using CommunityToolkit.Mvvm.ComponentModel;
using Rezeptbuch.Interfaces;

namespace Rezeptbuch.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;
        [ObservableProperty]
        string title;

        internal IAlertService _alertService;

        public BaseViewModel(IAlertService alertService)
        {
            this._alertService = alertService;
            this.Title = "Nicht festegelgt";
        }

        public bool IsNotBusy => !this.IsBusy;
    }
}
