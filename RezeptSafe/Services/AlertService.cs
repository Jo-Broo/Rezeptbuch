using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using RezeptSafe.Interfaces;

namespace RezeptSafe.Services
{
    internal class AlertService : IAlertService
    {
        public IToast CreateToastMessageAsync(string message)
        {
            return Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
        }

        public Task<bool> ShowAlertWithChoiceAsync(string title, string message, string accept, string cancel)
        {
            return Shell.Current.DisplayAlert(title, message,accept,cancel);
        }

        public Task ShowAlertAsync(string title, string message)
        {
            return Shell.Current.DisplayAlert(title, message, "OK");
        }
    }
}
