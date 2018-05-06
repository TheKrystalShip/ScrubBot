using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Models;
using ScrubBot.Properties;

namespace ScrubBot.Modules
{
    public class SettingsModule  : ModuleBase<SocketCommandContext>
    {
        //[Command("Info"), Alias("BotInfo")]
        //public async Task Info()
        //{
        //    DatabaseContext db = new DatabaseContext();

        //    if (!GetGuild(db, out Guild guild))
        //    {
        //        await OnGuildNotFound();
        //        return;
        //    }

        //    EmbedBuilder embed = new EmbedBuilder { Color = Color.Magenta, Title = "Bot Info"};
        //    embed.AddField("Server:", guild.Name ?? "null");
        //    embed.AddField("Audit Channel:", Context.Guild.GetChannel(ulong.Parse(guild.AuditChannelId)).ToString() ?? "null");
        //    embed.AddField("Char prefix:", guild.CharPrefix ?? "null");
        //    embed.AddField("String prefix:", guild.StringPrefix ?? "null");

        //    await ReplyAsync(string.Empty, false, embed.Build());
        //}

        private bool GetGuild(DatabaseContext dbContext, out Guild outGuild)
        {
            string guildId = Context.Guild.Id.ToString();
            Guild localGuild = dbContext.Guilds.FirstOrDefault(x => x.Id == guildId);

            if (localGuild == null)
            {
                outGuild = null;
                return false;
            }

            outGuild = localGuild;
            return true;
        }

        private async Task OnGuildNotFound() => await ReplyAsync($"```Current guild was not found in the database...\nAborting operation```");
    }
}