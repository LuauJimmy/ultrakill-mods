using BepInEx.Configuration;
using PluginConfig.API;
using PluginConfig.API.Decorators;
using PluginConfig.API.Fields;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace UltraShade
{
#nullable disable

    public static class Settings
    {
        private static PluginConfigurator? config;

        
        public static void Init(ConfigFile cfg)
        {
            config = PluginConfigurator.Create("UltraShade", "luaujimmy.UltraShade");
            string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string iconPath = Path.Combine(pluginPath, "Assets\\icon.png");
            config.SetIconWithURL("file://" + iconPath);

            ConfigHeader headerTitle = new ConfigHeader(config.rootPanel, "UltraShade", 30);
            ConfigHeader headerInfo = new ConfigHeader(config.rootPanel, "--Restart Level for Changes to Take Effect--", 20);

        }
    }
}