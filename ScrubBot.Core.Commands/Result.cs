using Discord.Commands;

namespace ScrubBot.Core.Commands
{
    public static class Result
    {
        public static SuccessResult Success(CommandError commandError, string message)
        {
            return new SuccessResult(commandError, message);
        }

        public static SuccessResult Success(string message)
        {
            return new SuccessResult(message);
        }

        public static ErrorResult Error(CommandError commandError, string message)
        {
            return new ErrorResult(commandError, message);
        }

        public static ErrorResult Error(string message)
        {
            return new ErrorResult(message);
        }

        public static InfoResult Info(CommandError commandError, string message)
        {
            return new InfoResult(commandError, message);
        }

        public static InfoResult Info(string message)
        {
            return new InfoResult(message);
        }
    }
}
