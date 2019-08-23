using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FantasyBot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FantasyBot
{
    public class Program
    {
        static IConfigurationRoot Config { get; set; }
        static string Token => Config[Constants.ConfigID];
        DiscordSocketClient _client;
        static void Main()
            => new Program().MainAsync().GetAwaiter().GetResult();

        public Program()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable(Constants.Env);
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == Constants.Dev;

            // Get the settings file. 
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile(Constants.AppSettings,
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
                await services.GetRequiredService<FantasyCriticService>().InitializeAsync();

                await Task.Delay(-1);
            }
        }

        Task LogAsync(LogMessage log)
            => Task.Run(() => Console.WriteLine(log.ToString()));

        Task ReadyAsync()
            => Task.Run(() => Console.WriteLine($"Connected as -> [{_client.CurrentUser}] :)"));

        ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(Config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<FantasyCriticService>()
                .BuildServiceProvider();
        }

    }
}