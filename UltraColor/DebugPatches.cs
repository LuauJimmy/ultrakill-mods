﻿using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using System.Linq;

using PluginInfo = EffectChanger.PluginInfo;
namespace UltraColor
{
    [BepInPlugin(PluginInfo.guid, PluginInfo.name, PluginInfo.version)]
    public sealed class DebugPatches : BaseUnityPlugin
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Coin), "Start")]
        private static void CoinDebugMethod(Coin __instance)
        {
            var assetPaths = Addressables.ResourceLocators
                .SelectMany(locator => locator.Keys)
                .Distinct()
                .OfType<string>()
                .Where(key => key.Contains('/'));

            Utils.DumpAssetPaths(assetPaths);
        }
    }
}
