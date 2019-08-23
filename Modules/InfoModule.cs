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
        [Summary("Echo back a command. For testing.")]
        public Task SayAsync(string echo)
            => ReplyAsync(echo);

        [Command("standings")]
        [Summary("Get the league standings")]
        public async Task StandingsAsync()
        {
            var publishers = await Client.GetLeaguePublishers();
            var results = publishers
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
            var msg = $"Fantasycritic update!!\n {game.gameName}, will be released: {game.releaseDate.ToString()}";
            await ReplyAsync(msg);
        }
    }
}
