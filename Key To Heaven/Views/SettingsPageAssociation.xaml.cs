namespace KeyToHeaven.Views;

public partial class SettingsPageAssociation : ContentPage
{
    private int _associationId;
    public SettingsPageAssociation()

     
    {
        InitializeComponent();
    }

    private async void OnBackButtonTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Go back to previous page
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
        
        DisplayAlert("Success", "Changes saved successfully!", "OK");
    }
}
