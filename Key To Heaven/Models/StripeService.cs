using RestSharp;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace KeyToHeaven.Services
{
    public class StripeService
    {
        private const string StripeSecretKey = "sk_live_YOUR_LIVE_SECRET_KEY"; // Replace with your live secret key
        private const string StripeBaseUrl = "https://api.stripe.com/v1";

        public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "usd")
        {
            var client = new RestClient(StripeBaseUrl);
            var request = new RestRequest("/payment_intents", Method.Post);
            request.AddHeader("Authorization", $"Bearer {StripeSecretKey}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("amount", ((int)(amount * 100)).ToString()); // Convert to cents
            request.AddParameter("currency", currency);
            request.AddParameter("payment_method_types[]", "card");

            var response = await client.ExecuteAsync(request);
            var json = JObject.Parse(response.Content);

            return json["client_secret"]?.ToString();  // Return client secret for the Stripe payment
        }
    }
}
