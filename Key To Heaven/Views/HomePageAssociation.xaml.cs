using KeyToHeaven.Data;
using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyToHeaven.Data;
using KeyToHeavean.Views;
using static KeyToHeaven.Data.DatabaseHelper;


namespace KeyToHeaven.Views
{
    public partial class HomePageAssociation : ContentPage
    {
        private int _associationId;  

        public HomePageAssociation(int associationId)
        {
            InitializeComponent();
            _associationId = associationId;
            LoadBeneficiaries();
            LoadDonationChart();
        }


        private async void LoadBeneficiaries()
        {
            try
            {
                var beneficiaries = await DatabaseHelper.GetBeneficiariesByAssociation(_associationId);
                BeneficiaryListView.ItemsSource = beneficiaries;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading beneficiaries: {ex.Message}");
            }
        }
        private async void OnHomeTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new HomePageAssociation(_associationId)); // Reloads Home Page
        }


        private async void OnProfileTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new AccountPageAssociation(_associationId)); // Navigate to Profile Page
        }


        private async void OnSettingsTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new SettingsPageAssociation()); // Navigate to Settings Page
        }
        private async void OnIconTapped(object sender, EventArgs e)
        {
            if (sender is Image image)
            {
                // Animate the icon by scaling it up and then back to its original size
                await image.ScaleTo(1.2, 100); // Scale up to 120% over 100 milliseconds
                await image.ScaleTo(1.0, 100); // Scale back to 100% over 100 milliseconds
            }
        }
        private void LoadDonationChart()
        {
            List<DonationData> donations = DatabaseHelper.GetDonationHistory();

            var entries = donations.Select(d => new ChartEntry((float)d.Amount)
            {
                Label = d.Date.ToString("MMM dd"),
                ValueLabel = d.Amount.ToString("F2"),
                Color = SKColor.Parse("#FF8C42") 
            }).ToList();

            DonationChart.Chart = new BarChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent, 
                BarAreaAlpha = 40, 
                LabelTextSize = 18, 
                LabelOrientation = Orientation.Horizontal, 
                ValueLabelOrientation = Orientation.Horizontal,
               
            };
        }
        private async void OnCaseTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Beneficiary selectedBeneficiary)
            {
                await Navigation.PushAsync(new CasesDetailAssociation(selectedBeneficiary));
            }

        }
        private async void OnAddCaseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCasePage());
        }

    }
}
