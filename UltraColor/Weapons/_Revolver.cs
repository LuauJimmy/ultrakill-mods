using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using Color = UnityEngine.Color;
using System.Text;
using UltraColor;
using UnityEngine;
using System.Linq;

namespace EffectChanger.Weapons
{
    public sealed class _Revolver : MonoSingleton<_Revolver>
    {
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Sprite? muzzleFlashInnerBase => Plugin.muzzleFlashInnerBase;

        private static Sprite? defaultMuzzleFlashSprite => Plugin.defaultMuzzleFlashSprite;
        private static Texture? defaultChargeTexture => Plugin.chargeBlankTexture;

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
        [HarmonyPatch(typeof(RemoveOnTime), nameof(RemoveOnTime.Start))]
        private static void RecolorLasss(RemoveOnTime __instance)
        {
            if (!__instance.gameObject.name.Contains("LaserHitParticle")) return;
            var currentRevolver = GunControl.instance.currentWeapon.GetComponent<Revolver>();
            if (currentRevolver == null) return;
            GameObject currentBeam;
            Color color2;
            switch(__instance.gameObject.name)
            {
                case "LaserHitParticle(Clone)":
                    currentBeam = currentRevolver.revolverBeam;
                    color2 = currentBeam.GetComponent<LineRenderer>().endColor;
                    break;
                case "SuperLaserHitParticle(Clone)":
                    currentBeam = currentRevolver.revolverBeamSuper;
                    color2 = currentBeam.GetComponent<LineRenderer>().startColor;
                    break;
                case "SharpLaserHitParticle(Clone)":
                    currentBeam = currentRevolver.revolverBeamSuper;
                    color2 = currentBeam.GetComponent<LineRenderer>().startColor;
                    break;
                default:
                    return;
            }

            Gradient gradient = new Gradient();
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 0.3f), new GradientColorKey(color2, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 0.3f), new GradientAlphaKey(1f, 0.7f) });


            var COL = __instance.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;

            COL.color = gradient;
        }

        //static bool shouldInitRevolverHitParticles = true;

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.RestartScene))]
        //[HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.LoadScene))]
        //private static void RecolorLaserHitParticles()
        //{
        //    Gradient gradient = new Gradient();

        //    //ps.startColor = Settings.altPiercerRevolverBeamEndColor.value;
        //    try
        //    {
        //        var revolver = Resources.FindObjectsOfTypeAll<Revolver>()
        //            .Where(s => s.name.Contains("Ricochet(Clone)"))
        //            .First();

        //        if (revolver != null) { Debug.Log("\n\n\n\n\n\n\n\nSuccessfully found Revolver"); }

        //        var revolverBeam = revolver.revolverBeam.GetComponent<RevolverBeam>();
        //        var origHp = revolverBeam.hitParticle;
        //        var origPs = origHp.GetComponent<ParticleSystem>();
        //        var origPsr = origHp.GetComponent<ParticleSystemRenderer>();
        //        origHp.SetActive(true);
        //        var newHp = Instantiate(origHp);

        //        revolverBeam.hitParticle = newHp;
        //        newHp.SetActive(true);

        //        Color color1 = Settings.dodgeStartColor.value;
        //        Color color2 = Settings.dodgeEndColor.value;
        //        //var myMat = Plugin.Fetch<Material>("Assets/Materials/Dev/Additive.mat");
        //        //var unlitShader = Plugin.Fetch<Shader>("Assets/Shaders/Transparent/ULTRAKILL-simple-additive.shader"); //Fetch<Shader>("Assets/Shaders/Particles/Particle_Additive.shader");

        //        //var newMat = new Material(myMat)
        //        //{
        //        //    color = new Color(1, 1, 1, 1f),
        //        //};

        //        //var ps = newHp.GetComponent<ParticleSystem>();
        //        //var psr = newHp.GetComponent<ParticleSystemRenderer>();

        //        //newHp.GetComponent<ParticleSystemRenderer>().material = newMat;

        //        gradient.SetKeys(
        //        new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 0.5f) },
        //            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 0.7f) });

        //        var COL = revolverBeam.hitParticle.GetComponent<ParticleSystem>().colorOverLifetime;
        //        COL.enabled = true;
        //        COL.color = gradient;

        //        //var COL = ps.colorOverLifetime;
        //        //COL.enabled = true;
        //        //COL.color = gradient;
        //        //newMat.shader = unlitShader;

        //        //revolverBeam.hitParticle = newHp;

        //        //Debug.Log("\n\n\n\n\n\n\n\n" + revolverBeam.hitParticle);
        //        shouldInitRevolverHitParticles = false;
        //    }
        //    catch
        //    {
        //        return;
        //    }

        //}


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
                    if (!Settings.sharpShooterEnabled.value) return true;
                    muzzleFlashColor = Settings.sharpShooterMuzzleFlashColor.value;
                    break;
                case "Alternative Revolver Twirl(Clone)":
                    if (!Settings.altSharpShooterEnabled.value) return true;
                    muzzleFlashColor = Settings.altSharpShooterMuzzleFlashColor.value;
                    break;


                default: return true;
            }

            var light = __instance.revolverBeamSuper.GetComponent<Light>();
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
            MeshRenderer mr;
            Material coloredChargeMaterial;
            ParticleSystemRenderer psr;
            
            switch (__instance.gameObject.name)
            {
                case "Revolver Pierce(Clone)":
                    if (!Settings.piercerRevolverEnabled.value) return true;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.piercerRevolverChargeBeamStartColor.value;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.piercerRevolverChargeBeamEndColor.value;

                    if (Settings.piercerRevolverChargeEffectColor.value == Settings.piercerRevolverChargeEffectColor.defaultValue) return true;
                    // Replace revolver charge effect material

                    var piercerChargeEffect = __instance.transform.Find("Revolver_Rerigged_Standard/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint/ChargeEffect");
                    mr = piercerChargeEffect.GetComponent<MeshRenderer>();
                    psr = piercerChargeEffect.GetComponent<ParticleSystemRenderer>();
                    coloredChargeMaterial = new Material(mr.material) 
                    {
                        mainTexture = defaultChargeTexture,
                        color = Settings.piercerRevolverChargeEffectColor.value,
                    };
                    mr.material = coloredChargeMaterial;
                    psr.material = coloredChargeMaterial;
                    piercerChargeEffect.gameObject.AddComponent<ChargeEffectScale>();
                    piercerChargeEffect.GetComponent<Light>().color = Settings.piercerRevolverChargeEffectColor.value;
                    break;

                case "Alternative Revolver Pierce(Clone)":
                    if (!Settings.altPiercerRevolverEnabled.value) return true;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.altPiercerRevolverChargeBeamStartColor.value;
                    __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.altPiercerRevolverChargeBeamEndColor.value;

                    if (Settings.altPiercerRevolverChargeEffectColor.value == Settings.altPiercerRevolverChargeEffectColor.defaultValue) return true;

                    var altPiercerChargeEffect = __instance.transform.Find("Revolver_Rerigged_Alternate/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint (1)/ChargeEffect");
                    mr = altPiercerChargeEffect.GetComponent<MeshRenderer>();
                    psr = altPiercerChargeEffect.GetComponent<ParticleSystemRenderer>();
                    
                    coloredChargeMaterial = new Material(mr.material)
                    {
                        mainTexture = defaultChargeTexture,
                        color = Settings.altPiercerRevolverChargeEffectColor.value,
                    };
                    mr.material = coloredChargeMaterial;
                    psr.material = coloredChargeMaterial;
                    altPiercerChargeEffect.gameObject.AddComponent<ChargeEffectScale>();
                    altPiercerChargeEffect.GetComponent<Light>().color = Settings.altPiercerRevolverChargeEffectColor.value;
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
        [HarmonyFinalizer]
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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Coin), "Start")]
        private static void RecolorCoinFlash(Coin __instance)
        {
            if (!Settings.revolverCoinFlashEnabled.value) { return; }
            var color = Settings.revolverCoinFlashColor.value;
            var flashGo = __instance.flash;
            flashGo.GetComponent<Light>().color = color;
            var flashes = flashGo.GetComponentsInChildren<SpriteRenderer>();
            foreach ( var f in flashes )
            {
                f.sprite = blankMuzzleFlashSprite;
                f.color = color;
            }
            
        }
    }
}