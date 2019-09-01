﻿
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using FantasyBot.Models;
using System;
using System.Linq;

namespace FantasyBot
{
    /// <summary>
    /// Service class for notifying users about League info.
    /// </summary>
    public class NotificationService : Scheduler
    {
        readonly DiscordSocketClient _client;
        readonly FantasyCriticService _criticService;
        readonly TimeSpan _daySpan = TimeSpan.FromHours(24);
        DateTime _notificationTime;
        readonly string _notificationTitle;

        /// <summary>
        /// The <c>NotificationService</c> class constructor. Setup services.
        /// </summary>
        /// <param name="services">App/Service configurations </param>
        /// 
        public NotificationService(IServiceProvider services) : base()
        {
            // Define services
            _client = services.GetRequiredService<DiscordSocketClient>();
            _criticService = services.GetRequiredService<FantasyCriticService>();
            _notificationTitle = "Fantasy Critic Notification";
            _notificationTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0).Add(_daySpan);
        }

        /// <summary>The time when the notification will be announced. Make configurable later...</summary>
        /// <value>Tommorrow at 06:00 AM</value>
        public DateTime NotificationTime
        {
            get => _notificationTime;
            set => _notificationTime = value;
        }

        /// <summary>
        /// Add a task with the offset from now and when notifications will be invoke. 
        /// </summary>
        /// <param name="leagueId">Task Key</param>
        public void AddNotification(string leagueId)
        {
            // The time span until <c>NotificationTime</c>.
            var offset = NotificationTime - DateTime.Now;
            ScheduleTask(leagueId, ReleaseNotification, offset, _daySpan);
        }

        /// <summary>
        /// (Currently) The only <c>Notification</c> task.
        /// Notify channel about the League game releases.
        /// </summary>
        /// <param name="state">Null</param>
        /// <returns>Sends Message to channel</returns>
        public async void ReleaseNotification(object state)
        {
            var games = await _criticService.GetLeagueGameReleases();
            var releases = games
                .Select(game => $"{game.GameName} will be released: {game.FormatedDate}")
                .ToArray();

            var msg = $"{_notificationTitle}\n" + String.Join(".\n", releases);

            // Gets first available channel, this will be made configurable.
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
