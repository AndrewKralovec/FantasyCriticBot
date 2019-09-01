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
        [Summary("Set league you want to watch for the bot")]
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
    }
}
