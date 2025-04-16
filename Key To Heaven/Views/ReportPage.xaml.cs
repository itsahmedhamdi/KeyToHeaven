namespace KeyToHeaven.Views;

public partial class ReportPage : ContentPage
{
    public ReportPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSubmitReportClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ReportEditor.Text))
        {
            await DisplayAlert("Missing Info", "Please describe the issue before submitting.", "OK");
            return;
        }

        await DisplayAlert("Thank you!", "Your report has been submitted.", "OK");
        await Navigation.PopAsync();
    }
}
