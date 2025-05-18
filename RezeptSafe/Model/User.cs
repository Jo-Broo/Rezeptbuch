using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Model
{
    public interface IUser
    {
        string GetUsername();
    }
    public class User : IUser
    {
        public string Username = "Gast";

        public string GetUsername()
        {
            return this.Username;
        }
    }
}
