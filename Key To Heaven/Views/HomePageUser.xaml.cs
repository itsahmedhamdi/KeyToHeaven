using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using KeyToHeavean.Views;
using KeyToHeaven.Data;
using Microsoft.Maui.Controls;

namespace KeyToHeaven.Views
{
    public partial class HomePageUser : ContentPage
    {
        public ObservableCollection<Association> Associations { get; set; } = new ObservableCollection<Association>();
        public ObservableCollection<Beneficiary> Beneficiaries { get; set; } = new ObservableCollection<Beneficiary>();
        private List<Beneficiary> allBeneficiaries = new List<Beneficiary>();

        private int _userId;

        // Constructor that receives userId
        public HomePageUser(int userId)
        {
            InitializeComponent();
            _userId = userId;
            BindingContext = this;

            LoadData();

            ApplyAnimation(FoodIcon, "Food");
            ApplyAnimation(HealthcareIcon, "Healthcare");
            ApplyAnimation(EducationIcon, "Education");
            ApplyAnimation(BuildingIcon, "Building");
        }

        private async void LoadData()
        {
            try
            {
                var associations = await DatabaseHelper.GetAssociations();
                var beneficiaries = await DatabaseHelper.GetBeneficiaries();

                Associations.Clear();
                Beneficiaries.Clear();
                allBeneficiaries.Clear();

                foreach (var association in associations)
                    Associations.Add(association);

                foreach (var beneficiary in beneficiaries)
                    allBeneficiaries.Add(beneficiary);

                Beneficiaries = new ObservableCollection<Beneficiary>(allBeneficiaries);
                OnPropertyChanged(nameof(Beneficiaries));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                await DisplayAlert("Error", "Failed to load data. Please try again.", "OK");
            }
        }

        private async void OnProfileTapped(object sender, TappedEventArgs e)
        {
            // Pass the userId to the AccountPageUser
            await Navigation.PushAsync(new AccountPageUser(_userId));
        }

        private async void OnSettingsTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new SettingsPageUser());
        }

        private async void OnBeneficiaryTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Beneficiary selectedBeneficiary)
            {
                await Navigation.PushAsync(new CasesDetailUser(selectedBeneficiary));
            }
        }

        private async void OnHomeTapped(object sender, TappedEventArgs e)
        {
            // Reload HomePage with same userId
            await Navigation.PushAsync(new HomePageUser(_userId));
        }

        private async void OnAssociationTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Association selectedAssociation)
            {
                await Navigation.PushAsync(new AssociationPage(selectedAssociation));
            }
        }

        private void ApplyAnimation(VerticalStackLayout icon, string category)
        {
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await icon.ScaleTo(1.2, 100);
                await icon.ScaleTo(1.0, 100);

                FilterBeneficiaries(category);
            };

            icon.GestureRecognizers.Add(tapGesture);
        }

        private void FilterBeneficiaries(string category)
        {
            var filteredList = allBeneficiaries.Where(b => b.Needs.Contains(category)).ToList();
            Beneficiaries.Clear();
            foreach (var beneficiary in filteredList)
                Beneficiaries.Add(beneficiary);
        }
    }
}
