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
                        .SendMessageAsync(text: successResult.Reason ?? "Success", isTTS: false, embed: successResult.Embed);
                    break;
                case ErrorResult errorResult:
                    await context
                        .Channel
                        .SendMessageAsync(text: errorResult.Reason ?? "Error", isTTS: false, embed: errorResult.Embed);
                    break;
                case InfoResult infoResult:
                    await context
                        .Channel
                        .SendMessageAsync(text: infoResult.Reason ?? "Info", isTTS: false, embed: infoResult.Embed);
                    break;
                default:
                    await context
                        .Channel
                        .SendMessageAsync(text: result.ToString());
                    break;
            }
        }
    }
}
