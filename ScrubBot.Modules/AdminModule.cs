using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Core.Commands;
using ScrubBot.Database.Domain;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator, Group = nameof(AdminModule))]
    [RequireOwner(Group = nameof(AdminModule))]
    public class AdminModule : Module
    {
        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")]
        public async Task<RuntimeResult> UrMomGay()
        {
            return Result.Success($"{Context.User.Mention} No u");
        }

        [Command("HelloThere")]
        public async Task<RuntimeResult> HelloThere()
        {
            return Result.Success("General Kenobi!");
        }

        [Command("success")]
        public async Task<RuntimeResult> Success()
        {
            return Result.Success("Success result");
        }

        [Command("info")]
        public async Task<RuntimeResult> Info()
        {
            return Result.Info("Information result");
        }

        [Command("error")]
        public async Task<RuntimeResult> Error()
        {
            return Result.Error("Error result");
        }

        [Command("SetPrefix"), Summary("Change this server's current command character prefix")]
        public async Task<RuntimeResult> SetPrefix(string newPrefix)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                return Result.Error($"Admin commands are only allowed in the audit channel ({auditChannel.Mention})");
            }

            string old = Guild.Prefix;
            await Prefix.SetAsync(Guild.Id, newPrefix);

            return Result.Success($"Changed Command Char Prefix from ' {old} ' to ' {newPrefix} '");
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task<RuntimeResult> SetAuditChannel(SocketTextChannel newChannel)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                return Result.Error($"Admin commands are only allowed in the audit channel ({auditChannel.Mention})");
            }

            Guild.AuditChannelId = newChannel.Id;

            return Result.Success($"Set this server's audit channel to {newChannel.Mention}");
        }
    }
}
