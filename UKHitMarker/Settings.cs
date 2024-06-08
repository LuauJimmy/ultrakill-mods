using BepInEx.Configuration;
using PluginConfig.API;
using PluginConfig.API.Decorators;
using PluginConfig.API.Fields;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace UKHitMarker
{
#nullable disable

    public static class Settings
    {
        private static PluginConfigurator? config;

        public static BoolField hitMarkerEnabled;
        public static ColorField hitMarkerColor;
        public static ColorField killMarkerColor;
        public static IntField hitMarkerSize;
        public static void Init(ConfigFile cfg)
        {
            config = PluginConfigurator.Create("UKHitMarker", "luaujimmy.UKHitMarker");
            string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string iconPath = Path.Combine(pluginPath, "Assets\\icon.png");
            config.SetIconWithURL("file://" + iconPath);

            ConfigHeader headerTitle = new ConfigHeader(config.rootPanel, "UKHitMarker", 30);
            ConfigHeader headerInfo = new ConfigHeader(config.rootPanel, "--Restart Level for Changes to Take Effect--", 20);

            hitMarkerEnabled = new BoolField(config.rootPanel, "Enabled", "HitMarkerEnabled", true);
            hitMarkerColor = new ColorField(config.rootPanel, "Hit Marker Color", "HitMarkerColor", new Color(1, 1, 1, 1));
            killMarkerColor = new ColorField(config.rootPanel, "Kill Marker Color", "KillMarkerColor", Color.red);
            hitMarkerSize = new IntField(config.rootPanel, "Hit Marker Size", "HitMarkerSize", 20);
        }
    }
}