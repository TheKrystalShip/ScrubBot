namespace ScrubBot.Extensions
{
    public static class UlongExtensions
    {
        public static string Mention (this ulong id)
        {
            return $"<@{id}>";
        }
    }
}
