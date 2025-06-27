using RezeptSafe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Services
{
    public class ThemeService : IThemeService
    {
        public void ToggleTheme()
        {
            try
            {
                App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;

                Preferences.Set("AppTheme", App.Current.UserAppTheme.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
