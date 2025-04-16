namespace KeyToHeaven.Views;

public partial class FeedbackPage : ContentPage
{
    public FeedbackPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSendFeedbackClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Feedback Sent", "Thank you for your feedback!", "OK");
        await Navigation.PopAsync();
    }
}
