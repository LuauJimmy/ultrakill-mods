using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Weapons
{
    [BepInPlugin(PluginInfo.guid, PluginInfo.name, PluginInfo.version)]
    public sealed class _RocketLauncher : BaseUnityPlugin
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RocketLauncher), "OnEnable")]
        private static void RecolorRockets(RocketLauncher __instance)
        {
            switch (__instance.gameObject.name)
            {
                case "Rocket Launcher Freeze(Clone)":
                    if (!Settings.freezeRocketLauncherEnabled.value) return;
                    __instance.rocket.GetComponent<TrailRenderer>().startColor = Settings.freezeRocketLauncherTrailStartColor.value;
                    __instance.rocket.GetComponent<TrailRenderer>().endColor = Settings.freezeRocketLauncherTrailEndColor.value;
                    break;

                case "Rocket Launcher Cannonball(Clone)":
                    if (!Settings.cannonballRocketLauncherEnabled.value) return;
                    __instance.rocket.GetComponent<TrailRenderer>().startColor = Settings.cannonballRocketLauncherTrailStartColor.value;
                    __instance.rocket.GetComponent<TrailRenderer>().endColor = Settings.cannonballRocketLauncherTrailEndColor.value;
                    break;
            }
        }
    }
}
