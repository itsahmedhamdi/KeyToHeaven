using KeyToHeaven.Data;
using Microsoft.Maui.Controls;
using System;

namespace KeyToHeaven.Views
{
    public partial class AssociationPage : ContentPage
    {
        private readonly int associationId;  // Store the associationId

        // Constructor now accepts an Association object and extracts the association ID
        public AssociationPage(Association association)
        {
            InitializeComponent();
            associationId = association.Association_Id;  // Store the association's ID
            LoadAssociation(association);
        }

        // Back button tapped, navigate to the HomePageUser and pass the associationId
        private async void OnBackButtonTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Method to load association details into the page
        private void LoadAssociation(Association association)
        {
            try
            {
                AssociationImage.Source = association.Image;
                NameLabel.Text = $"{association.FirstName} {association.LastName}";
                EmailLabel.Text = $"📧 {association.Email}";
                PhoneLabel.Text = $"📞 {association.Phone}";
                IDMinistryLabel.Text = $"🆔 Ministry: {association.IDMinistry}";
                DescriptionLabel.Text = string.IsNullOrWhiteSpace(association.Description)
                    ? "No description provided."
                    : association.Description;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading association: {ex.Message}");
            }
        }
    }
}
