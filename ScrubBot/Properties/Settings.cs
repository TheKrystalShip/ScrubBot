using Microsoft.Extensions.Configuration;

using System.IO;

namespace ScrubBot.Properties
{
    public class Settings
    {
        static Settings()
        {
            string settingsFile = Path.Combine("Properties", "settings.json");
            if (!File.Exists(settingsFile ))
                File.Create(settingsFile );
        }

        private static IConfiguration _config;

        public static IConfiguration Instance => _config ??
            (_config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
                .Build());
    }
}
