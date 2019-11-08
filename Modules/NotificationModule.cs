using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyBot
{
    public class NotificationModule : ModuleBase<SocketCommandContext>
    {
        public FantasyCriticService Client { get; set; }
        public ReleasesService Notifications { get; set; }

        [Command("watch")]
        [Summary("Add a league to the Notification service")]
        public async Task Watch(string id = "")
        {
            var nTime = Notifications.NotificationTime;
            var msg = "Notification scheduling time not set";
            if(String.IsNullOrEmpty(id))
                id = Client.LeagueID;
                        
            if (nTime != null)
            {
                Notifications.AddNotification(id);
                msg = $"You are now watching, {id}.\n" +
                    "You will now receive be notified everyday of league game releases.\n" +
                    $"The next notification will be announced at, {Notifications.NotificationTime}.";
            }
            await ReplyAsync(msg);
        }

        [Command("change_schedule")]
        [Summary("Change Notification scheduling time")]
        public async Task SetNotificationTime(string dateInput = "", string id = "")
        {
            if (String.IsNullOrEmpty(id))
                id = Client.LeagueID;

            if (String.IsNullOrEmpty(dateInput))
            {
                await ReplyAsync("Date input was empty. Please enter a valid date");
                return;
            }

            var date = DateTime.Parse(dateInput);
            if (DateTime.Now > date)
            {
                await ReplyAsync($"Date, {date.ToString()}, is in the past. It must be a future date");
                return;
            }

            var updated = Notifications.ChangeNotificationTime(date, id);
            var msg = $"Was unable to change the notification scheduling time";
            if (updated)
            {
                msg = ":bell: Notification settings have been updated.\n" +
                    $"Notifications will be announced at, {Notifications.NotificationTime}.";
            }
            await ReplyAsync(msg);
        }
    }
}
