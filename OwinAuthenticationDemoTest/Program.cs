using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace OwinAuthenticationDemoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:56391";
            Token token = new Token();
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                    {"grant_type", "password" },
                    {"username", "abhinav" },
                    {"password", "user123456" },
                };

                //Get the token first
                var tokenResponse = client.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(form)).Result;
                //var token = tokenResponse.Content.ReadAsStringAsync().Result;
                token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;

                if (string.IsNullOrEmpty(token.Error))
                {
                    Console.WriteLine("Token issued is {0}", token.AccessToken);
                }
                else
                {
                    Console.WriteLine("Error: {0}", token.Error);
                }
            }

            //Call the Authorized Test api method from controller
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));
                HttpResponseMessage response = httpClient.GetAsync("api/Test/AddApiUserAsync2").Result;
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("API Call Successful");

                string message = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("URL response: " + message);
            }
            Console.Read();
        }
    }
}
