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
    }

    public class UserService : IUserService
    {
        User current = new User();

        public UserService()
        {
            current.Username = DeviceInfo.Name;
        }

        public string GetUsername()
        {
            return this.current.Username;
        }
    }
}
