using RezeptSafe.Interfaces;
using RezeptSafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Services
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
