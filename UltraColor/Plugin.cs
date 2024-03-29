﻿using BepInEx;
using EffectChanger;
using EffectChanger.Enum;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EffectChanger.Weapons;
using UnityEngine.Assertions;
using System.Runtime.CompilerServices;
namespace UltraColor;
using PluginInfo = EffectChanger.PluginInfo;

//[BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin("aglooper", "aglooper", PluginInfo.version)]
public sealed class Plugin : BaseUnityPlugin
{
    public sealed class AssetDir : SortedDictionary<string, object>;

    private static string AssetPath;

    public static string? workingDir;
    public static string? ultraColorCatalogPath;
    public static Sprite? chargeBlankSprite;
    public static Texture2D? blankExplosionTexture;
    public static Sprite? blankMuzzleFlashSprite;
    public static Sprite? muzzleFlashInnerBase;
    public static Sprite? chargeBlank;
    public static Sprite? blankMuzzleFlashShotgunSprite;
    public static Sprite? shotgunInnerComponent;
    public static Sprite? defaultMuzzleFlashSprite;
    public static Texture2D? chargeBlankTexture;
    public static Texture2D? basicWhiteTexture;
    public static Texture2D? whiteSparkTexture;
    private static Color _revolverMuzzleFlashColor;
    private static bool debugMode;

    public static T Fetch<T>(string key)
    {
        return Addressables.LoadAssetAsync<T>((object)key).WaitForCompletion();
    }

    public void Awake()
    {
        AssetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        Debug.Log("Pathname: " + AssetPath);
        debugMode = false;
        defaultMuzzleFlashSprite = Utils.LoadPNG($"{AssetPath}\\muzzleflash.png");
        blankExplosionTexture = Utils.LoadTexture($"{AssetPath}\\explosion_blank.png");
        blankMuzzleFlashSprite = Utils.LoadPNG($"{AssetPath}\\muzzleflashblank2.png");
        blankMuzzleFlashShotgunSprite = Utils.LoadPNG($"{AssetPath}\\muzzleflashshotgunblank.png");
        muzzleFlashInnerBase = Utils.LoadPNG($"{AssetPath}\\muzzleflash-innerbase.png");
        chargeBlankSprite = Utils.LoadPNG($"{AssetPath}\\chargeblank.png");
        shotgunInnerComponent = Utils.LoadPNG($"{AssetPath}\\muzzleflashshotguninnercomponent.png");
        chargeBlankTexture = Utils.LoadTexture($"{AssetPath}\\chargeblank.png");
        basicWhiteTexture = Utils.LoadTexture($"{AssetPath}\\basicwhite.png");
        whiteSparkTexture = Utils.LoadTexture($"{AssetPath}\\spark.png");
        Settings.Init(this.Config);

        Harmony.CreateAndPatchAll(this.GetType());
        Harmony.CreateAndPatchAll(typeof(_Shotgun));
        Harmony.CreateAndPatchAll(typeof(_Revolver));
        Harmony.CreateAndPatchAll(typeof(_Nailgun));
        Harmony.CreateAndPatchAll(typeof(_RocketLauncher));
        Harmony.CreateAndPatchAll(typeof(_Railcannon));

        Harmony.CreateAndPatchAll(typeof(_Idol));
        
        
        if (debugMode) { Harmony.CreateAndPatchAll(typeof(DebugPatches)); }


        //workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        //PostAwake();
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
    //        ps.startColor = Settings.altPiercerRevolverBeamEndColor.value;
    //    }
    //}

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Explosion), nameof(Explosion.Start))]
    private static bool RecolorKBExplosion(Explosion __instance)
    {
        if (!Settings.knuckleBlasterEnabled.value) return true;
        Color expColor = Settings.knuckleBlasterExplosionColor.value;
        Color lightColor = Settings.knuckleBlasterLightColor.value;
        Color shockwaveColor = Settings.knuckleBlasterShockwaveSpriteColor.value;
        if (__instance.transform.parent.name == "Explosion Wave(Clone)")
        {
            var trans = __instance.transform.parent;
            trans.GetComponentInChildren<Light>().color = lightColor;
            trans.GetComponentInChildren<SpriteRenderer>().color = shockwaveColor;
            var mr = __instance.GetComponentInChildren<MeshRenderer>();
            var newMat = new Material(mr.material)
            {
                //mainTexture = blankExplosionTexture,
                //shaderKeywords = [/*"_FADING_ON",*/ "_EMISSION"],
                color = expColor,
            };
            mr.material = newMat;
        }
        return true;
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
            //__instance.transform.Find("Sphere_8").gameObject.AddComponent<RendererFader>();
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
            //__instance.transform.Find("Sphere_8").gameObject.AddComponent<RendererFader>();
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ExplosionController), "Start")]
    private static void expFader(ExplosionController __instance)
    {
        var expMesh = __instance.transform.Find("Sphere_8");
        if (expMesh == null) return;
        __instance.transform.Find("Sphere_8").gameObject.AddComponent<ExplosionFader>();
    }
}