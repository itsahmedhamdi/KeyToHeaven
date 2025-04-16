using KeyToHeaven.Data;
namespace KeyToHeaven.Views;

public partial class AddCasePage : ContentPage
{
    private FileResult _imageFile;

    public AddCasePage()
    {
        InitializeComponent();
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            _imageFile = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select an image",
                FileTypes = FilePickerFileType.Images
            });

            if (_imageFile != null)
            {
                var stream = await _imageFile.OpenReadAsync();
                CaseImage.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
        }
    }

    private async void OnSaveCaseClicked(object sender, EventArgs e)
    {
        var firstName = FirstNameEntry.Text;
        var lastName = LastNameEntry.Text;
        var description = DescriptionEditor.Text;
        var daysText = DaysEntry.Text;

        if (!int.TryParse(daysText, out int days))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid number of days.", "OK");
            return;
        }

        byte[] imageBytes = null;
        if (_imageFile != null)
        {
            using var stream = await _imageFile.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            imageBytes = memoryStream.ToArray();
        }

        await DatabaseHelper.AddNewBeneficiaryAsync(firstName, lastName, description, days, imageBytes);

        await DisplayAlert("Success", "New case added!", "OK");
        await Navigation.PopAsync();
    }
    private async void OnBackButtonTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Go back to previous page
    }
}

