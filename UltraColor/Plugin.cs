using System;
using System.Collections.Generic;

using BepInEx;

using HarmonyLib;

using UnityEngine;
using UnityEngine.AddressableAssets;
using EffectChanger.Enum;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.Experimental.GlobalIllumination;

namespace UltraColor;

[BepInPlugin("luaujimmy.UltraColor", "UltraColor", "0.0.1")]
public sealed class Plugin : BaseUnityPlugin
{
    public sealed class AssetDir : SortedDictionary<string, object>;


    public static string workingDir;
    public static string ultraColorCatalogPath;

    private static Texture2D purpleExplosionTexture;


    public static T Fetch<T>(string key)
    {
        return Addressables.LoadAssetAsync<T>((object)key).WaitForCompletion();
    }

    public void Awake()
    {
        purpleExplosionTexture = Utils.LoadTexture("BepInEx\\plugins\\Ultracolor\\Assets\\explosion_purple.png");
        Settings.Init(this.Config);
        Harmony.CreateAndPatchAll(this.GetType());


        workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        //PostAwake();

    }

    public void PostAwake()
    {
        Addressables.InitializeAsync().WaitForCompletion();
        ultraColorCatalogPath = Path.Combine(workingDir, "Assets");
        Addressables.LoadContentCatalogAsync(Path.Combine(ultraColorCatalogPath, "catalog.json"), true).WaitForCompletion();

        Material m = Fetch<Material>("Assets/Explosion Purple.mat");
        Debug.Log(m);
        Debug.Log("PostAwake method completed successfully.");
    }


    public void checkAddress(string address)
    {
        Addressables.LoadResourceLocationsAsync(address).Completed += checkAddressHandle =>
        {
            //If the list is greater than zero, the address is good.
            if (checkAddressHandle.Result.Count > 0)
            {
                Debug.Log($"Found {checkAddressHandle.Result.Count} assets");
                foreach (var res in checkAddressHandle.Result)
                {
                    Debug.Log(res);
                }
            }
            //The address is bad
            else
            {
                Debug.Log("Didn't find the address");
            }
        };
    }


    public void Start()
    {
        UltraColor.SetLogger(this.Logger);

    }

