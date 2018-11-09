using System.Linq;
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
                await ReplyAsync(new EmbedBuilder().CreateError($"Admin commands are only allowed in the audit channel ({auditChannel.Mention})"));
                return;
            }

            string old = Guild.Prefix;
            await Prefix.SetAsync(Guild.Id, newPrefix);

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Changed Command Char Prefix from ' {old} ' to ' {newPrefix} '"));
        }

        [Command("SetAuditChannel"), Summary("Change this server's current audit channel")]
        public async Task SetAuditChannel(SocketTextChannel newChannel)
        {
            SocketTextChannel auditChannel;
            if ((auditChannel = Context.Guild.GetTextChannel(Guild.AuditChannelId)) != null && Context.Channel.Id != Guild.AuditChannelId)
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"Admin commands are only allowed in the audit channel ({auditChannel.Mention})"));
                return;
            }

            Guild.AuditChannelId = newChannel.Id;

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Set this server's audit channel to {newChannel.Mention}"));
        }

        [Command("test")]
        public async Task TestAsync()
        {
            Event _event = Database.Events.FirstOrDefault();

            var author = Context.Client.GetUser(_event.Author.Id);

            if (author is null)
            {
                await ReplyAsync("Event author is null for some f-ing reason");
            }
            else
                await ReplyAsync(new EmbedBuilder().CreateSuccess($"Title\t{_event.Title}\n" +
                                                                  $"Author\t{_event.Author.Username}\n" +
                                                                  $"Description\t{_event.Description}\n"+
                                                                  $"Guild\t{_event.Guild.Name}\n" +
                                                                  $"CreationDate\t{_event.CreationDate}\n" +
                                                                  $"OccurenceDate\t{_event.OccurenceDate}\n" +
                                                                  $"Subscribers\t{_event.Subscribers.Count}\n" +
                                                                  $"MaxSubscribers\t{_event.MaxSubscribers}"));
        }
    }
}
