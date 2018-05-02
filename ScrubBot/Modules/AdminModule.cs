using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("IkBinDeBaas")]
        public async Task SetBotOwner(IGuildUser owner)
        {

        }

        [Command("UrMomGay")]
        public async Task UrMomGay() => await ReplyAsync("No u");
    }
}