using Rezeptbuch.Enum;

namespace Rezeptbuch.Interfaces
{
    public interface IPreferenceService
    {
        void SetPreference(RezeptbuchPreferences name, string value);
        string GetPreference(RezeptbuchPreferences name);
    }
}
