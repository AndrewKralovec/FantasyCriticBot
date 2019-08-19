using Discord.Commands;
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
            var data = await Client.GetPlayerStandings();
            await ReplyAsync(data);
        }
    }
}
