
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using FantasyBot.Models;
using System;
using System.Linq;

namespace FantasyBot
{
    public class NotificationService : Scheduler
    {
        readonly DiscordSocketClient _client;
        readonly FantasyCriticService _criticService;
        readonly TimeSpan _daySpan = TimeSpan.FromHours(24);
        readonly string _notificationTitle;
        public NotificationService(IServiceProvider services) : base()
        {
            // Define services
            _client = services.GetRequiredService<DiscordSocketClient>();
            _criticService = services.GetRequiredService<FantasyCriticService>();
            _notificationTitle = "Fantasy Critic Notification";
        }

        /// <summary>The time when the notification will be announced. Make configurable later...</summary>
        /// <value>Tommorrow at 06:00 AM</value>
        DateTime NotificationTime
        {
            get => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0).Add(_daySpan);
        }

        public void AddNotification(string leagueId)
        {
            // The time span until <c>NotificationTime</c>.
            var offset = NotificationTime - DateTime.Now;
            ScheduleTask(leagueId, ReleaseNotification, offset, _daySpan);
        }

        public async void ReleaseNotification(object state)
        {
            var games = await _criticService.GetLeagueGameReleases();
            var releases = games
                .Select(game => $"{game.GameName} will be released: {game.FormatedDate}")
                .ToArray();

            var msg = $"{_notificationTitle}\n" + String.Join(".\n", releases);

            var channel = _client
                .Guilds
                .FirstOrDefault()
                .Channels
                .Select(c => _client.GetChannel(c.Id) as IMessageChannel)
                .OfType<IMessageChannel>()
                .FirstOrDefault();
    
            await channel.SendMessageAsync(msg);
        }
    }
}
