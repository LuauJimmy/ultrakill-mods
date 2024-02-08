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
using EffectChanger.Weapons;
namespace UltraColor;
using PluginInfo = EffectChanger.PluginInfo;

[BepInPlugin(PluginInfo.guid, PluginInfo.name, PluginInfo.version)]
public sealed class Plugin : BaseUnityPlugin
{
    public sealed class AssetDir : SortedDictionary<string, object>;

    public static string? workingDir;
    public static string? ultraColorCatalogPath;
    public static Texture2D? blankExplosionTexture;
    public static Sprite? blankMuzzleFlashSprite;
    public static Sprite? muzzleFlashInnerBase;
    public static Sprite? blankMuzzleFlashShotgunSprite;
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
        blankMuzzleFlashShotgunSprite = Utils.LoadPNG("BepInEx\\plugins\\Ultracolor\\Assets\\muzzleflashshotgunblank.png");
        muzzleFlashInnerBase = Utils.LoadPNG("BepInEx\\plugins\\Ultracolor\\Assets\\muzzleflash-innerbase.png");
        Settings.Init(this.Config);
        Harmony.CreateAndPatchAll(this.GetType());
        Harmony.CreateAndPatchAll(typeof(_Shotgun));
        Harmony.CreateAndPatchAll(typeof(_Revolver));
        Harmony.CreateAndPatchAll(typeof(_Nailgun));
        Harmony.CreateAndPatchAll(typeof(_RocketLauncher));
        Harmony.CreateAndPatchAll(typeof(_Railcannon));
        
        
        if (debugMode) { Harmony.CreateAndPatchAll(typeof(DebugPatches)); }


        workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
    //        ps.startColor = Color.red;
    //    }
    //}

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
}