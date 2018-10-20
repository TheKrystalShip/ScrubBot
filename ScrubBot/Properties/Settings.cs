using Microsoft.Extensions.Configuration;

using System.IO;

namespace ScrubBot.Properties
{
    public class Settings
    {
        private readonly static string _settingsFile = Path.Combine("Properties", "settings.json");
        private readonly static string _secretsFile = Path.Combine("Properties", "secrets.json");

        private static IConfiguration _config;

        public static IConfiguration Instance => _config ??
            (_config = new ConfigurationBuilder()
                .AddJsonFile(path: _settingsFile, optional: false, reloadOnChange: true)
                .AddJsonFile(path: _secretsFile, optional: false, reloadOnChange: true)
                .Build());

        static Settings()
        {
            Directory.CreateDirectory("Properties");

            if (!File.Exists(_settingsFile))
                File.Create(_settingsFile);

            if (!File.Exists(_secretsFile))
                File.Create(_secretsFile);
        }
    }
}
