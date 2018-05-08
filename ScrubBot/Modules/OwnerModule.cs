using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ScrubBot.Properties;

namespace ScrubBot.Modules
{
    [RequireOwner]
    public class OwnerModule : ModuleBase<SocketCommandContext>
    {
        [Command("ChangeGame"), Summary("You do not have access to this command.")]
        public async Task ChangeGame([Remainder]string newGame)
        {
            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success" };
            embed.AddField("Change Game:", $"Changed active game from {Context.Client.CurrentUser.Game} to {newGame}");
            await Context.Client.SetGameAsync(newGame);
            await ReplyAsync("", false, embed.Build());
        }
    }
}