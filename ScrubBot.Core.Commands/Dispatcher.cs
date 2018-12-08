using System.Threading.Tasks;

using Discord.Commands;

namespace ScrubBot.Core.Commands
{
    public static class Dispatcher
    {
        public static async Task Dispatch(CommandInfo commandInfo, ICommandContext context, IResult result)
        {
            // IResult needs to be unboxed in order to access the correct properties
            switch (result)
            {
                case SuccessResult successResult:
                    await context
                        .Channel
                        .SendMessageAsync(text: successResult.Reason, isTTS: false, embed: successResult.Embed);
                    break;
                case ErrorResult errorResult:
                    await context
                        .Channel
                        .SendMessageAsync(text: errorResult.Reason, isTTS: false, embed: errorResult.Embed);
                    break;
                case InfoResult infoResult:
                    await context
                        .Channel
                        .SendMessageAsync(text: infoResult.Reason, isTTS: false, embed: infoResult.Embed);
                    break;
                default:
                    break;
            }
        }
    }
}
