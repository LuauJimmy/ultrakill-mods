using BepInEx;
using EffectChanger.Enum;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Weapons
{
    public sealed class _Revolver : MonoSingleton<_Revolver>
    {
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Sprite? muzzleFlashInnerBase => Plugin.muzzleFlashInnerBase;

        private static Sprite? defaultMuzzleFlashSprite => Plugin.defaultMuzzleFlashSprite;

        // Need to patch ReadyGun instead of Start because lots of weapons use the same normal fire mode projectile
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Revolver), "ReadyGun")]
        private static void RecolorRevolverMuzzleFlash(Revolver __instance)
        {
            Color color;
            bool resetToDefault = false;
            switch (__instance.gameObject.name)
            {
                case "Revolver Pierce(Clone)":
                    if (!Settings.piercerRevolverEnabled.value) resetToDefault = true;
                    color = Settings.piercerRevolverMuzzleFlashColor.value;
                    break;

                case "Revolver Twirl(Clone)":
                    if (!Settings.sharpShooterEnabled.value) resetToDefault = true;
                    color = Settings.sharpShooterMuzzleFlashColor.value;
                    break;

                case "Revolver Ricochet(Clone)":
                    if (!Settings.marksmanEnabled.value) resetToDefault = true;
                    color = Settings.marksmanMuzzleFlashColor.value;
                    break;

                case "Alternative Revolver Ricochet(Clone)":
                    if (!Settings.altMarksmanEnabled.value) resetToDefault = true;
                    color = Settings.altMarksmanMuzzleFlashColor.value;
                    break;

                case "Alternative Revolver Twirl(Clone)":
                    if (!Settings.altSharpShooterEnabled.value) resetToDefault = true;
                    color = Settings.altSharpShooterMuzzleFlashColor.value;
                    break;

                case "Alternative Revolver Pierce(Clone)":
                    if (!Settings.altPiercerRevolverEnabled.value) resetToDefault = true;
                    color = Settings.altPiercerRevolverMuzzleFlashColor.value;
                    break;

                default: return;
            }

            if (resetToDefault)
            {
                __instance.revolverBeam.GetComponent<Light>().color = new Color(1, 0.7594f, 0, 1);
                var mfs = __instance.revolverBeam.GetComponentsInChildren<SpriteRenderer>();
                foreach (var muzzle in mfs)
                {
                    muzzle.sprite = defaultMuzzleFlashSprite;
                    muzzle.color = new Color(1, 1, 1, 1);
                }
                return;
            }

            var light = __instance.revolverBeam.GetComponent<Light>();
            light.color = color;
            var muzzleFlashes = __instance.revolverBeam.GetComponentsInChildren<SpriteRenderer>();
            foreach (var muzzle in muzzleFlashes)
            {
                muzzle.sprite = blankMuzzleFlashSprite;
                muzzle.color = color;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Revolver), "ReadyGun")]
        private static bool RecolorRevolverChargeMuzzleFlash(Revolver __instance)
        {
            Color muzzleFlashColor;
            bool resetToDefault = false;
            switch (__instance.gameObject.name)
            {
                case "Revolver Pierce(Clone)":
                    if (!Settings.piercerRevolverEnabled.value) resetToDefault = true;
                    muzzleFlashColor = Settings.piercerRevolverChargeMuzzleFlashColor.value;
                    break;

                case "Alternative Revolver Pierce(Clone)":
                    if (!Settings.altPiercerRevolverEnabled.value) resetToDefault = true;
                    muzzleFlashColor = Settings.altPiercerRevolverChargeMuzzleFlashColor.value;
                    break;
                case "Revolver Twirl(Clone)":
                    if (!Settings.sharpShooterEnabled.value) resetToDefault = true;
                    muzzleFlashColor = Settings.sharpShooterMuzzleFlashColor.value;
                    break;
                case "Alternative Revolver Twirl(Clone)":
                    if (!Settings.altSharpShooterEnabled.value) resetToDefault = true;
                    muzzleFlashColor = Settings.altSharpShooterMuzzleFlashColor.value;
                    break;
                default: return true;
            }

            if (resetToDefault)
            {
                __instance.revolverBeam.GetComponent<Light>().color = new Color(1, 0.7594f, 0, 1);
                var mfs = __instance.revolverBeamSuper.GetComponentsInChildren<SpriteRenderer>();
                foreach (var muzzle in mfs)
                {
                    muzzle.sprite = defaultMuzzleFlashSprite;
                    muzzle.color = new Color(1, 1, 1, 1);
                }
                return true;
            }

            var light = __instance.revolverBeam.GetComponent<Light>();
            light.color = muzzleFlashColor;
            var muzzleFlashes = __instance.revolverBeamSuper.GetComponentsInChildren<SpriteRenderer>();
            foreach (var muzzle in muzzleFlashes)
            {
                muzzle.sprite = blankMuzzleFlashSprite;
                muzzle.color = muzzleFlashColor;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Revolver), "ReadyGun")]
        private static bool RecolorRevolverBeam(Revolver __instance)
        {
            switch (__instance.gameObject.name)
            {
                case "Revolver Pierce(Clone)":
                    if (!Settings.piercerRevolverEnabled.value) return true;
                    __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.piercerRevolverBeamStartColor.value;
                    __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.piercerRevolverBeamEndColor.value;

                    break;

                case "Revolver Twirl(Clone)":
                    if (!Settings.sharpShooterEnabled.value) return true;
                    __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.sharpShooterBeamStartColor.value;
                    __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.sharpShooterBeamEndColor.value;
                    break;

                case "Revolver Ricochet(Clone)":
                    if (!Settings.marksmanEnabled.value) return true;
                    __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.marksmanBeamStartColor.value;
                    __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.marksmanBeamEndColor.value;
                    break;

                case "Alternative Revolver Ricochet(Clone)":
                    if (!Settings.altMarksmanEnabled.value) return true;
                    __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.altMarksmanBeamStartColor.value;
                    __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.altMarksmanBeamEndColor.value;
                    break;

                case "Alternative Revolver Twirl(Clone)":
                    if (!Settings.altSharpShooterEnabled.value) return true;
                    __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.altSharpShooterBeamStartColor.value;
                    __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.altSharpShooterBeamEndColor.value;
                    break;

                case "Alternative Revolver Pierce(Clone)":
                    if (!Settings.altPiercerRevolverEnabled.value) return true;
                    __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.altPiercerRevolverBeamStartColor.value;
                    __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.altPiercerRevolverBeamEndColor.value;
                    break;
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Revolver), "Start")]
        private static bool RecolorRevolverChargeBeam(Revolver __instance)
        {
            switch (__instance.gameObject.name)
            {
                case "Revolver Pierce(Clone)":
                    if (!Settings.piercerRevolverEnabled.value) return true;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.piercerRevolverBeamStartColor.value;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.piercerRevolverBeamEndColor.value;

                    if (Settings.piercerRevolverChargeEffectColor.value == ColorHelper.BulletColor.Default) return true;
                    // Replace revolver charge effect material
                    Material newMat = ColorHelper.LoadBulletColor(Settings.piercerRevolverChargeEffectColor.value);
                    var c = __instance.transform.Find("Revolver_Rerigged_Standard/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint/ChargeEffect");
                    c.GetComponent<MeshRenderer>().material = newMat;
                    break;

                case "Alternative Revolver Pierce(Clone)":
                    if (!Settings.altPiercerRevolverEnabled.value) return true;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.altPiercerRevolverChargeBeamStartColor.value;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.altPiercerRevolverChargeBeamEndColor.value;

                    if (Settings.altPiercerRevolverChargeEffectColor.value == ColorHelper.BulletColor.Default) return true;

                    var altPiercerChargeEffect = __instance.transform.Find("Revolver_Rerigged_Alternate/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint (1)/ChargeEffect");
                    altPiercerChargeEffect.GetComponent<MeshRenderer>().material = ColorHelper.LoadBulletColor(Settings.altPiercerRevolverChargeEffectColor.value);
                    break;

                case "Revolver Twirl(Clone)":
                    if (!Settings.sharpShooterEnabled.value) return true;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.sharpShooterChargeBeamStartColor.value;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.sharpShooterChargeBeamEndColor.value;
                    break;

                case "Alternative Revolver Twirl(Clone)":
                    if (!Settings.altSharpShooterEnabled.value) return true;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.altSharpShooterChargeBeamStartColor.value;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.altSharpShooterChargeBeamEndColor.value;
                    break;

                default:
                    break;
            }
            return true;
        }

        // Add a little white
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RevolverBeam), "Shoot")]
        private static void AddMuzzleFlashInnerComponent_Revolver(RevolverBeam __instance)
        {
            var muzzleFlashes = __instance.GetComponentsInChildren<SpriteRenderer>();

            foreach (var flash in muzzleFlashes)
            {
                var colorA = flash.GetComponent<SpriteRenderer>().color;
                var obj = Instantiate(flash);
                var interpColor = Color.Lerp(colorA, Color.white, 0.8f);
                interpColor.a = 0.95f;
                obj.GetComponent<SpriteRenderer>().color = interpColor;
                obj.GetComponent<SpriteRenderer>().sprite = muzzleFlashInnerBase;
                obj.transform.position = flash.transform.position;
                obj.transform.rotation = flash.transform.rotation;
                obj.transform.localScale = __instance.transform.localScale;
                //obj.transform.localScale = flash.transform.localScale;
                obj.gameObject.AddComponent<MuzzleFlashInnerComponent>();
            }
            return;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RevolverBeam), "Start")]
        private static void RecolorReflectedRevolverBeam(RevolverBeam __instance)
        {
            if (__instance.gameObject.name == "ReflectedBeamPoint(Clone)")
            {
                if (!Settings.marksmanEnabled.value) return;
                __instance.gameObject.GetComponentInParent<LineRenderer>().startColor = Settings.revolverCoinRicochetBeamStartColor.value;
                __instance.gameObject.GetComponentInParent<LineRenderer>().endColor = Settings.revolverCoinRicochetBeamEndColor.value;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Coin), "Start")]
        private static void RecolorCoinTrail(Coin __instance)
        {
            if (!Settings.coinEnabled.value) return;
            __instance.GetComponent<TrailRenderer>().startColor = Settings.revolverCoinTrailStartColor.value;
            __instance.GetComponent<TrailRenderer>().endColor = Settings.revolverCoinTrailEndColor.value;
        }
    }
}