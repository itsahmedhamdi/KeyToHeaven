using Stripe;
using Microsoft.Maui.Controls;
using System;

namespace KeyToHeaven.Views
{
    public partial class StripePaymentPage : ContentPage
    {
        private readonly string _clientSecret;

        public StripePaymentPage(string clientSecret)
        {
            InitializeComponent();
            _clientSecret = clientSecret;
        }

        private async void OnPayNowClicked(object sender, EventArgs e)
        {
            try
            {
                // Create Stripe payment method from card details
                var cardParams = new PaymentMethodCreateOptions
                {
                    Card = new PaymentMethodCardOptions
                    {
                        Number = CardNumberEntry.Text,
                        ExpMonth = Convert.ToInt32(ExpiryEntry.Text.Split('/')[0]),
                        ExpYear = Convert.ToInt32(ExpiryEntry.Text.Split('/')[1]),
                        Cvc = CVCEntry.Text
                    }
                };

                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = await paymentMethodService.CreateAsync(cardParams);

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.ConfirmAsync(
                    _clientSecret,
                    new PaymentIntentConfirmOptions
                    {
                        PaymentMethod = paymentMethod.Id
                    }
                );

                if (paymentIntent.Status == "succeeded")
                {
                    await DisplayAlert("Success", "Payment Successful", "OK");
                    await Navigation.PopAsync(); // Go back after success
                }
                else
                {
                    await DisplayAlert("Error", "Payment failed", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
