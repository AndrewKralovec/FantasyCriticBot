using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace FantasyBot
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        public CriticService _client { get; set; }

        [Command("say")]
        [Summary("Echo back a command. For testing.")]
        public Task SayAsync(string echo)
            => ReplyAsync(echo);
    }
}
