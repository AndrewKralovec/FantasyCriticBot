using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyBot
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        public FantasyCriticService Client { get; set; }

        [Command("say")]
        [Summary("Echo back a command. For testing")]
        public Task SayAsync(string echo)
            => ReplyAsync(echo);

        [Command("time")]
        [Summary("Echo back what time the bot thinks it is")]
        public Task TimeAsync()
            => ReplyAsync(DateTime.Now.ToString());

        [Command("standings")]
        [Summary("Get the league standings")]
        public async Task StandingsAsync()
        {
            var results = (await Client.GetLeaguePublishers())
                .Select(pub => $"{pub.publisherName} has {pub.totalFantasyPoints} total points")
                .ToArray();

            var msg = String.Join(".\n", results);
            await ReplyAsync(msg);
        }

        [Command("next_release")]
        [Summary("Get the next game that will be released for your league")]
        public async Task NextRelease()
        {
            var game = await Client.GetNextGameRelease();
            var user = Context.Client.CurrentUser;
            var msg = $"{user.Username}, {game.GameName} will be released: {game.FormatedDate}";
            await ReplyAsync(msg);
        }

        [Command("change_league")]
        [Summary("Set league you want to watch for the bot")]
        public async Task SetLeagueID(string id)
        {
            Client.LeagueID = id;
            var msg = $"Set league id to {Client.LeagueID}";
            await ReplyAsync(msg);
        }
    }
}
