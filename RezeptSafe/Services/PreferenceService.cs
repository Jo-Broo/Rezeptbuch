using Rezeptbuch.Enum;
using Rezeptbuch.Interfaces;

namespace Rezeptbuch.Services
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
