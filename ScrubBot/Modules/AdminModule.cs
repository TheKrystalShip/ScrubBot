using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Models;
using ScrubBot.Handlers;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("UrMomGay")] public async Task UrMomGay() => await ReplyAsync($"{Context.Message.Author.Mention} No u");

        [Command("ChangeCharPrefix")]
        public async Task ChengeCharPrefix(string newPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                await ReplyAsync($"```Current guild was not found in the database...\nAborting operation```");
                return;
            }

            string old = guild.CharPrefix;
            guild.CharPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetCharPrefix(guild.Id, newPrefix);
            await ReplyAsync($"`Changing Command Char Prefix from {old} to {newPrefix}`");
        }

        [Command("ChangeStringPrefix")]
        public async Task ChengeStringPrefix([Remainder]string newPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                await ReplyAsync($"```Current guild was not found in the database...\nAborting operation```");
                return;
            }

            if (!newPrefix.EndsWith(" ")) newPrefix += " ";

            string old = guild.StringPrefix;
            guild.StringPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetStringPrefix(guild.Id, newPrefix);
            await ReplyAsync($"`Changing Command String Prefix from {old} to {newPrefix}`");
        }

        [Command("SetAuditChannel")]
        public async Task SetAuditChannel([Remainder]SocketGuildChannel newChannel)
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                await ReplyAsync($"```Current guild was not found in the database...\nAborting operation```");
                return;
            }

            guild.AuditChannelId = newChannel.Id.ToString();
            db.SaveChanges();
            await ReplyAsync($"```Successfully changed ScrubBot's Audit Channel to {newChannel.Name} for {newChannel.Guild.Name}!```");
        }

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

            await ReplyAsync("Adding `Audit Channel` field to EmbedBuilder embed completed");
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
    }
}