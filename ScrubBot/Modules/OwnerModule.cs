using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Domain;
using ScrubBot.Extensions;

using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    [RequireOwner]
    public class OwnerModule : Module
    {
        public OwnerModule(Tools tools) : base(tools)
        {

        }

        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")]
        public async Task UrMomGay()
        {
            await ReplyAsync($"{Context.Message.Author.Mention} No u");
        }

        [Command("ChangeGame"), Summary("You do not have access to this command")]
        public async Task ChangeGame([Remainder]string newGame)
        {
            await Context.Client.SetGameAsync(newGame);

            EmbedBuilder embedBuilder = new EmbedBuilder()
                .CreateSuccess("Done")
                .AddField("Change Game:", $"Changed active game from {Context.Client.CurrentUser.Game} to {newGame}");

            await ReplyAsync(embedBuilder);
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

            if (!newPrefix.EndsWith(" "))
            {
                newPrefix += " ";
            }

            string old = Guild.Prefix;

            Guild.Prefix = newPrefix;
            await Tools.Prefix.SetAsync(Guild.Id, newPrefix).ConfigureAwait(false);

            await ReplyAsync(new EmbedBuilder().CreateSuccess("Changed Command String Prefix from ' {old} ' to ' {newPrefix} '"));
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task SetAuditChannel([Remainder]SocketTextChannel newChannel)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "CANNOT PERFORM ACTION" };
                errorEmbed.Description = $"Admin commands are only allowed in the audit channel ({auditChannel.Mention})\nAborting operation";
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            Guild.AuditChannelId = newChannel.Id;

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Set this server's audit channel to {newChannel.Mention}"));
        }
    }
}
