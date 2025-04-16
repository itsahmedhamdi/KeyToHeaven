using RestSharp;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace KeyToHeaven.Services
{
    public class PayPalService
    {
        private const string ClientId = "YOUR_LIVE_PAYPAL_CLIENT_ID";  // Replace with your actual PayPal live client ID
        private const string Secret = "YOUR_LIVE_PAYPAL_SECRET";  // Replace with your actual PayPal live secret
        private const string PayPalBaseUrl = "https://api-m.paypal.com";  // PayPal live URL

        public async Task<string> CreateOrderAsync(decimal amount, string currency = "USD")
        {
            string accessToken = await GetAccessTokenAsync();

            var client = new RestClient(PayPalBaseUrl);
            var request = new RestRequest("/v2/checkout/orders", Method.Post);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = currency,
                            value = amount.ToString("F2")
                        }
                    }
                }
            };

            request.AddJsonBody(body);
            var response = await client.ExecuteAsync(request);
            var json = JObject.Parse(response.Content);

            return json["id"]?.ToString();  // Return order ID to initiate payment
        }

        public async Task<string> CaptureOrderAsync(string orderId)
        {
            string accessToken = await GetAccessTokenAsync();

            var client = new RestClient(PayPalBaseUrl);
            var request = new RestRequest($"/v2/checkout/orders/{orderId}/capture", Method.Post);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");

            var response = await client.ExecuteAsync(request);
            var json = JObject.Parse(response.Content);

            return json.ToString();  // Return the response containing payment status
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var options = new RestClientOptions(PayPalBaseUrl)
            {
                Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(ClientId, Secret)
            };

            var client = new RestClient(options);
            var request = new RestRequest("/v1/oauth2/token", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Accept-Language", "en_US");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");

            var response = await client.ExecuteAsync(request);
            var json = JObject.Parse(response.Content);

            return json["access_token"]?.ToString();  // Return the access token for authorization
        }
    }
}
