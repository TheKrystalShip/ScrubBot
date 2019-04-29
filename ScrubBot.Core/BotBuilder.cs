namespace ScrubBot.Core
{
    public static class BotBuilder
    {
        public static T UseStartup<T>() where T : new() => new T();
    }
}
