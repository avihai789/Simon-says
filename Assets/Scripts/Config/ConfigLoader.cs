using System;
using System.Linq;

namespace SimonSays.Config
{
    public class ConfigLoader
    {
        private Config _loadedConfig;

        // Can be used to load config from xml or json (and other formats if needed in the future
        public Config LoadConfigByType(IConfigLoad configLoad)
        {
            _loadedConfig = configLoad.LoadConfig();
            CheckConfig();
            return _loadedConfig;
        }

        // Check if the config is valid
        private void CheckConfig()
        {
            if (!_loadedConfig.levelsData.Any(level => level.buttons is < 2 or > 6)) return;
            _loadedConfig = null;
            throw new Exception("number of buttons is wrong");
        }
    }
}