using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyBot
{
    public class NotificationModule : ModuleBase<SocketCommandContext>
    {
        public FantasyCriticService Client { get; set; }
        public NotificationService Notifications { get; set; }

        [Command("watch")]
        [Summary("Add a league to the Notification service")]
        public async Task Watch(string id = "")
        {
            if(String.IsNullOrEmpty(id))
                id = Client.LeagueID;

            Notifications.AddNotification(id);
            
            var msg = $"You are now watching, {id}.\n" + 
                "You will now receive be notified everyday of league game releases.\n" +
                $"The next notification will be announced at, {Notifications.NotificationTime}.";
            await ReplyAsync(msg);
        }

        [Command("change_schedule")]
        [Summary("Change Notification scheduling time")]
        public async Task SetNotificationTime(string dateInput = "")
        {
            var msg = "Date input was empty. Please enter a valid date";
            if (!String.IsNullOrEmpty(dateInput))
            {
                var date = DateTime.Parse(dateInput);

                if (DateTime.Now > date)
                {
                    msg = "Date cannot be in the past";
                }
                else 
                {
                    Notifications.NotificationTime = date;
                    msg = "Notification settings have been updated.\n" +
                        $"The next notification will be announced at, {Notifications.NotificationTime}.";
                }
            }
            await ReplyAsync(msg);
        }
    }
}
