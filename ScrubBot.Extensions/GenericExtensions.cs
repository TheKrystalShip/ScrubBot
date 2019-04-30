using System.Collections.Generic;
using System.Reflection;

namespace ScrubBot.Extensions
{
    public static class GenericExtensions
    {
        public class Variance
        {
            public string Property { get; set; }
            public object Original { get; set; }
            public object Modified { get; set; }
        }

        public static IEnumerable<Variance> Compare<T>(this T original, T modified)
        {
            PropertyInfo[] properties = original.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Variance variance = new Variance
                {
                    Property = property.Name,
                    Original = property.GetValue(original),
                    Modified = property.GetValue(modified)
                };

                if (!Equals(variance.Original, variance.Modified))
                {
                    yield return variance;
                }
            }
        }
    }
}
