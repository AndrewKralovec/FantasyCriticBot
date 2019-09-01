
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FantasyBot.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FantasyBot
{
    /// <summary>
    /// Service class for handling discord bot commands.
    /// </summary>
    public class CommandHandler
    {
        readonly IConfigurationRoot _config;
        readonly CommandService _commands;
        readonly DiscordSocketClient _client;
        readonly IServiceProvider _services;
        readonly string _prefix;

        /// <summary>
        /// The <c>CommandHandler</c> class constructor. Retrieve client and CommandService instance to setup the handlers.
        /// </summary>
        /// <param name="services"></param>
        public CommandHandler(IServiceProvider services)
        {
            // Define services
            _config = services.GetRequiredService<IConfigurationRoot>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _prefix = _config[Constants.ConfigPrefix];

            if (string.IsNullOrWhiteSpace(_prefix))
                throw new Exception($"{Constants.ConfigPrefix} is missing from -> [{Constants.AppSettings}]");

            // Handle closing action.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Handle message action, prevent bad commands.
            _client.MessageReceived += MessageReceivedAsync;
        }

        /// <summary>
        /// Load the Discord command modules and inject the services.
        /// </summary>
        /// <returns>The command modules</returns>
        public async Task InitializeAsync() 
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        /// <summary>
        /// Message handler. Attempts to exectue a requested command.
        /// </summary>
        /// <param name="rawMessage">Message</param>
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

        /// <summary>
        /// Execute the command with the command context. Handle any Exceptions with the command.
        /// </summary>
        /// <param name="command">Bot command</param>
        /// <param name="context">Context of the command</param>
        /// <param name="result">Result of the command</param>
        /// <returns></returns>
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
