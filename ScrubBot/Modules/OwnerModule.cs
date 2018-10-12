using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Database.Models;
using ScrubBot.Extensions;

using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    [RequireOwner]
    public class OwnerModule : Module
    {
        public OwnerModule(Tools tools) : base(tools)
        {
            Guild = Tools.Database.Guilds.Find(Context.Guild.Id);
            User = Tools.Database.Users.Find(Context.User.Id);
        }

        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")]
        public async Task UrMomGay()
        {
            await ReplyAsync($"{Context.Message.Author.Mention} No u");
        }

        [Command("ChangeGame"), Summary("You do not have access to this command")]
        public async Task ChangeGame([Remainder]string newGame)
        {
            EmbedBuilder embed = new EmbedBuilder().CreateSuccess("Done");

            embed.AddField("Change Game:", $"Changed active game from {Context.Client.CurrentUser.Game} to {newGame}");

            await Context.Client.SetGameAsync(newGame);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetStringPrefix"), Summary("Change this server's current command string prefix")]
        public async Task SetStringPrefix([Remainder]string newPrefix)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "CANNOT PERFORM ACTION" };
                errorEmbed.Description = $"Admin commands are only allowed in the audit channel ({auditChannel.Mention})\nAborting operation";
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            if (!newPrefix.EndsWith(" ")) newPrefix += " ";

            string old = Guild.Prefix;

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Title = "Success", Description = $"Changed Command String Prefix from ' {old} ' to ' {newPrefix} '" };

            Guild.Prefix = newPrefix;
            await Tools.Prefix.SetAsync(Guild.Id, newPrefix);

            await ReplyAsync("", false, embed.Build());
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task SetAuditChannel([Remainder]SocketTextChannel newChannel)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "CANNOT PERFORM ACTION"};
                errorEmbed.Description = $"Admin commands are only allowed in the audit channel ({auditChannel.Mention})\nAborting operation";
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Green, Description = "Success" };
            embed.AddField("Audit Channel:", $"Set this server's audit channel to {newChannel.Mention}");

            Guild.AuditChannelId = newChannel.Id;

            await ReplyAsync("", false, embed.Build());
        }
    }
}