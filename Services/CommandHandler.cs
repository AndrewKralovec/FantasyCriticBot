using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace FantasyBot
{
    public class CommandHandler
    {
        private readonly IConfigurationRoot _config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly string _prefix;

        public CommandHandler(IServiceProvider services)
        {
            // Define services
            _config = services.GetRequiredService<IConfigurationRoot>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _prefix = _config["Bot:Prefix"];

            if (string.IsNullOrWhiteSpace(_prefix))
                throw new Exception("Bot:Prefix missing from -> [appsettings.json]");

            // Handle closing action.
            _commands.CommandExecuted += CommandExecutedAsync;

            // Handle message action, prevent bad commands.
            _client.MessageReceived += MessageReceivedAsync;

        }

        public async Task InitializeAsync() 
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message))
                return;

            if (message.Source != MessageSource.User)
                return;

            var argPos = 0;
            var prefix = char.Parse(_prefix);

            // Check if valid context
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
                return;

            // Execute command
            var context = new SocketCommandContext(_client, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // Command not found
            if (!command.IsSpecified)
            {
                System.Console.WriteLine($"Command failed to execute for [{context.User.Username}] -> [{result.ErrorReason}]!");
                return;
            }
            // Command Success
            if (result.IsSuccess)
            {
                System.Console.WriteLine($"Command [{command.Value.Name}] executed for -> [{context.User.Username}]");
                return;
            }

            // Error 
            await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
        }
    }
}
