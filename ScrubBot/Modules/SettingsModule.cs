using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Models;

namespace ScrubBot.Modules
{
    public class SettingsModule  : ModuleBase<SocketCommandContext>
    {
        [Command("Info"), Alias("BotInfo")]
        public async Task Info()
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                await ReplyAsync($"```Current guild was not found in the database...\nAborting operation```");
                return;
            }
            
            EmbedBuilder embed = new EmbedBuilder { Color = Color.Magenta, Title = "Bot Info"};
            embed.AddField("Server:", guild.Name ?? "null");

            if (guild.AuditChannelId != null)
            {
                var auditChannel = Context.Guild.GetChannel(Convert.ToUInt64(guild.AuditChannelId)) as SocketTextChannel;
                embed.AddField("Audit Channel:", auditChannel != null ? auditChannel.Mention : "Invalid channel!");
            } 
            else
            {
                embed.AddField("Audit Channel:", "null");
            }
            
            embed.AddField("Char prefix:", guild.CharPrefix ?? "null");
            embed.AddField("String prefix:", guild.StringPrefix ?? "null");

            await ReplyAsync("", false, embed.Build());
        }

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