using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // Ensure Newtonsoft.Json package is installed

namespace AzureOpenAIExample
{
    class Program
    {
        private static readonly string apiKey = "mfxPoLRPrpqCq4bwZmqdEI25WbZRNLTj"; // Replace with your actual API key
        private static readonly string endpoint = "https://Phi-3-small-8k-instruct-axtje.eastus2.models.ai.azure.com/v1/chat/completions"; // Replace with your actual endpoint URL

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter your prompt:");
            string prompt = Console.ReadLine();

            string response = await GetOpenAIResponse(prompt);
            Console.WriteLine("Response from OpenAI:");
            Console.WriteLine(response);
        }

        private static async Task<string> GetOpenAIResponse(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful assistant." },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 50
                };

                var content = new StringContent(
                    JObject.FromObject(requestBody).ToString(),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseBody);

                return jsonResponse["choices"][0]["message"]["content"].ToString();
            }
        }
    }
}