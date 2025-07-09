using Rezeptbuch.Interfaces;
using Rezeptbuch.Model;

namespace Rezeptbuch.Services
{
    

    public class UserService : IUserService
    {
        User current = new User();

        IPreferenceService _preferenceService;

        public UserService(IPreferenceService preferenceService)
        {
            this._preferenceService = preferenceService;

            string preference = this._preferenceService.GetPreference(Enum.RezeptbuchPreferences.UserName);

            if (preference == string.Empty)
            {
                this.current.Username = DeviceInfo.Name;
            }
            else
            {
                this.current.Username = preference;
            }
        }

        public string GetUsername()
        {
            return this.current.Username;
        }

        public void SetUsername(string username)
        {
            this.current.Username = username;
            this._preferenceService.SetPreference(Enum.RezeptbuchPreferences.UserName,username);
        }
    }
}
