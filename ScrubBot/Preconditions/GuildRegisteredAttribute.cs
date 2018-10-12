using Discord.Commands;

using Microsoft.Extensions.DependencyInjection;

using ScrubBot.Database;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Preconditions
{
    public class GuildRegisteredAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            DatabaseContext dbContext = services.GetRequiredService<DatabaseContext>();

            return Task.FromResult(dbContext.Guilds.Any(x => x.Id == context.Guild.Id) ? PreconditionResult.FromSuccess() : PreconditionResult.FromError("Current guild was not found in the database...\nAborting operation"));
        }
    }
}
