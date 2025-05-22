using RezeptSafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Services
{
    public interface IUser
    {
        public string GetUsername();
    }

    public class UserService : IUser
    {
        User current = new User();

        public string GetUsername()
        {
            return this.current.Username;
        }
    }
}
