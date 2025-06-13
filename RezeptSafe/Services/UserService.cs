using RezeptSafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Services
{
    public interface IUserService
    {
        public string GetUsername();
        public void SetUsername(string username);
    }

    public class UserService : IUserService
    {
        User current = new User();

        public UserService()
        {
            string preference = Preferences.Get("Username", string.Empty);

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
            Preferences.Set("Username", username);
        }
    }
}
