using System;
using KeyToHeaven.Data;
using Microsoft.Maui.Controls;

namespace KeyToHeaven.Views
{
    public partial class Loginpage : ContentPage
    {
        private bool isPasswordVisible = false;

        public Loginpage()
        {
            InitializeComponent();
        }

        private void OnTogglePasswordClicked(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            PasswordEntry.IsPassword = !isPasswordVisible;
            TogglePasswordButton.Source = isPasswordVisible ? "eye_icon1.png" : "eye_icon2.png";
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both email and password", "OK");
                return;
            }

            var (isAuthenticated, userType, userIdOrAssociationId) = await DatabaseHelper.AuthenticateUser(email, password);

            if (isAuthenticated)
            {
                if (userType == "Donor")
                {
                    await Navigation.PushAsync(new HomePageUser(userIdOrAssociationId)); // userId
                }
                else if (userType == "Association")
                {
                    await Navigation.PushAsync(new HomePageAssociation(userIdOrAssociationId)); // associationId
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid email or password", "OK");
            }
        }

        private async void OnSignInTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}
