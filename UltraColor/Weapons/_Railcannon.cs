using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Weapons
{
    public sealed class _Railcannon : MonoSingleton<_Railcannon>
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Harpoon), "Start")]
        private static bool RecolorScrewRail(Harpoon __instance)
        {
            if (!Settings.greenRailcannonEnabled.value) return true;
            __instance.gameObject.GetComponent<TrailRenderer>().startColor = Settings.greenRailcannonTrailStartColor.value;
            __instance.gameObject.GetComponent<TrailRenderer>().endColor = Settings.greenRailcannonTrailEndColor.value;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RevolverBeam), "Start")]
        private static void RecolorRailcannonBeams(RevolverBeam __instance)
        {
            Debug.Log($"GameObject: {__instance.gameObject}");
            switch (__instance.gameObject.name)
            {
                case "Railcannon Beam(Clone)":
                    if (!Settings.blueRailcannonEnabled.value) return;
                    __instance.gameObject.GetComponent<LineRenderer>().startColor = Settings.blueRailcannonStartColor.value;
                    __instance.gameObject.GetComponent<LineRenderer>().endColor = Settings.blueRailcannonEndColor.value;
                    break;

                case "Railcannon Beam Malicious(Clone)":
                    if (!Settings.redRailcannonEnabled.value) return;
                    __instance.gameObject.GetComponent<LineRenderer>().startColor = Settings.redRailcannonStartColor.value;
                    __instance.gameObject.GetComponent<LineRenderer>().endColor = Settings.redRailcannonEndColor.value;

                    //__instance.gameObject.GetComponent<LineRenderer>().startColor = UltraColor.Config.redRailcannonGlowStartColor.value;
                    //__instance.gameObject.GetComponent<LineRenderer>().endColor = UltraColor.Config.redRailcannonGlowEndColor.value;
                    break;

                default: break;
            }
        }
    }
}
