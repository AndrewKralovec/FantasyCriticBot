using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FantasyBot
{
    public class FantasyCriticService
    {
        static readonly HttpClient _httpClient;
        static readonly UriBuilder _remoteServiceBaseUrl;
        static readonly UriBuilder _remoteServiceLoginUrl;

        static FantasyCriticService()
        {
            // Define a persistant session.
            // Methods use the shared instance of HttpClient for every call.
            _httpClient = new HttpClient(
                new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    UseCookies = true,
                    CookieContainer = new CookieContainer()
                });
            _remoteServiceBaseUrl = new UriBuilder("https://www.fantasycritic.games/api/League/GetLeagueYear");
            _remoteServiceLoginUrl = new UriBuilder("https://www.fantasycritic.games/api/account/login");
        }
        public async Task InitializeAsync()
        {
            // Setup login Json. 
            var creds = new Login
            {
                emailAddress = "",
                password = ""
            };
            var payload = JsonConvert.SerializeObject(creds);
            var requestContent = new StringContent(payload, Encoding.UTF8, Constants.JsonContent);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.JsonContent); // Might want to move to handler.

            // Post login credentials
            // Using might be over kill now that it has a smaller scope. 
            using (var response = await _httpClient.PostAsync(_remoteServiceLoginUrl.Uri, requestContent))
            {
                response.EnsureSuccessStatusCode();
                var temp = await response.Content.ReadAsStringAsync();
                var tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                var bearerToken = tokenDetails["token"];

                _httpClient.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue("Bearer", bearerToken);
            }
        }
        async Task<JObject> GetLeagueJson()
        {
            var query = HttpUtility.ParseQueryString(_remoteServiceBaseUrl.Query);
            query["leagueID"] = "";
            query["year"] = "2019";
            _remoteServiceBaseUrl.Query = query.ToString();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _remoteServiceBaseUrl.Uri))
            {
                var result = await _httpClient.SendAsync(requestMessage);
                var resultContent = await result.Content.ReadAsStringAsync();
                return JObject.Parse(resultContent);
            };
        }

        public async Task<string> GetPlayerStandings()
        {
            try
            {
                var googleSearch = await GetLeagueJson();
                var results = googleSearch["players"].Children()
                    .Select(player => player.ToObject<PlayerJson>())
                    .Select(player => player.publisher)
                    .Select(pub => $"{pub.publisherName} has {pub.totalFantasyPoints} total points")
                    .ToArray();

                return String.Join(".\n", results);
            }
            catch (Exception)
            {
                return $"Sorry, had problem accessing -> [{_remoteServiceBaseUrl.ToString()}]";
            }

        }
    }
    internal class Login
    {
        public string emailAddress { get; set; }
        public string password { get; set; }
    }
}
