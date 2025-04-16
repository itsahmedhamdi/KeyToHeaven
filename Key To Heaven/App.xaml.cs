using Microsoft.Maui.Controls;

namespace KeyToHeaven
{
    public partial class App : Application
    {
        [Obsolete]
        public App()
        {
            InitializeComponent();


            int associationId = Preferences.Get("AssociationId",1); 
            MainPage = new NavigationPage(new Views.Loginpage());
        }
    }
}
