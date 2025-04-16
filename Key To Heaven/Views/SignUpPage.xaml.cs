using KeyToHeaven.Data;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace KeyToHeaven.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void OnUserTypeChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isCharity = charityRadio.IsChecked;
            idMinistryContainer.IsVisible = isCharity;
            descriptionContainer.IsVisible = isCharity;

            // Smoothly fade in/out additional fields
            if (isCharity)
            {
                idMinistryContainer.Opacity = 0;
                idMinistryContainer.FadeTo(1, 300);
                descriptionContainer.Opacity = 0;
                descriptionContainer.FadeTo(1, 300);
            }
            else
            {
                idMinistryContainer.FadeTo(0, 300);
                descriptionContainer.FadeTo(0, 300);
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            string firstName = firstNameEntry.Text?.Trim();
            string lastName = lastNameEntry.Text?.Trim();
            string email = emailEntry.Text?.Trim();
            string password = passwordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;
            string phone = phoneEntry.Text?.Trim();
            string userType = donatorRadio.IsChecked ? "Donor" : charityRadio.IsChecked ? "Association" : null;
            string idMinistry = idMinistryEntry.Text?.Trim();
            string description = descriptionEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(phone) || userType == null)
            {
                await DisplayAlert("Error", "Please fill in all required fields", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            bool success = false;

            if (userType == "Donor")
            {
                success = await DatabaseHelper.RegisterDonor(firstName, lastName, email, password, phone);
            }
            else if (userType == "Association")
            {
                if (string.IsNullOrWhiteSpace(idMinistry) || string.IsNullOrWhiteSpace(description))
                {
                    await DisplayAlert("Error", "Please provide ID Ministry and Description for the association", "OK");
                    return;
                }

                success = await DatabaseHelper.RegisterAssociation(firstName, lastName, email, password, phone, idMinistry, description);
            }

            if (success)
            {
                await DisplayAlert("Success", "Account created successfully!", "OK");
                await Navigation.PopAsync(); // Navigate back to login
            }
            else
            {
                await DisplayAlert("Error", "Failed to create account. Email might already be registered.", "OK");
            }
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Navigate back to login page
        }
    }
}