    public void Update()
    {
        _ = UltraColor.Instance;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shotgun), "Start")]
    static void exp(Shotgun __instance)
    {
        var exp = __instance.explosion;

        var mr = exp.GetComponentsInChildren<MeshRenderer>();

        var s8 = exp.transform.Find("Sphere_8");

        var pl = s8.transform.Find("Point Light").GetComponent<Light>();

        pl.color = Settings.shotgunMuzzleFlashPointLightColor.value;

        var newMat = new Material(mr[0].material)
        {
            mainTexture = purpleExplosionTexture
        };

        mr[0].material = newMat;

        var explosionRenderers = __instance.explosion.gameObject.GetComponentsInChildren<MeshRenderer>();
        explosionRenderers[0].material = newMat;

    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shotgun), "Start")]
    static void RecolorShotgunProjectile(Shotgun __instance)
    {
        __instance.bullet.GetComponent<TrailRenderer>().startColor = Settings.shotgunProjectileStartColor.value;
        __instance.bullet.GetComponent<TrailRenderer>().endColor = Settings.shotgunProjectileEndColor.value;

        if (Settings.shotgunMuzzleFlashColor.value != ColorHelper.MuzzleFlash.Default)
        {
            Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(Settings.shotgunMuzzleFlashColor.value);
            __instance.muzzleFlash.GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
            __instance.muzzleFlash.GetComponent<Light>().color = Settings.shotgunMuzzleFlashPointLightColor.value;
        }

        if (Settings.shotgunBulletColor.value != ColorHelper.BulletColor.Default)
        {
            Material newMaterial = ColorHelper.LoadBulletColor(Settings.shotgunBulletColor.value);
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
                __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.magnetNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.magnetNailgunTrailEndColor.value;

                break;
            case "Nailgun Overheat(Clone)":
                __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.overheatNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.overheatNailgunTrailEndColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().startColor = Settings.overheatNailgunHeatedNailTrailStartColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().endColor = Settings.overheatNailgunHeatedNailTrailEndColor.value;

                break;
            case "Sawblade Launcher Magnet(Clone)":
                __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.altMagnetNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.altMagnetNailgunTrailEndColor.value;
                break;
            case "Sawblade Launcher Overheat(Clone)":
                __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.altOverheatNailgunTrailStartColor.value;
                __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.altOverheatNailgunTrailEndColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().startColor = Settings.altOverheatNailgunHeatedNailTrailStartColor.value;
                __instance.heatedNail.GetComponent<TrailRenderer>().endColor = Settings.altOverheatNailgunHeatedNailTrailEndColor.value;
                break;

        }
    }


    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(ExplosionController), "Start")]
    //static void RecolorExplosion(ExplosionController __instance)
    //{
    //    if (__instance.gameObject.name == "Explosion(Clone)")
    //    {
    //        //if (global::UltraColor.Config.smallExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

    //        //var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.smallExplosionColor.value);
    //        //var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
    //        //explosionRenderers[0].material = newExplosionMat;

    //        var mr = __instance.GetComponentsInChildren<MeshRenderer>();

    //        var newMat = new Material(mr[0].material);

    //        newMat.mainTexture = explosionTexture;

    //        mr[0].material = newMat;

    //        var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
    //        explosionRenderers[0].material = newMat;
    //    }
    //    else if (__instance.gameObject.name == "Explosion Malicious Railcannon(Clone)")
    //    {
    //        if (global::UltraColor.Config.maliciousExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

    //        var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.maliciousExplosionColor.value);
    //        var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
    //        explosionRenderers[0].material = newExplosionMat;
    //    }
    //    else if (__instance.gameObject.name == "Explosion Super(Clone)")
    //    {
    //        if (global::UltraColor.Config.nukeExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

    //        var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.nukeExplosionColor.value);
    //        var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
    //        explosionRenderers[0].material = newExplosionMat;
    //    }

    //}

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Grenade), "Start")]
    static void RecolorGrenadeSprite(Grenade __instance)
    {
        if (Settings.shotgunGrenadeSpriteColor.value == ColorHelper.MuzzleFlash.Default) return;
        Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(Settings.shotgunGrenadeSpriteColor.value);
        __instance.transform.Find("GameObject").GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
    }

    // Need to patch ReadyGun instead of Start because lots of weapons use the same normal fire mode projectile
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Revolver), "ReadyGun")]
    static void RecolorRevolverMuzzleFlash(Revolver __instance)
    {
        
        ColorHelper.MuzzleFlash spriteToLoad;
        switch (__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                if (Settings.piercerRevolverMuzzleFlashColor == default) return;
                spriteToLoad = Settings.piercerRevolverMuzzleFlashColor.value;
                break;
            case "Revolver Twirl(Clone)":
                if (Settings.sharpShooterMuzzleFlashColor == default) return;
                spriteToLoad = Settings.sharpShooterMuzzleFlashColor.value;
                break;
            case "Revolver Ricochet(Clone)":
                if (Settings.marksmanMuzzleFlashColor == default) return;
                spriteToLoad = Settings.marksmanMuzzleFlashColor.value;
                break;
            case "Alternative Revolver Ricochet(Clone)":
                if (Settings.altMarksmanMuzzleFlashColor == default) return;
                spriteToLoad = Settings.altMarksmanMuzzleFlashColor.value;
                break;
            case "Alternative Revolver Twirl(Clone)":
                if (Settings.altSharpShooterMuzzleFlashColor == default) return;
                spriteToLoad = Settings.altSharpShooterMuzzleFlashColor.value;
                break;
            case "Alternative Revolver Pierce(Clone)":
                if (Settings.altPiercerRevolverMuzzleFlashColor == default) return;
                spriteToLoad = Settings.altPiercerRevolverMuzzleFlashColor.value;
                break;
            default: return;
        }
        Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(spriteToLoad);
        var muzzleFlashes = __instance.revolverBeam.GetComponentsInChildren<SpriteRenderer>();
        foreach (var muzzle in muzzleFlashes) { muzzle.sprite = newSprite; }

    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Revolver), "ReadyGun")]
    static void RecolorRevolverBeam(Revolver __instance)
    {
        switch (__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.piercerRevolverBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.piercerRevolverBeamEndColor.value;
                break;
            case "Revolver Twirl(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.sharpShooterBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.sharpShooterBeamEndColor.value;
                break;
            case "Revolver Ricochet(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.marksmanBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.marksmanBeamEndColor.value;
                break;
            case "Alternative Revolver Ricochet(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.altMarksmanBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.altMarksmanBeamEndColor.value;
                break;
            case "Alternative Revolver Twirl(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.altSharpShooterBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.altSharpShooterBeamEndColor.value;
                break;
            case "Alternative Revolver Pierce(Clone)":
                __instance.revolverBeam.GetComponent<LineRenderer>().startColor = Settings.altPiercerRevolverBeamStartColor.value;
                __instance.revolverBeam.GetComponent<LineRenderer>().endColor = Settings.altPiercerRevolverBeamEndColor.value;
                break;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RocketLauncher), "OnEnable")]
    static void RecolorRevolverBeam(RocketLauncher __instance)
    {
        Debug.Log("OnEnable");
        switch (__instance.gameObject.name)
        {
            case "Rocket Launcher Freeze(Clone)":
                __instance.rocket.GetComponent<TrailRenderer>().startColor = Settings.freezeRocketLauncherTrailStartColor.value;
                __instance.rocket.GetComponent<TrailRenderer>().endColor= Settings.freezeRocketLauncherTrailEndColor.value;
                break;
            case "Rocket Launcher Cannonball(Clone)":
                __instance.rocket.GetComponent<TrailRenderer>().startColor = Settings.cannonballRocketLauncherTrailStartColor.value;
                __instance.rocket.GetComponent<TrailRenderer>().endColor = Settings.cannonballRocketLauncherTrailEndColor.value;
                break;

        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Revolver), "Start")]
    static void RecolorRevolverChargeBeam(Revolver __instance)
    {
        switch (__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.sharpShooterChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.sharpShooterChargeBeamEndColor.value;


                if (Settings.altPiercerRevolverChargeEffectColor.value == ColorHelper.BulletColor.Default) return;
                // Replace revolver charge effect material
                Material newMat = ColorHelper.LoadBulletColor(Settings.altPiercerRevolverChargeEffectColor.value);
                var c = __instance.transform.Find("Revolver_Rerigged_Alternate/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint (1)/ChargeEffect");
                c.GetComponent<MeshRenderer>().material = newMat;
                break;
            case "Alternative Revolver Pierce(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.altPiercerRevolverChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.altPiercerRevolverChargeBeamEndColor.value;

                if (Settings.altPiercerRevolverChargeEffectColor.value == ColorHelper.BulletColor.Default) return;


                var altPiercerChargeEffect = __instance.transform.Find("Revolver_Rerigged_Alternate/Armature/Upper Arm/Forearm/Hand/Revolver_Bone/ShootPoint (1)/ChargeEffect");
                altPiercerChargeEffect.GetComponent<MeshRenderer>().material = ColorHelper.LoadBulletColor(Settings.altPiercerRevolverChargeEffectColor.value); ;
                break;
            case "Revolver Twirl(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.sharpShooterChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.sharpShooterChargeBeamEndColor.value;
                break;
            case "Alternative Revolver Twirl(Clone)":
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().startColor = Settings.altSharpShooterChargeBeamStartColor.value;
                __instance.revolverBeamSuper.GetComponent<LineRenderer>().endColor = Settings.altSharpShooterChargeBeamEndColor.value;
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
                __instance.gameObject.GetComponentInParent<LineRenderer>().startColor = Settings.revolverCoinRicochetBeamStartColor.value;
                __instance.gameObject.GetComponentInParent<LineRenderer>().endColor = Settings.revolverCoinRicochetBeamEndColor.value;
                break;
            case "Railcannon Beam(Clone)":
                __instance.gameObject.GetComponent<LineRenderer>().startColor = Settings.blueRailcannonStartColor.value;
                __instance.gameObject.GetComponent<LineRenderer>().endColor = Settings.blueRailcannonEndColor.value;
                break;
            case "Railcannon Beam Malicious(Clone)":
                __instance.gameObject.GetComponent<LineRenderer>().startColor = Settings.redRailcannonStartColor.value;
                __instance.gameObject.GetComponent<LineRenderer>().endColor = Settings.redRailcannonEndColor.value;


                //__instance.gameObject.GetComponent<LineRenderer>().startColor = UltraColor.Config.redRailcannonGlowStartColor.value;
                //__instance.gameObject.GetComponent<LineRenderer>().endColor = UltraColor.Config.redRailcannonGlowEndColor.value;
                break;
            default: break;
        }

    }

}