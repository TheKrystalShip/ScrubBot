using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Domain;
using ScrubBot.Extensions;

using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : Module
    {
        public AdminModule(Tools tools) : base(tools)
        {

        }

        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")]
        public async Task UrMomGay()
        {
            await ReplyAsync($"{Context.Message.Author.Mention} No u");
        }

        [Command("SetPrefix"), Summary("Change this server's current command character prefix")]
        public async Task SetCharPrefix(string newPrefix)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "CANNOT PERFORM ACTION" };
                errorEmbed.Description = $"Admin commands are only allowed in the audit channel ({auditChannel.Mention})\nAborting operation";

                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            string old = Guild.Prefix;

            await Tools.Prefix.SetAsync(Guild.Id, newPrefix);

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Changed Command Char Prefix from ' {old} ' to ' {newPrefix} '"));
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task SetAuditChannel(SocketTextChannel newChannel)
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
