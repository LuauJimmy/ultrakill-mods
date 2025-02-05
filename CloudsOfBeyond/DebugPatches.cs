using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CloudsOfBeyond
{
    //[BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginInfo.guid, PluginInfo.name, PluginInfo.version)]
    public sealed class DebugPatches : BaseUnityPlugin
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Revolver), nameof(Revolver.ThrowCoin))]
        private static void CoinDebug(Revolver __instance)
        {
            var olm = (OutdoorLightMaster)FindObjectsOfType(typeof(OutdoorLightMaster))[0];
            olm.UpdateSkyboxMaterial();
        }

    }
}