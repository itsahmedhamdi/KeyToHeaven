using System;
using Microsoft.Maui.Controls;


namespace KeyToHeaven.Views
{
    public partial class CasesDetailAssociation : ContentPage
    {
        private Beneficiary _beneficiary;
        private FileResult _resultPhoto;

        public CasesDetailAssociation(Beneficiary selectedBeneficiary)
        {
            InitializeComponent();

            _beneficiary = selectedBeneficiary;
            BindingContext = _beneficiary;
        }

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Simulate saving to database (replace with your DB code)
                bool result = SaveBeneficiaryData(_beneficiary);

                if (result)
                {
                    await DisplayAlert("Success", "Changes saved successfully.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to save changes.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }
        }


        private bool SaveBeneficiaryData(Beneficiary beneficiary)
        {

            return true;
        }
        private async void OnAddResultButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Result Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    _resultPhoto = result;
                    var stream = await result.OpenReadAsync();
                    ResultImagePreview.Source = ImageSource.FromStream(() => stream);
                    ResultImagePreview.IsVisible = true;

                    // Optional: Save or bind the image to your model
                    // ((Beneficiary)BindingContext).ResultImage = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not load image: {ex.Message}", "OK");
            }

        }
    }
}
