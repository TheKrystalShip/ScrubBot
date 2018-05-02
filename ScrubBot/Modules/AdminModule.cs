using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("UrMomGay")] public async Task UrMomGay() => await ReplyAsync($"{Context.Message.Author.Mention} No u");

        [Command("FuckOff")] public async Task FuckOff() => await ReplyAsync("Na fam");
    }
}