using RezeptSafe.Enum;
using RezeptSafe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Services
{
    public class PreferenceService : IPreferenceService
    {
        public string GetPreference(RezeptbuchPreferences name)
        {
            return Preferences.Get(name.ToString(),string.Empty);
        }

        public void SetPreference(RezeptbuchPreferences name, string value)
        {
            Preferences.Set(name.ToString(), value);
        }
    }
}
