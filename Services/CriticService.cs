using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace FantasyBot
{
    public class CriticService
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _remoteServiceBaseUrl;

        static CriticService()
        {
            _httpClient = new HttpClient();
            _remoteServiceBaseUrl = "https://www.fantasycritic.games/api/League/GetLeagueYear";
        }

        // This method uses the shared instance of HttpClient for every call to GetProductAsync.
        public async Task<string> GetProductAsync()
        {
            var hostName = _remoteServiceBaseUrl;
            var result = await _httpClient.GetStringAsync(_remoteServiceBaseUrl);
            return "";
        }

    }
}
