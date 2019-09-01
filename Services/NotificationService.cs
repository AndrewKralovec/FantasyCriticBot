
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
    public class NotificationService : Scheduler
    {
        readonly DiscordSocketClient _client;
        readonly TimeSpan _daySpan = TimeSpan.FromHours(24);
        public NotificationService(IServiceProvider services) : base()
        {
            // Define services
            _client = services.GetRequiredService<DiscordSocketClient>();
        }

        /// <summary>The time when the notification will be announced. Make configurable later...</summary>
        /// <value>Tommorrow at 06:00 AM</value>
        DateTime NotificationTime
        {
            get => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0).Add(_daySpan);
        }

        public void AddNotification(string leagueId)
        {
            var offset = NotificationTime - DateTime.Now;
            ScheduleTask(leagueId, ReleaseNotification, offset, _daySpan);
        }

        public void ReleaseNotification(object state)
        {
            throw new NotImplementedException();
        }
    }
}
