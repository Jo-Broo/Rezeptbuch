using CommunityToolkit.Mvvm.ComponentModel;
using Rezeptbuch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rezeptbuch.ViewModel
{
    public partial class AboutViewModel : BaseViewModel
    {
        [ObservableProperty]
        string appversion;
        [ObservableProperty]
        string buildnumber;
        [ObservableProperty]
        string description;

        public AboutViewModel(IAlertService alertService) : base(alertService)
        {
            this.Appversion = $"Appversion: {AppInfo.VersionString}";
            this.Buildnumber = $"Appbuild: {AppInfo.BuildString}";
            this.Description = @"
Mit dieser App möchte ich das dicke, unhandliche Kochbuch von Oma durch eine moderne und einfach zu bedienende Lösung ersetzen.
Alle Rezepte, die du erstellst, werden ausschließlich lokal auf deinem Gerät gespeichert.
Wenn du deine Rezepte sichern möchtest, kannst du die Datenbank ganz einfach exportieren und die Datei extern aufbewahren.

Rezepte teilen? Kein Problem!
Dazu wird ein individueller QR-Code erstellt, den eine andere Person nur scannen muss – ganz unkompliziert.

Wenn dir ein Problem auffällt oder du Ideen zur Verbesserung hast, schreib mir gern. Ich bin leicht erreichbar und freue mich über Feedback!";
        }
    }
}
