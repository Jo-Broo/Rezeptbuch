using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Interfaces
{
    public interface IUserService
    {
        public string GetUsername();
        public void SetUsername(string username);
    }
}
