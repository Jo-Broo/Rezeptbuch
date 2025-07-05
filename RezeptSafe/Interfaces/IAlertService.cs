using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Interfaces
{
    public interface IAlertService
    {
        IToast CreateToastMessageAsync(string message);

        Task ShowAlertAsync(string title, string message);
        
        Task<bool> ShowAlertWithChoiceAsync(string title, string message, string accept = "OK", string cancel = "Abbruch");

        Task<string> ShowPromptAsync(string title, string message);
    }
}
