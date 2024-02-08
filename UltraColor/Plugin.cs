using BepInEx;
using EffectChanger;
using EffectChanger.Enum;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static EffectChanger.Enum.ColorHelper;

namespace UltraColor;

[BepInPlugin("luaujimmy.UltraColor", "UltraColor", "0.0.1")]
public sealed class Plugin : BaseUnityPlugin
{
    public sealed class AssetDir : SortedDictionary<string, object>;

    public static string workingDir;
    public static string ultraColorCatalogPath;
    private static Texture2D blankExplosionTexture;
    private static Sprite blankMuzzleFlashSprite;
    private static Sprite muzzleFlashInnerBase;
    private static Color _revolverMuzzleFlashColor;
    private static bool debugMode;

    public static T Fetch<T>(string key)
    {
        return Addressables.LoadAssetAsync<T>((object)key).WaitForCompletion();
    }

    public void Awake()
    {
        debugMode = false;
        blankExplosionTexture = Utils.LoadTexture("BepInEx\\plugins\\Ultracolor\\Assets\\explosion_blank.png");
        blankMuzzleFlashSprite = Utils.LoadPNG("BepInEx\\plugins\\Ultracolor\\Assets\\muzzleflashblank2.png");
        muzzleFlashInnerBase = Utils.LoadPNG("BepInEx\\plugins\\Ultracolor\\Assets\\muzzleflash-innerbase.png");
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

    public void Start()
    {
        UltraColor.SetLogger(this.Logger);
    }

    public void Update()
    {
        _ = UltraColor.Instance;
    }

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(RemoveOnTime), "Start")]
    //private static void RecolorLaserHitParticles(RemoveOnTime __instance)
    //{
    //    if (__instance.gameObject.name == "LaserHitParticle(Clone)")
    //    {
    //        var ps = __instance.gameObject.GetComponentInChildren<ParticleSystem>();
    //        ps.startColor = Color.red;
    //    }
    //}

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Coin), "Start")]
    private static void CoinDebugMethod(Coin __instance)
    {
        if (!debugMode) return;
        var assetPaths = Addressables.ResourceLocators
            .SelectMany(locator => locator.Keys)
            .Distinct()
            .OfType<string>()
            .Where(key => key.Contains('/'));

        Utils.DumpAssetPaths(assetPaths);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shotgun), "Start")]
    private static bool exp(Shotgun __instance)
    {
        if (!Settings.smallExplosionEnabled.value) return true;
        var exp = __instance.explosion;

        var mr = exp.GetComponentsInChildren<MeshRenderer>();

        var s8 = exp.transform.Find("Sphere_8");

        var pl = s8.transform.Find("Point Light").GetComponent<Light>();

        pl.color = Settings.shotgunMuzzleFlashPointLightColor.value;
        var newMat = new Material(mr[0].material)
        {
            mainTexture = blankExplosionTexture,
            shaderKeywords = ["_FADING_ON", "_EMISSION"]
        };

        newMat.color = Settings.smallExplosionColor.value;

        mr[0].material = newMat;

        var explosionRenderers = __instance.explosion.gameObject.GetComponentsInChildren<MeshRenderer>();
        explosionRenderers[0].material = newMat;
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shotgun), "Start")]
    private static bool RecolorShotgunProjectile(Shotgun __instance)
    {
        if (!Settings.shotgunEnabled.value) return true;
        __instance.bullet.GetComponent<TrailRenderer>().startColor = Settings.shotgunProjectileStartColor.value;
        __instance.bullet.GetComponent<TrailRenderer>().endColor = Settings.shotgunProjectileEndColor.value;

        if (true)
        {
            var color = Settings.shotgunMuzzleFlashColor.value;
            var light = __instance.muzzleFlash.GetComponentInChildren<Light>();
            light.color = color;
            var muzzleFlashes = __instance.muzzleFlash.GetComponentsInChildren<SpriteRenderer>();
            foreach (var muzzle in muzzleFlashes)
            {
                muzzle.sprite = blankMuzzleFlashSprite;
                muzzle.color = color;
            };
        }

        if (Settings.shotgunBulletColor.value != ColorHelper.BulletColor.Default)
        {
            Material newMaterial = ColorHelper.LoadBulletColor(Settings.shotgunBulletColor.value);
            __instance.bullet.GetComponent<MeshRenderer>().material = newMaterial;
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Shotgun), "Shoot")]
    private static void AddMuzzleFlashInnerComponent_Shotgun(Shotgun __instance)
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
    [HarmonyPatch(typeof(ExplosionController), "Start")]
    private static bool RecolorExplosion(ExplosionController __instance)
    {
        //if (__instance.gameObject.name == "Explosion(Clone)")
        //{
        //    //if (global::UltraColor.Config.smallExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

        //    //var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.smallExplosionColor.value);
        //    //var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
        //    //explosionRenderers[0].material = newExplosionMat;

        //    var mr = __instance.GetComponentsInChildren<MeshRenderer>();

        //    var newMat = new Material(mr[0].material);

        //    newMat.mainTexture = explosionTexture;

        //    mr[0].material = newMat;

        //    var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
        //    explosionRenderers[0].material = newMat;
        //}
        if (__instance.gameObject.name == "Explosion Malicious Railcannon(Clone)")
        {
            if (!Settings.maliciousExplosionEnabled.value) return true;
            var mr = __instance.GetComponentsInChildren<MeshRenderer>();
            var newMat = new Material(mr[0].material)
            {
                mainTexture = blankExplosionTexture,
                shaderKeywords = ["_FADING_ON", "_EMISSION"]
            };
            newMat.color = Settings.maliciousExplosionColor.value;
            mr[0].material = newMat;
        }
        else if (__instance.gameObject.name == "Explosion Super(Clone)")
        {
            if (!Settings.nukeExplosionEnabled.value) return true;

            var mr = __instance.GetComponentsInChildren<MeshRenderer>();
            var newMat = new Material(mr[0].material)
            {
                mainTexture = blankExplosionTexture,
                shaderKeywords = ["_FADING_ON", "_EMISSION"]
            };
            newMat.color = Settings.nukeExplosionColor.value;
            mr[0].material = newMat;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Grenade), "Start")]
    private static void RecolorGrenadeSprite(Grenade __instance)
    {
        if (!Settings.shotgunEnabled.value) return;
        Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(Settings.shotgunGrenadeSpriteColor.value);
        __instance.transform.Find("GameObject").GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
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

    // Need to patch ReadyGun instead of Start because lots of weapons use the same normal fire mode projectile
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Revolver), "ReadyGun")]
    private static void RecolorRevolverMuzzleFlash(Revolver __instance)
    {
        Color color;
        switch (__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                if (Settings.piercerRevolverMuzzleFlashColor == default) return;
                color = Settings.piercerRevolverMuzzleFlashColor.value;
                break;

            case "Revolver Twirl(Clone)":
                if (Settings.sharpShooterMuzzleFlashColor == default) return;
                color = Settings.sharpShooterMuzzleFlashColor.value;
                break;

            case "Revolver Ricochet(Clone)":
                if (Settings.marksmanMuzzleFlashColor == default) return;
                color = Settings.marksmanMuzzleFlashColor.value;
                break;

            case "Alternative Revolver Ricochet(Clone)":
                if (Settings.altMarksmanMuzzleFlashColor == default) return;
                color = Settings.altMarksmanMuzzleFlashColor.value;
                break;

            case "Alternative Revolver Twirl(Clone)":
                if (Settings.altSharpShooterMuzzleFlashColor == default) return;
                color = Settings.altSharpShooterMuzzleFlashColor.value;
                break;

            case "Alternative Revolver Pierce(Clone)":
                if (Settings.altPiercerRevolverMuzzleFlashColor == default) return;
                color = Settings.altPiercerRevolverMuzzleFlashColor.value;
                break;

            default: return;
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
        switch (__instance.gameObject.name)
        {
            case "Revolver Pierce(Clone)":
                if (Settings.piercerRevolverChargeMuzzleFlashColor == default) return true;
                muzzleFlashColor = Settings.piercerRevolverChargeMuzzleFlashColor.value;
                break;

            case "Alternative Revolver Pierce(Clone)":
                if (Settings.altPiercerRevolverChargeMuzzleFlashColor == default) return true;
                muzzleFlashColor = Settings.altPiercerRevolverChargeMuzzleFlashColor.value;
                break;

            default: return true;
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
    [HarmonyPatch(typeof(RocketLauncher), "OnEnable")]
    private static void RecolorRockets(RocketLauncher __instance)
    {
        switch (__instance.gameObject.name)
        {
            case "Rocket Launcher Freeze(Clone)":
                if (!Settings.freezeRocketLauncherEnabled.value) return;
                __instance.rocket.GetComponent<TrailRenderer>().startColor = Settings.freezeRocketLauncherTrailStartColor.value;
                __instance.rocket.GetComponent<TrailRenderer>().endColor = Settings.freezeRocketLauncherTrailEndColor.value;
                break;

            case "Rocket Launcher Cannonball(Clone)":
                if (!Settings.cannonballRocketLauncherEnabled.value) return;
                __instance.rocket.GetComponent<TrailRenderer>().startColor = Settings.cannonballRocketLauncherTrailStartColor.value;
                __instance.rocket.GetComponent<TrailRenderer>().endColor = Settings.cannonballRocketLauncherTrailEndColor.value;
                break;
        }
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
    [HarmonyPatch(typeof(Coin), "Start")]
    private static void RecolorCoinTrail(Coin __instance)
    {
        if (!Settings.coinEnabled.value) return;
        __instance.GetComponent<TrailRenderer>().startColor = Settings.revolverCoinTrailStartColor.value;
        __instance.GetComponent<TrailRenderer>().endColor = Settings.revolverCoinTrailEndColor.value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RevolverBeam), "Start")]
    private static void RecolorRevolverBeams(RevolverBeam __instance)
    {
        Debug.Log($"GameObject: {__instance.gameObject}");
        switch (__instance.gameObject.name)
        {
            case "ReflectedBeamPoint(Clone)":
                if (!Settings.marksmanEnabled.value) return;
                __instance.gameObject.GetComponentInParent<LineRenderer>().startColor = Settings.revolverCoinRicochetBeamStartColor.value;
                __instance.gameObject.GetComponentInParent<LineRenderer>().endColor = Settings.revolverCoinRicochetBeamEndColor.value;
                break;

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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RevolverBeam), "Shoot")]
    private static void AddMuzzleFlashInnerComponent_Revolver(RevolverBeam __instance)
    {
        //var go = Instantiate(__instance, __instance.transform);
        //var sr = go.GetComponent<SpriteRenderer>();
        //sr.sprite = muzzleFlashInnerBase;
        //sr.color = new Color(1, 1, 1);
        var muzzleFlashes = __instance.GetComponentsInChildren<SpriteRenderer>();

        foreach (var flash in muzzleFlashes)
        {
            var colorA = flash.GetComponent<SpriteRenderer>().color;
            var obj = Instantiate(flash);
            var interpColor = Color.Lerp(colorA, Color.white, 0.8f);
            interpColor.a = 0.8f;
            obj.GetComponent<SpriteRenderer>().color = interpColor;
            obj.GetComponent<SpriteRenderer>().sprite = muzzleFlashInnerBase;
            obj.transform.position = flash.transform.position;
            obj.transform.rotation = flash.transform.rotation;
            obj.gameObject.AddComponent<MuzzleFlashInnerComponent>();
        }
        //var muzzleflashParent = __instance.transform.Find("MuzzleFlash");
        //var muzzleflashChild = muzzleflashParent.transform.Find("muzzleflash");

        //var colorA = muzzleflashChild.GetComponent<SpriteRenderer>().color;
        //var obj = Instantiate(muzzleflashChild);
        //var interpColor = Color.Lerp(colorA, Color.white, 0.95f);
        //obj.GetComponent<SpriteRenderer>().color = interpColor;
        //obj.GetComponent <SpriteRenderer>().sprite = muzzleFlashInnerBase;
        //obj.transform.position = muzzleflashChild.transform.position;
        //obj.transform.rotation = muzzleflashChild.transform.rotation;
        //obj.gameObject.AddComponent<MuzzleFlashInnerComponent>();
        return;
    }
}