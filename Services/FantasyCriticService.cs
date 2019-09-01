using FantasyBot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    /// <summary>
    /// Service class for getting Fantasy Critic data from their API.
    /// </summary>
    public class FantasyCriticService
    {
        static readonly HttpClient _httpClient;
        static readonly UriBuilder _remoteServiceBaseUrl;
        static readonly UriBuilder _remoteServiceLoginUrl;
        readonly IConfigurationRoot _config;
        public string LeagueID { get; set; } = "";

        /// <summary>
        /// The static <c>FantasyCriticService</c> class constructor. Sets a shared instance of HttpClient for the class.
        /// </summary>
        static FantasyCriticService()
        {
            // Define a persistant session.
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

        /// <summary>
        /// The <c>FantasyCriticService</c> class constructor. Setup services.
        /// </summary>
        /// <param name="services">App/Service configurations </param>
        public FantasyCriticService(IServiceProvider services)
        {
            // Define services
            _config = services.GetRequiredService<IConfigurationRoot>();

            // Check if user wants to set LeagueID on startup
            if (!string.IsNullOrWhiteSpace(_config[Constants.ConfigLeagueID]))
                LeagueID = _config[Constants.ConfigLeagueID];

        }

        /// <summary>
        /// Request and set the authentication token using the user credentials.
        /// </summary>
        /// <exception cref="FantasyBot.FantasyRequestException">Thrown when there is a login error</exception>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            try
            {
                // Setup the JSON login content.
                var requestContent = new StringContent(JsonConvert.SerializeObject(value:
                    new LoginJson
                    {
                        emailAddress = _config[Constants.ConfigEmail],
                        password = _config[Constants.ConfigPassword]
                    }), Encoding.UTF8, Constants.JsonContent);

                requestContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.JsonContent); // Might want to move to handler.

                // Post login credentials.
                // Using might be over kill now that it has a smaller scope. 
                using (var response = await _httpClient.PostAsync(_remoteServiceLoginUrl.Uri, requestContent))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    var bearerToken = tokenDetails["token"];

                    _httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", bearerToken);
                }
            }
            catch (Exception)
            {
                throw new FantasyRequestException($"[Login Error]: Check your email or password");
            }
        }

        /// <summary>
        /// Get and parse the League JSON from the API. 
        /// </summary>
        /// <exception cref="FantasyBot.FantasyRequestException">Thrown when there problem getting League data</exception>
        /// <returns>League JSON parsed object</returns>
        async Task<JObject> GetLeagueJson()
        {
            if (string.IsNullOrEmpty(LeagueID))
                throw new FantasyRequestException("[League Error]: No leagueID has been set, are you watching a league?");

            try
            {
                var query = HttpUtility.ParseQueryString(_remoteServiceBaseUrl.Query);
                query["leagueID"] = LeagueID;
                query["year"] = "2019"; // Move to paramters later 
                _remoteServiceBaseUrl.Query = query.ToString();

                // Call the API League endpoint, with the LeagueID and year.
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _remoteServiceBaseUrl.Uri))
                {
                    var response = await _httpClient.SendAsync(requestMessage);
                    response.EnsureSuccessStatusCode();
                    var resultContent = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(resultContent);
                };
            }
            catch (Exception ex)
            {
                throw new FantasyRequestException($"[League Error]: Error accessing {_remoteServiceBaseUrl.ToString()}, {ex.Message}");
            }
        }

        /// <summary>
        /// Get the publishers currently apart of the League.
        /// </summary>
        /// <returns>The League Publishers</returns>
        public async Task<List<PublisherJson>> GetLeaguePublishers()
        {
            var leagueSearch = await GetLeagueJson();
            var results = leagueSearch["players"].Children()
                .Select(player => player.ToObject<PlayerJson>())
                .Select(player => player.publisher)
                .ToList();

            return results;
        }

        /// <summary>
        /// Get next game, in the League that will be released (from now). 
        /// </summary>
        /// <returns>The next Game</returns>
        public async Task<GameRelease> GetNextGameRelease()
        {
            var leagueSearch = await GetLeagueJson();
            var gameResult = leagueSearch["publishers"].Children()
                .Select(publisher => publisher.ToObject<PublisherJson>())
                .Select(publisher => new GameRelease(publisher))
                .OrderBy(game => game.ReleaseDate)
                .FirstOrDefault();

            return gameResult;
        }
        
        /// <summary>
        /// Get the next game to be released for every publisher in the league. 
        /// </summary>
        /// <returns>League game releases</returns>
        public async Task<List<GameRelease>> GetLeagueGameReleases()
        {
            var leagueSearch = await GetLeagueJson();
            var gameResult = leagueSearch["publishers"].Children()
                .Select(publisher => publisher.ToObject<PublisherJson>())
                .Select(publisher => new GameRelease(publisher))
                .ToList();

            return gameResult;
        }
    }

    /// <summary>
    /// The internal <c>FantasyRequestException</c> class. In charge of any errors with the Fantasy Critic API.
    /// </summary>
    [Serializable()]
    internal class FantasyRequestException : System.Exception
    {
        public FantasyRequestException() : base() { }
        public FantasyRequestException(string message) : base(message) { }
        public FantasyRequestException(string message, System.Exception inner) : base(message, inner) { }
        protected FantasyRequestException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
