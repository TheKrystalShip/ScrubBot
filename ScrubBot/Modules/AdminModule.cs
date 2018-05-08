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
        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")] public async Task UrMomGay() => await ReplyAsync($"{Context.Message.Author.Mention} No u");

        [Command("SetCharPrefix"), Summary("Change this server's current command character prefix.")]
        public async Task SetCharPrefix(string newCharPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "ERROR", Description = "Current guild was not found in the database...\nAborting operation"};
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            if (Context.Guild.GetTextChannel(Convert.ToUInt64(guild.AuditChannelId)) != null && guild.AuditChannelId != null && Context.Channel.Id.ToString() != guild.AuditChannelId)
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "CANNOT PERFORM ACTION" };
                var auditChannel = Context.Guild.GetChannel(Convert.ToUInt64(guild.AuditChannelId)) as SocketTextChannel;
                errorEmbed.Description = $"Admin commands are only allowed in the audit channel ({auditChannel.Mention})\nAborting operation";
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            string old = guild.CharPrefix;

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success", Description = $"Changed Command Char Prefix from ' {old} ' to ' {newCharPrefix} '"};

            guild.CharPrefix = newCharPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetCharPrefix(guild.Id, newCharPrefix);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetStringPrefix"), Summary("Change this server's current command string prefix.")]
        public async Task SetStringPrefix([Remainder]string newStringPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "ERROR", Description = "Current guild was not found in the database...\nAborting operation" };
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            if (Context.Guild.GetTextChannel(Convert.ToUInt64(guild.AuditChannelId)) != null && guild.AuditChannelId != null && Context.Channel.Id.ToString() != guild.AuditChannelId)
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "CANNOT PERFORM ACTION" };
                var auditChannel = Context.Guild.GetChannel(Convert.ToUInt64(guild.AuditChannelId)) as SocketTextChannel;
                errorEmbed.Description = $"Admin commands are only allowed in the audit channel ({auditChannel.Mention})\nAborting operation";
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            if (!newStringPrefix.EndsWith(" ")) newStringPrefix += " ";

            string old = guild.StringPrefix;

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success", Description = $"Changed Command String Prefix from ' {old} ' to ' {newStringPrefix} '" };

            guild.StringPrefix = newStringPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetStringPrefix(guild.Id, newStringPrefix);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel.")]
        public async Task SetAuditChannel([Remainder]SocketTextChannel newAuditChannel)
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "ERROR", Description = "Current guild was not found in the database...\nAborting operation"};
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            if (Context.Guild.GetTextChannel(Convert.ToUInt64(guild.AuditChannelId)) != null && guild.AuditChannelId != null && Context.Channel.Id.ToString() != guild.AuditChannelId)
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "CANNOT PERFORM ACTION"};
                var auditChannel = Context.Guild.GetChannel(Convert.ToUInt64(guild.AuditChannelId)) as SocketTextChannel;
                errorEmbed.Description = $"Admin commands are only allowed in the audit channel ({auditChannel.Mention})\nAborting operation";
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Description = "Success" };
            embed.AddField("Audit Channel:", $"Set this server's audit channel to {newAuditChannel.Mention}");

            guild.AuditChannelId = newAuditChannel.Id.ToString();
            db.SaveChanges();
            await ReplyAsync("", false, embed.Build());
        }

        [Command("RemoveAuditChannel"), Summary("Remove this server's audit channel alltogether, allowing admin commands anywhere.")]
        public async Task RemoveAuditChannel()
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "ERROR", Description = "Current guild was not found in the database...\nAborting operation"};
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Description = "Success (?)" };
            embed.AddField("Audit Channel:", "Removed this server's audit log.\nEnjoy posting admin commands EVERYWHERE!");

            guild.AuditChannelId = null;
            db.SaveChanges();
            await ReplyAsync("", false, embed.Build());
        }

        private bool GetGuild(DatabaseContext dbContext, out Guild outGuild)
        {
            outGuild = null;
            string guildId = Context.Guild.Id.ToString();
            Guild localGuild = dbContext.Guilds.FirstOrDefault(x => x.Id == guildId);

            if (localGuild == null) return false;

            outGuild = localGuild;
            return true;
        }
    }
}