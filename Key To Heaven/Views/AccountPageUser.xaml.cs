using System;
using KeyToHeaven.Data;
using Microsoft.Maui.Controls;
using static KeyToHeaven.Data.DatabaseHelper;

namespace KeyToHeaven.Views
{
    public partial class AccountPageUser : ContentPage
    {
        private int _userId;

        public AccountPageUser(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadUserData();
        }

        private async void LoadUserData()
        {
            var user = await DatabaseHelper.GetDonorById(_userId);
            if (user != null)
            {
                FirstNameEntry.Text = user.FirstName;
                LastNameEntry.Text = user.LastName;
                EmailEntry.Text = user.Email;
                PhoneEntry.Text = user.Phone;

                if (!string.IsNullOrEmpty(user.Image))
                {
                    UserImage.Source = user.Image;
                }
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var updatedUser = new Donor
            {
                Donor_Id = _userId,
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                Email = EmailEntry.Text,
                Phone = PhoneEntry.Text
            };

            bool success = await DatabaseHelper.UpdateDonor(updatedUser);
            if (success)
            {
                await DisplayAlert("Success", "User details updated successfully!", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to update user details.", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
