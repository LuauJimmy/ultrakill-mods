using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using System.Linq;

using PluginInfo = EffectChanger.PluginInfo;
using UnityEngine;
using UnityEngine.UI;
namespace UltraColor
{
    public sealed class DebugPatches : MonoSingleton<DebugPatches>
    {
        private static GameObject statsPanel;
        private static TextMesh dashFrames;
        private static TextMesh dashStorage;
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Coin), "Start")]
        //private static void CoinDebugMethod(Coin __instance)
        //{
        //    var assetPaths = Addressables.ResourceLocators
        //        .SelectMany(locator => locator.Keys)
        //        .Distinct()
        //        .OfType<string>()
        //        .Where(key => key.Contains('/'));

        //    Utils.DumpAssetPaths(assetPaths);
        //}

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Coin), "Start")]
        private static void Test(Coin __instance)
        {

        }




        [HarmonyPostfix]
        [HarmonyPatch(typeof(RailcannonMeter), nameof(RailcannonMeter.Start))]
        private static void SetBullshit(RailcannonMeter __instance)
        {
            if (__instance.gameObject == null) return;
            statsPanel = __instance.gameObject;
            Debug.Log(statsPanel.gameObject.name);
            statsPanel.GetComponent<Image>().enabled = false;
            dashFrames = statsPanel.AddComponent<TextMesh>();
            dashFrames.fontSize = 36;
            __instance.transform.localScale *= 10;
            GameObject dsgo = new();
            dsgo.name = "dsgo";
            dashStorage = dsgo.AddComponent<TextMesh>();
            dashStorage.fontSize = 36;
            dsgo.transform.parent = __instance.transform;
            dsgo.transform.position= dashFrames.transform.position;
            dsgo.transform.rotation = dashFrames.transform.rotation;
            dsgo.transform.localScale = dashFrames.transform.localScale;
            
            //dashStorage.transform.localPosition = new Vector3(pos.x, pos.y - 1, pos.z);
            return;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Update))]
        private static void UpdateDashFrameCounter(NewMovement __instance)
        {
            if (dashFrames == null || dashStorage == null) return;
            dashFrames.text = __instance.boostLeft.ToString();
            dashStorage.text = __instance.dashStorage.ToString();
        }
    }
}
