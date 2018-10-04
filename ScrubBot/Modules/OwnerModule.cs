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
    [RequireOwner]
    public class OwnerModule : ModuleBase<SocketCommandContext>
    {
        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")] public async Task UrMomGay() => await ReplyAsync($"{Context.Message.Author.Mention} No u");

        [Command("ChangeGame"), Summary("You do not have access to this command")]
        public async Task ChangeGame([Remainder]string newGame)
        {
            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success" };
            embed.AddField("Change Game:", $"Changed active game from {Context.Client.CurrentUser.Game} to {newGame}");
            await Context.Client.SetGameAsync(newGame);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetCharPrefix"), Summary("Change this server's current command character prefix")]
        public async Task SetCharPrefix(string newPrefix)
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

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success", Description = $"Changed Command Char Prefix from ' {old} ' to ' {newPrefix} '"};

            guild.CharPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetCharPrefix(guild.Id, newPrefix);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetStringPrefix"), Summary("Change this server's current command string prefix")]
        public async Task SetStringPrefix([Remainder]string newPrefix)
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

            if (!newPrefix.EndsWith(" ")) newPrefix += " ";

            string old = guild.StringPrefix;

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success", Description = $"Changed Command String Prefix from ' {old} ' to ' {newPrefix} '" };

            guild.StringPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetStringPrefix(guild.Id, newPrefix);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task SetAuditChannel([Remainder]SocketTextChannel newChannel)
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
            embed.AddField("Audit Channel:", $"Set this server's audit channel to {newChannel.Mention}");

            guild.AuditChannelId = newChannel.Id.ToString();
            db.SaveChanges();
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