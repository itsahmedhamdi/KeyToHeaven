using KeyToHeaven.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace KeyToHeaven.Views
{
    public partial class DonatePage : ContentPage
    {
        private PayPalService _payPalService = new PayPalService();
        private StripeService _stripeService = new StripeService();

        public DonatePage()
        {
            InitializeComponent();
        }

        // PayPal Payment
        private async void OnPayPalPaymentClicked(object sender, EventArgs e)
        {
            string amountText = AmountEntry.Text;
            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid amount.", "OK");
                return;
            }

            string orderId = await _payPalService.CreateOrderAsync(amount);
            if (!string.IsNullOrEmpty(orderId))
            {
                string checkoutUrl = $"https://www.api-m.paypal.com/checkoutnow?token={orderId}";
                await Launcher.OpenAsync(new Uri(checkoutUrl));
            }
            else
            {
                await DisplayAlert("Payment Error", "Failed to initiate PayPal payment.", "OK");
            }
        }

        // Stripe Payment (MasterCard, Visa, etc.)
        private async void OnStripePaymentClicked(object sender, EventArgs e)
        {
            string amountText = AmountEntry.Text;
            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid amount.", "OK");
                return;
            }

            string clientSecret = await _stripeService.CreatePaymentIntentAsync(amount);
            if (!string.IsNullOrEmpty(clientSecret))
            {
                string checkoutUrl = $"https://checkout.stripe.com/pay/{clientSecret}";
                await Launcher.OpenAsync(new Uri(checkoutUrl));
            }
            else
            {
                await DisplayAlert("Payment Error", "Failed to process Stripe payment.", "OK");
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync(); // Go back to the previous page
        }
    }
}
