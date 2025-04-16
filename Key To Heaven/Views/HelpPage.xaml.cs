namespace KeyToHeaven.Views;

public partial class HelpPage : ContentPage
{
    public HelpPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnContactSupportClicked(object sender, EventArgs e)
    {
        await Launcher.Default.OpenAsync("mailto:keytoheaven@gmail.com");
    }
}
