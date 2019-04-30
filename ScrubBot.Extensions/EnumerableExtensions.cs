using System.Collections.Generic;
using System.Text;

using static ScrubBot.Extensions.GenericExtensions;

namespace ScrubBot.Extensions
{
    public static class EnumerableExtensions
    {
        public static string BuildString(this IEnumerable<Variance> variances)
        {
            StringBuilder @string = new StringBuilder();

            foreach (Variance variance in variances)
            {
                @string.AppendLine($"Property {variance.Property} has been modified:");
                @string.AppendLine($"Original: {variance.Original.ToString()}, Modified: {variance.Modified.ToString()}");
                @string.AppendLine();
            }

            return @string.ToString();
        }
    }
}
