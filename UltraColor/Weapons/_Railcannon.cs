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
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Sprite? muzzleFlashInnerBase => Plugin.muzzleFlashInnerBase;
        private static Sprite? chargeBlank => Plugin.chargeBlankSprite;

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
            switch (__instance.gameObject.name)
            {
                case "Railcannon Beam(Clone)":
                    if (!Settings.blueRailcannonEnabled.value) return;
                    __instance.gameObject.GetComponent<LineRenderer>().startColor = Settings.blueRailcannonStartColor.value;
                    __instance.gameObject.GetComponent<LineRenderer>().endColor = Settings.blueRailcannonEndColor.value;

                    var color = Settings.blueRailcannonMuzzleFlashColor.value;

                    var light = __instance.gameObject.GetComponent<Light>();
                    light.color = color;
                    var flash = __instance.gameObject.GetComponentInChildren<SpriteRenderer>();
                    flash.sprite = blankMuzzleFlashSprite;
                    flash.color = color;
                    break;

                case "Railcannon Beam Malicious(Clone)":
                    if (!Settings.redRailcannonEnabled.value) return;
                    __instance.gameObject.GetComponent<LineRenderer>().startColor = Settings.redRailcannonStartColor.value;
                    __instance.gameObject.GetComponent<LineRenderer>().endColor = Settings.redRailcannonEndColor.value;

                    var color2 = Settings.redRailcannonMuzzleFlashColor.value;

                    var light2 = __instance.gameObject.GetComponent<Light>();
                    light2.color = color2;
                    var flash2 = __instance.gameObject.GetComponentInChildren<SpriteRenderer>();
                    flash2.sprite = chargeBlank;
                    flash2.color = color2;

                    //__instance.gameObject.GetComponent<LineRenderer>().startColor = UltraColor.Config.redRailcannonGlowStartColor.value;
                    //__instance.gameObject.GetComponent<LineRenderer>().endColor = UltraColor.Config.redRailcannonGlowEndColor.value;
                    break;

                default: break;
            }
        }
    }
}
