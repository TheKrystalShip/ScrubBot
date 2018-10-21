using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Domain;
using ScrubBot.Extensions;

using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator, Group = nameof(AdminModule)), RequireOwner(Group = nameof(AdminModule))]
    public class AdminModule : Module
    {
        public AdminModule()
        {

        }

        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")]
        public async Task UrMomGay()
        {
            await ReplyAsync($"{Context.User.Mention} No u");
        }

        [Command("HelloThere")]
        public async Task HelloThere() => await ReplyAsync("General Kenobi!");

        [Command("SetPrefix"), Summary("Change this server's current command character prefix")]
        public async Task SetPrefix(string newPrefix)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                await ReplyAsync(string.Empty, false, new EmbedBuilder().CreateError($"Admin commands are only allowed in the audit channel ({auditChannel.Mention})").Build());
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
                await ReplyAsync(string.Empty, false, new EmbedBuilder().CreateError($"Admin commands are only allowed in the audit channel ({auditChannel.Mention})").Build());
                return;
            }

            Guild.AuditChannelId = newChannel.Id;

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Set this server's audit channel to {newChannel.Mention}"));
        }
    }
}