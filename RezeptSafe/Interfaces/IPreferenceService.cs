using RezeptSafe.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Interfaces
{
    public interface IPreferenceService
    {
        void SetPreference(RezeptbuchPreferences name, string value);
        string GetPreference(RezeptbuchPreferences name);
    }
}
