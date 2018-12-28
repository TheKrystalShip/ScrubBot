using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Database.Domain;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator, Group = nameof(AdminModule))]
    [RequireOwner(Group = nameof(AdminModule))]
    public class AdminModule : Module
    {
        public AdminModule()
        {

        }

        [Command("UrMomGay"), Summary("( ͡° ͜ʖ ͡°)")]
        public async Task<RuntimeResult> UrMomGay()
        {
            return new SuccessResult($"{Context.User.Mention} No u");
        }

        [Command("HelloThere")]
        public async Task<RuntimeResult> HelloThere()
        {
            return new SuccessResult($"General {Context.User.Username} ⚔️⚔️");
        }
        
        [Command("SetPrefix"), Summary("Change this server's current command character prefix")]
        public async Task<RuntimeResult> SetPrefix(string newPrefix)
        {
            string old = Guild.Prefix;
            await Prefix.SetAsync(Guild.Id, newPrefix);

            return new SuccessResult($"Changed Command Char Prefix from '{old}' to '{newPrefix}'");
        }
        
        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task<RuntimeResult> SetAuditChannel(SocketTextChannel newChannel)
        {
            SocketGuild newChannelGuild = newChannel.Guild;

            // Don't know how often this can happen, or if it _can_ happen at all, but just in case
            if (Context.Guild != newChannelGuild)
            {
                return new ErrorResult(CommandError.Exception, "Cannot set the audit channel of this guild to a channel from a different guild");
            }

            Guild.AuditChannelId = newChannel.Id;

            return new SuccessResult($"Set this server's audit channel to {newChannel.Mention}");
        }
    }
}
