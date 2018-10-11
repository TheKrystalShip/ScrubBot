using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Database;
using ScrubBot.Database.Models;
using ScrubBot.Handlers;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly PrefixHandler _prefixHandler;
        private readonly DatabaseContext _db;

        public AdminModule(PrefixHandler prefixHandler, DatabaseContext dbContext)
        {
            _prefixHandler = prefixHandler;
            _db = dbContext;
        }

        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")]
        public async Task UrMomGay()
        {
            await ReplyAsync($"{Context.Message.Author.Mention} No u");
        }

        [Command("SetCharPrefix"), Summary("Change this server's current command character prefix")]
        public async Task SetCharPrefix(string newPrefix)
        {
            if (!GetGuild(out Guild guild))
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

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success", Description = $"Changed Command Char Prefix from ' {old} ' to ' {newPrefix} '"};

            guild.CharPrefix = newPrefix;
            _db.Guilds.Update(guild);
            await _db.SaveChangesAsync();
            _prefixHandler.SetCharPrefix(guild.Id, newPrefix);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetStringPrefix"), Summary("Change this server's current command string prefix")]
        public async Task SetStringPrefix([Remainder]string newPrefix)
        {
            if (!GetGuild(out Guild guild))
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

            if (!newPrefix.EndsWith(" ")) newPrefix += " ";

            string old = guild.StringPrefix;

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success", Description = $"Changed Command String Prefix from ' {old} ' to ' {newPrefix} '" };

            guild.StringPrefix = newPrefix;
            _db.Guilds.Update(guild);
            await _db.SaveChangesAsync();
            _prefixHandler.SetStringPrefix(guild.Id, newPrefix);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task SetAuditChannel([Remainder]SocketTextChannel newChannel)
        {
            if (!GetGuild(out Guild guild))
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
            embed.AddField("Audit Channel:", $"Set this server's audit channel to {newChannel.Mention}");

            guild.AuditChannelId = newChannel.Id.ToString();
            _db.SaveChanges();
            await ReplyAsync("", false, embed.Build());
        }

        private bool GetGuild(out Guild outGuild)
        {
            string guildId = Context.Guild.Id.ToString();
            Guild localGuild = _db.Guilds.FirstOrDefault(x => x.Id == guildId);

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