using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FantasyBot
{
    public class CriticService
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _remoteServiceBaseUrl;
        private static readonly string _remoteServiceLoginUrl;

        static CriticService()
        {
            // Define a persistant session.
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            _httpClient = new HttpClient(handler);
            _remoteServiceBaseUrl = "https://www.fantasycritic.games/api/League/GetLeagueYear";
            _remoteServiceLoginUrl = "https://www.fantasycritic.games/api/account/login";
        }
        public async Task<string> PostProductAsync()
        {
            // Setup login Json. 
            var creds = new Login
            {
                emailAddress = "",
                password = ""
            };
            var payload = JsonConvert.SerializeObject(creds);
            var requestContent = new StringContent(payload);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.JsonContent); // Might need to move to handler.

            // Post login credentials
            // Using might be over kill now that it has a smaller scope. 
            using (var response = await _httpClient.PostAsync(_remoteServiceLoginUrl, requestContent))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        // This method uses the shared instance of HttpClient for every call to GetProductAsync.
        public async Task<string> GetProductAsync()
        {
            try
            {
                var result = await _httpClient.GetStringAsync(_remoteServiceBaseUrl);
                return result;
            }
            catch (HttpRequestException)
            {

                return $"Sorry, had problem accessing [{_remoteServiceBaseUrl}]";
            }
        }
    }
    internal class Login
    {
        public string emailAddress { get;set; }
        public string password { get; set; }
    }
}
