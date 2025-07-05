using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezeptSafe.Interfaces;
using RezeptSafe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class ProfilViewModel : BaseViewModel
    {
        IUserService userService;

        [ObservableProperty]
        string username;

        [ObservableProperty]
        bool nameHasChanged;

        public ProfilViewModel(IUserService userService, IAlertService alertService) : base(alertService)
        {
            this.userService = userService;
            this.Username = this.userService.GetUsername();
            this.NameHasChanged = false;
            this.Title = "Profil";
        }

        [RelayCommand]
        async Task SetUsernameAsync(string username)
        {
            this.userService.SetUsername(username);
            this.NameHasChanged = false;
        }
    }
}
