using KeyToHeaven.Views;
using Microsoft.Maui.Controls;

namespace KeyToHeavean.Views
{
    public partial class SettingsPageUser : ContentPage
    {
        public SettingsPageUser()
        {
            InitializeComponent();
        }

        private async void OnBackButtonTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnHelpButtonTapped(object sender, TappedEventArgs e)
        {
            Navigation.PushAsync(new HelpPage());
        }

        private void OnReportButtonTapped(object sender, TappedEventArgs e)
        {
            Navigation.PushAsync(new ReportPage());
        }

        private void OnFeedbackButtonTapped(object sender, TappedEventArgs e)
        {
            Navigation.PushAsync(new FeedbackPage());
        }

        private void OnLogOutButtonTapped(object sender, TappedEventArgs e)
        {
            Navigation.PushAsync(new Loginpage());
        }

        private void OnSaveChangesButtonTapped(object sender, TappedEventArgs e)
        {
            // Ajoute ici la logique d'enregistrement des changements
            DisplayAlert("Success", "Changes saved successfully!", "OK");
        }
    }
}