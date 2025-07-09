using CommunityToolkit.Maui.Core;

namespace Rezeptbuch.Interfaces
{
    public interface IAlertService
    {
        IToast CreateToastMessageAsync(string message);

        Task ShowAlertAsync(string title, string message);
        
        Task<bool> ShowAlertWithChoiceAsync(string title, string message, string accept = "OK", string cancel = "Abbruch");

        Task<string> ShowPromptAsync(string title, string message);
    }
}
