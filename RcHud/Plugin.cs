using System;
using System.Collections.Generic;

using BepInEx;

using HarmonyLib;

using UnityEngine;
using UnityEngine.AddressableAssets;
using EffectChanger.Enum;

namespace UltraColor;

[BepInPlugin("luaujimmy.UltraColor", "UltraColor", "0.0.1")]
public sealed class Plugin : BaseUnityPlugin {
    public static T Fetch<T>(string key)
    {
        return Addressables.LoadAssetAsync<T>((object)key).WaitForCompletion();
    }

    public void Awake() {
        global::UltraColor.Config.Init(this.Config);
        Harmony.CreateAndPatchAll(this.GetType());

        
    }

    public void Start() {
        UltraColor.SetLogger(this.Logger);

    }

    public void Update() {
        _ = UltraColor.Instance;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shotgun), "Start")]
    static void RecolorShotgunProjectile(Shotgun __instance)
    {
        var newExplosionMat = ColorHelper.LoadBulletColor(ColorHelper.BulletColor.Purple);

        __instance.bullet.GetComponent<TrailRenderer>().startColor = global::UltraColor.Config.shotgunProjectileStartColor.value;
        __instance.bullet.GetComponent<TrailRenderer>().endColor = global::UltraColor.Config.shotgunProjectileEndColor.value;
        
        if (global::UltraColor.Config.shotgunMuzzleFlashColor.value != ColorHelper.MuzzleFlash.Default)
        {
            Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(global::UltraColor.Config.shotgunMuzzleFlashColor.value);
            __instance.muzzleFlash.GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
        }

        if (global::UltraColor.Config.shotgunBulletColor.value != ColorHelper.BulletColor.Default)
        {
            Material newMaterial = ColorHelper.LoadBulletColor(global::UltraColor.Config.shotgunBulletColor.value);
            __instance.bullet.GetComponent<MeshRenderer>().material = newMaterial;
        }
       
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Nailgun), "Start")]
    static void RecolorNailgunProjecileTrail(Nailgun __instance)
    {
       switch (__instance.gameObject.name)
        {
            case "Nailgun Magnet(Clone)":
                __instance.nail.GetComponent<TrailRenderer>().startColor = global::UltraColor.Config.magnetNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = global::UltraColor.Config.magnetNailgunTrailEndColor.value;

                break;
            case "Nailgun Overheat(Clone)":
                __instance.nail.GetComponent<TrailRenderer>().startColor = global::UltraColor.Config.overheatNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = global::UltraColor.Config.overheatNailgunTrailEndColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().startColor = global::UltraColor.Config.overheatNailgunHeatedNailTrailStartColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().endColor = global::UltraColor.Config.overheatNailgunHeatedNailTrailEndColor.value;

                break;
            case "Sawblade Launcher Magnet(Clone)":
                __instance.nail.GetComponent<TrailRenderer>().startColor = global::UltraColor.Config.altMagnetNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = global::UltraColor.Config.altMagnetNailgunTrailEndColor.value;
                break;
            case "Sawblade Launcher Overheat(Clone)":
                __instance.nail.GetComponent<TrailRenderer>().startColor = global::UltraColor.Config.altOverheatNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = global::UltraColor.Config.altOverheatNailgunTrailEndColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().startColor = global::UltraColor.Config.altOverheatNailgunHeatedNailTrailStartColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().endColor = global::UltraColor.Config.altOverheatNailgunHeatedNailTrailEndColor.value;
                break;

        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ExplosionController), "Start")]
    static void RecolorExplosion(ExplosionController __instance)
    {
        if (__instance.gameObject.name == "Explosion(Clone)")
        {
            if (global::UltraColor.Config.smallExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

            var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.smallExplosionColor.value);
            var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
            explosionRenderers[0].material = newExplosionMat;
        }
        else if (__instance.gameObject.name == "Explosion Malicious Railcannon(Clone)")
        {
            if (global::UltraColor.Config.maliciousExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

            var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.maliciousExplosionColor.value);
            var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
            explosionRenderers[0].material = newExplosionMat;
        }
        else if (__instance.gameObject.name == "Explosion Super(Clone)")
        {
            if (global::UltraColor.Config.nukeExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

            var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.nukeExplosionColor.value);
            var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
            explosionRenderers[0].material = newExplosionMat;
        }

    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Grenade), "Start")]
    static void RecolorGrenadeSprite(Grenade __instance)
    {
        if (global::UltraColor.Config.shotgunGrenadeSpriteColor.value == ColorHelper.MuzzleFlash.Default) return;
        Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(global::UltraColor.Config.shotgunGrenadeSpriteColor.value);
        __instance.transform.Find("GameObject").GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Revolver), "Start")]
    static void RecolorRevolverMuzzleFlash(Revolver __instance)
    {
        ColorHelper.MuzzleFlash spriteToLoad;
        switch (__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                if (global::UltraColor.Config.piercerRevolverMuzzleFlashColor == default) return;
                spriteToLoad = global::UltraColor.Config.piercerRevolverMuzzleFlashColor.value;
                break;
            case "Revolver Twirl(Clone)":
                if (global::UltraColor.Config.sharpShooterMuzzleFlashColor== default) return;
                spriteToLoad = global::UltraColor.Config.sharpShooterMuzzleFlashColor.value;
                break;
            case "Revolver Ricochet(Clone)":
                if (global::UltraColor.Config.marksmanMuzzleFlashColor== default) return;
                spriteToLoad = global::UltraColor.Config.marksmanMuzzleFlashColor.value;
                break;
            case "Alternative Revolver Ricochet(Clone)":
                if (global::UltraColor.Config.altMarksmanMuzzleFlashColor== default) return;
                spriteToLoad = global::UltraColor.Config.altMarksmanMuzzleFlashColor.value;
                break;
            case "Alternative Revolver Twirl(Clone)":
                if (global::UltraColor.Config.altSharpShooterMuzzleFlashColor == default) return;
                spriteToLoad = global::UltraColor.Config.altSharpShooterMuzzleFlashColor.value;
                break;
            case "Alternative Revolver Pierce(Clone)":
                if (global::UltraColor.Config.altPiercerRevolverMuzzleFlashColor == default) return;
                spriteToLoad = global::UltraColor.Config.altPiercerRevolverMuzzleFlashColor.value;
                break;
            default: return;
        }
        Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(spriteToLoad);
        var muzzleFlashes = __instance.revolverBeam.GetComponentsInChildren<SpriteRenderer>();
        foreach ( var muzzle in muzzleFlashes ) { muzzle.sprite = newSprite; }

    }

    // Need to patch ReadyGun instead of Start because revolvers use the same normal fire mode projectile
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Revolver), "ReadyGun")]
    static void RecolorRevolverBeam(Revolver __instance)
    {
        switch (__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.piercerRevolverBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.piercerRevolverBeamEndColor.value;
                break;
            case "Revolver Twirl(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.sharpShooterBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.sharpShooterBeamEndColor.value;
                break;
            case "Revolver Ricochet(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.marksmanBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.marksmanBeamEndColor.value;
                break;
            case "Alternative Revolver Ricochet(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.altMarksmanBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.altMarksmanBeamEndColor.value;
                break;
            case "Alternative Revolver Twirl(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.altSharpShooterBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.altSharpShooterBeamEndColor.value;
                break;
            case "Alternative Revolver Pierce(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.altPiercerRevolverBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.altPiercerRevolverBeamEndColor.value;
                break;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Revolver), "Start")]
    static void RecolorRevolverChargeBeam(Revolver __instance)
    {
        switch(__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.sharpShooterChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.sharpShooterChargeBeamEndColor.value;


                if (global::UltraColor.Config.altPiercerRevolverChargeEffectColor.value == ColorHelper.BulletColor.Default) return;
                // Replace revolver charge effect material
                Material newMat = ColorHelper.LoadBulletColor(global::UltraColor.Config.altPiercerRevolverChargeEffectColor.value);
                var c = __instance.transform.Find("Revolver_Rerigged_Alternate/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint (1)/ChargeEffect");
                c.GetComponent<MeshRenderer>().material = newMat;
                break;
            case "Alternative Revolver Pierce(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.altPiercerRevolverChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.altPiercerRevolverChargeBeamEndColor.value;

                if (global::UltraColor.Config.altPiercerRevolverChargeEffectColor.value == ColorHelper.BulletColor.Default) return;

                
                var altPiercerChargeEffect = __instance.transform.Find("Revolver_Rerigged_Alternate/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint (1)/ChargeEffect");
                altPiercerChargeEffect.GetComponent<MeshRenderer>().material = ColorHelper.LoadBulletColor(global::UltraColor.Config.altPiercerRevolverChargeEffectColor.value); ;
                break;
            case "Revolver Twirl(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.sharpShooterChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.sharpShooterChargeBeamEndColor.value;
                break;
            case "Alternative Revolver Twirl(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.altSharpShooterChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.altSharpShooterChargeBeamEndColor.value;
                break;
            default:
                break;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RevolverBeam), "Start")]
    static void RecolorRevolverCoinShot(RevolverBeam __instance)
    {
        switch (__instance.gameObject.name)
        {
            case "ReflectedBeamPoint(Clone)":
                __instance.gameObject.GetComponentInParent<LineRenderer>().startColor = global::UltraColor.Config.revolverCoinRicochetBeamStartColor.value;
                __instance.gameObject.GetComponentInParent<LineRenderer>().endColor = global::UltraColor.Config.revolverCoinRicochetBeamEndColor.value;
                break;
            case "Railcannon Beam(Clone)":
                __instance.gameObject.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.blueRailcannonStartColor.value;
                __instance.gameObject.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.blueRailcannonEndColor.value;
                break;
            case "Railcannon Beam Malicious(Clone)":
                __instance.gameObject.GetComponent<LineRenderer>().startColor = global::UltraColor.Config.redRailcannonStartColor.value;
                __instance.gameObject.GetComponent<LineRenderer>().endColor = global::UltraColor.Config.redRailcannonEndColor.value;

 
                //__instance.gameObject.GetComponent<LineRenderer>().startColor = UltraColor.Config.redRailcannonGlowStartColor.value;
                //__instance.gameObject.GetComponent<LineRenderer>().endColor = UltraColor.Config.redRailcannonGlowEndColor.value;
                break;
            default: break;
        }

    }

}
