using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace FantasyBot
{
    public class Program
    {
        public static IConfigurationRoot Config { get; set; }
        private static string Token => Config[Constants.ClientID];
        private DiscordSocketClient _client;
        static void Main()
            => new Program().MainAsync().GetAwaiter().GetResult();

        public Program()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable(Constants.Env);
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == Constants.Dev;

            // Get `appsettings.json`.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true);

            // Get the TOKEN for secrests, should store else where on prod.
            if (isDevelopment)
                builder.AddUserSecrets<Program>();

            Config = builder.Build();
        }

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                // Get/Set client clients.
                var client = services.GetRequiredService<DiscordSocketClient>();
                _client = client;

                // Handle log action events.
                client.Log += LogAsync;
                client.Ready += ReadyAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                // Get start bot.
                await client.LoginAsync(TokenType.Bot, Token);
                await client.StartAsync();

                // Call the InitializeAsync to start the CommandHandler service. 
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(-1);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"Connected as -> [{_client.CurrentUser}] :)");
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(Config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<CriticService>()
                .BuildServiceProvider();
        }

    }
    static class Constants
    {
        public const string ClientID = "Bot:ClientID";
        public const string Dev = "development";
        public const string Env = "NETCORE_ENVIRONMENT";
        public const string JsonContent = "application/json";
    }
}