using RezeptSafe.View;

namespace RezeptSafe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // nameof(DetailsPage) == "DetailsPage"
            Routing.RegisterRoute(nameof(DetailsPage),typeof(DetailsPage));
        }
    }
}
