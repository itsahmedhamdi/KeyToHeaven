namespace KeyToHeaven.Views;

public partial class CasesDetailUser : ContentPage
{
    public CasesDetailUser(Beneficiary beneficiary)
    {
        InitializeComponent(); 
        BindingContext = beneficiary;
    }
    private async void OnBackButtonTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void OnDonateButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DonatePage());
    }
    private async void OnShareButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var caseDetails = $"Help this beneficiary:{{Binding FullName}}\n" +
                              $"Organized by: {{Binding.Association_Name}}\n" +
                              $"Goal: {{Binding FormattedRaisedAmount}}\n" +
                              $"Description: {{Binding Description}}\n" +
                              $"Donate now: [Link to donate page]"; // You can add a link to the donation page

            // Using the Share functionality from .NET MAUI Essentials
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = caseDetails,
                Title = "Share this case"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sharing the case: {ex.Message}");
            await DisplayAlert("Error", "Failed to share the case.", "OK");
        }
    }
}


