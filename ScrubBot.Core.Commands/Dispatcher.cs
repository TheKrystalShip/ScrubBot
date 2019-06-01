using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using ScrubBot.Modules;

namespace ScrubBot.Core.Commands
{
    public static class Dispatcher
    {
        public static async Task Dispatch(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
        {
            // IResult needs to be unboxed in order to access the correct properties
            switch (result)
            {
                case SuccessResult successResult:
                    await context
                        .Channel
                        .SendMessageAsync(string.Empty, false, successResult?.Embed);
                    break;
                case ErrorResult errorResult:
                    await context
                        .Channel
                        .SendMessageAsync(string.Empty, false, errorResult?.Embed);
                    break;
                case InfoResult infoResult:
                    await context
                        .Channel
                        .SendMessageAsync(string.Empty, false, infoResult?.Embed);
                    break;
                case EmptyResult emptyResult:
                    await context
                        .Channel
                        .SendMessageAsync(string.Empty, false, emptyResult?.Embed);
                    break;
                default:
                    await context
                        .Channel
                        .SendMessageAsync(result.ToString());
                    break;
            }
        }
    }
}
