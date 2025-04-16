using KeyToHeaven.Data;

using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace KeyToHeaven.Views
{
    public partial class AccountPageAssociation : ContentPage
    {
        private int _associationId;
        private Association _association;

        public AccountPageAssociation(int associationId)
        {
            InitializeComponent();
            _associationId = associationId;
            LoadAssociationDetails();
        }

        private async void LoadAssociationDetails()
        {
            _association = await DatabaseHelper.GetAssociationById(_associationId);
            if (_association != null)
            {
                // Displaying existing details as placeholders
                FirstNameEntry.Text = _association.FirstName;
                Password.Text = _association.Password;
                EmailEntry.Text = _association.Email;
                PhoneEntry.Text = _association.Phone;
                DescriptionEditor.Text = _association.Description;

                // Load the image if available
                AssociationImage.Source = _association.Image ?? "default.jpg";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (_association != null)
            {
                _association.FirstName = FirstNameEntry.Text;
                _association.Password = Password.Text;
                _association.Phone = PhoneEntry.Text;
                _association.Description = DescriptionEditor.Text;

                bool success = await DatabaseHelper.UpdateAssociation(_association);
                if (success)
                {
                    await DisplayAlert("Success", "Your details have been updated.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update details. Try again.", "OK");
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
