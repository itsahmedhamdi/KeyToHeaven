
using KeyToHeaven.Views;

namespace KeyToHeaven
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("associationpage", typeof(AssociationPage));


            InitializeComponent();
        }
    }
}
