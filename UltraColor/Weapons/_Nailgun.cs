using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Weapons
{
    public sealed class _Nailgun : MonoSingleton<_Nailgun>
    {
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Sprite? muzzleFlashInnerBase => Plugin.muzzleFlashInnerBase;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Nailgun), "Start")]
        private static void RecolorNailgunProjecileTrail(Nailgun __instance)
        {
            switch (__instance.gameObject.name)
            {
                case "Nailgun Magnet(Clone)":
                    if (!Settings.magnetNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.magnetNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.magnetNailgunTrailEndColor.value;

                    break;

                case "Nailgun Overheat(Clone)":
                    if (!Settings.overheatNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.overheatNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.overheatNailgunTrailEndColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().startColor = Settings.overheatNailgunHeatedNailTrailStartColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().endColor = Settings.overheatNailgunHeatedNailTrailEndColor.value;

                    break;

                case "Sawblade Launcher Magnet(Clone)":
                    if (!Settings.altMagnetNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.altMagnetNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.altMagnetNailgunTrailEndColor.value;
                    break;

                case "Sawblade Launcher Overheat(Clone)":
                    if (!Settings.altOverheatNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.altOverheatNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.altOverheatNailgunTrailEndColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().startColor = Settings.altOverheatNailgunHeatedNailTrailStartColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().endColor = Settings.altOverheatNailgunHeatedNailTrailEndColor.value;
                    break;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Nailgun), "OnEnable")]
        private static void RecolorNailgunMuzzleFlash(Nailgun __instance)
        {
            Color color;
            switch (__instance.gameObject.name)
            {
                case "Nailgun Magnet(Clone)":
                    if (!Settings.magnetNailgunEnabled.value) return;
                    color = Settings.magnetNailgunMuzzleFlashColor.value;
                    break;

                case "Nailgun Overheat(Clone)":
                    if (!Settings.overheatNailgunEnabled.value) return;
                    color = Settings.overheatNailgunMuzzleFlashColor.value;
                    break;

                case "Sawblade Launcher Magnet(Clone)":
                    if (!Settings.altMagnetNailgunEnabled.value) return;
                    color = Settings.altMagnetNailgunMuzzleFlashColor.value;
                    break;

                case "Sawblade Launcher Overheat(Clone)":
                    if (!Settings.altOverheatNailgunEnabled.value) return;
                    color = Settings.altOverheatNailgunMuzzleFlashColor.value;
                    break;

                default: return;
            }
            var light = __instance.muzzleFlash.GetComponentInChildren<Light>();
            light.color = color;
            var muzzleFlashes = __instance.muzzleFlash.GetComponentsInChildren<SpriteRenderer>();
            foreach (var muzzle in muzzleFlashes)
            {
                muzzle.sprite = blankMuzzleFlashSprite;
                muzzle.color = color;
            };
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Nailgun), "Shoot")]
        private static void AddMuzzleFlashInnerComponent_Nailgun(Nailgun __instance)
        {
            var muzzleFlashes = __instance.muzzleFlash.GetComponentsInChildren<SpriteRenderer>();

            foreach (var flash in muzzleFlashes)
            {
                var colorA = flash.GetComponent<SpriteRenderer>().color;
                var obj = Instantiate(flash);
                var interpColor = Color.Lerp(colorA, Color.white, 0.8f);
                interpColor.a = 0.8f;
                obj.GetComponent<SpriteRenderer>().color = interpColor;
                obj.GetComponent<SpriteRenderer>().sprite = muzzleFlashInnerBase;
                obj.transform.position = __instance.shootPoints[0].transform.position;
                obj.transform.rotation = __instance.shootPoints[0].transform.rotation;
                obj.gameObject.AddComponent<MuzzleFlashInnerComponent>();
            };
        }
    }
}
