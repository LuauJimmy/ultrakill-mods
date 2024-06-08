using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UKHitMarker;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using PluginInfo = UKHitMarker.PluginInfo;

namespace UKHitMarker
{
    //[BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("luaujimmy.UKHitMarker", "UKHitMarker", PluginInfo.version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        private static string AssetPath;
        private static bool debugMode;

        private static GameObject crosshair;
        private static GameObject hitMarker;
        private static Vector3 hitMarkerPos;
        private static TMPro.TextMeshProUGUI textComp;
        public static Sprite hitMarkerSprite;

        public static List<EnemyIdentifier> killMarkerEIDs;

        public static string[] invalidHitters = ["fire",];

        public static T Fetch<T>(string key)
        {
            return Addressables.LoadAssetAsync<T>((object)key).WaitForCompletion();
        }

        public void Awake()
        {
            AssetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
            debugMode = false;
            Settings.Init(this.Config);

            Harmony.CreateAndPatchAll(this.GetType());
            hitMarkerSprite = LoadPNG($"{AssetPath}\\hitmarker.png");

        }

        public void Start()
        {
            UKHitMarker.SetLogger(this.Logger);

        }

        public void Update()
        {
            _ = UKHitMarker.Instance;
        }
        public static Sprite LoadPNG(string filePath)
        {
            Debug.Log($"trying filePath: {filePath}");
            Texture2D Tex = null;
            byte[] FileData;
            if (File.Exists(filePath))
            {
                FileData = File.ReadAllBytes(filePath);
                Tex = new Texture2D(2, 2);
                Tex.LoadImage(FileData);
                Tex.filterMode = FilterMode.Point;
            }

            return Sprite.Create(Tex, new Rect(0.0f, 0.0f, Tex.width, Tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        [HarmonyPatch(typeof(Crosshair), "Start")]
        [HarmonyPrefix]
        static void AddHitMarkerComponent()
        {
            if (!Settings.hitMarkerEnabled.value) return;

            crosshair = GameObject.Find("Canvas/Crosshair Filler/Crosshair");

            hitMarker = new GameObject("HitMarker");
            hitMarker = Instantiate(hitMarker, crosshair.transform);
            hitMarker.SetActive(true);
            hitMarkerPos = new Vector3(
                hitMarker.transform.localPosition.x,
                hitMarker.transform.localPosition.y,
                hitMarker.transform.localPosition.z
            );
            hitMarker.transform.localPosition = hitMarkerPos;

            hitMarker.AddComponent<HudFader>();

        }

        [HarmonyPatch(typeof(EnemyIdentifier), nameof(EnemyIdentifier.DeliverDamage))]
        [HarmonyPostfix]
        static void UpdateHitMarker(EnemyIdentifier __instance, float multiplier)
        {
            try
            {
                if (__instance.dead || __instance.blessed) return;
                var hitter = __instance.hitter;
                //Debug.Log($"hitter {hitter} Mult {multiplier}");
                if (invalidHitters.Contains(hitter) || multiplier <= 0.15) return;
                hitMarker.GetComponent<HudFader>().ShowHitMarker();
            }
            catch
            {

            }
        }

        //[HarmonyPatch(typeof(TimeController), nameof(TimeController.ParryFlash))]
        //[HarmonyPostfix]
        //static void UpdateParryMarker(TimeController __instance)
        //{
        //    hitMarker.GetComponent<HudFader>().ShowParryMarker();
        //}

        [HarmonyPatch(typeof(EnemyIdentifier), nameof(EnemyIdentifier.Death), typeof(bool))]
        [HarmonyPrefix]
        static void UpdateKillMarker(EnemyIdentifier __instance, bool fromExplosion)
        {
            //Debug.Log("fired");
            if (__instance.dead) return;
            hitMarker.GetComponent<HudFader>().ShowKillMarker();
        }

        

    }
}