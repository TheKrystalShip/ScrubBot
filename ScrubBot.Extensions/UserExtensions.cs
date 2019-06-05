using ScrubBot.Database.Domain;

namespace ScrubBot.Extensions
{
    public static class UserExtensions
    {
        public static string Mention (this User user)
        {
            return $"<@{user.Id}>";
        }
    }
}
