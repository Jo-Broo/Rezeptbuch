using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using RezeptSafe.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;
        [ObservableProperty]
        string title;

        internal IAlertService alertService;

        public BaseViewModel(IAlertService alertService)
        {
            this.alertService = alertService;
        }

        public bool IsNotBusy => !this.IsBusy;
    }
}
