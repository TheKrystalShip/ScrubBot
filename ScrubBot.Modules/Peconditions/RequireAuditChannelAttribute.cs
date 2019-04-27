using System;
using System.Threading.Tasks;

using Discord.Commands;

using ScrubBot.Database;
using ScrubBot.Database.Domain;

namespace ScrubBot.Modules.Peconditions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequireAuditChannelAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            IDbContext dbContext = (IDbContext) services.GetService(typeof(IDbContext));

            Guild guild = dbContext.Guilds.Find(context.Guild.Id);

            if (guild is null)
            {
                return Task.FromResult(PreconditionResult.FromError("Guild is null"));
            }

            if (guild.AuditChannelId is null)
            {
                return Task.FromResult(PreconditionResult.FromError("Audit channel is null"));
            }

            if (guild.AuditChannelId != context.Channel.Id)
            {
                return Task.FromResult(PreconditionResult.FromError("Not in audit channel"));
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
