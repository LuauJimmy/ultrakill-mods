using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UltraShade;
using UltraShade.Weapons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using PluginInfo = UltraShade.PluginInfo;

namespace UltraShade
{
    //[BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("UNIVERSARIO", "UNIVERSARIO", PluginInfo.version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public sealed class AssetDir : SortedDictionary<string, object>;

        private static string AssetPath;

        public static string? workingDir;
        public static string? ultraColorCatalogPath;
        
        public static Material? universeMat;
        public static Shader? universeShader;
        public static Material? jimmat;
        public static Shader? jimshade;
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
            Settings.Init(this.Config);

            Harmony.CreateAndPatchAll(this.GetType());
            Harmony.CreateAndPatchAll(typeof(_Shotgun));
            //Harmony.CreateAndPatchAll(typeof(_Movement));
            //Harmony.CreateAndPatchAll(typeof(EnemyProjectile));

            Addressables.LoadContentCatalogAsync($"{AssetPath}\\catalog.json").WaitForCompletion();

            Addressables.UpdateCatalogs();

            Addressables.LoadAssetAsync<Material>("Assets/Universio.mat").Completed += (h) =>
            {
                Debug.Log("\n\n\n");
                universeMat = h.Result;
            };

            Addressables.LoadAssetAsync<Shader>("Assets/shader2.shader").Completed += (h) =>
            {
                Debug.Log("\n\n\n");
                universeShader = h.Result;
            };

            Addressables.LoadAssetAsync<Material>("Assets/jimmaterial.mat").Completed += (h) =>
            {
                Debug.Log("\n\n\n");
                jimmat = h.Result;
            };

            Addressables.LoadAssetAsync<Shader>("Assets/rolo.shader").Completed += (h) =>
            {
                Debug.Log("\n\n\n");
                jimshade = h.Result;
            };
            


        }

        public void Start()
        {
            UltraShade.SetLogger(this.Logger);

        }

        public void Update()
        {
            _ = UltraShade.Instance;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CustomTextures), nameof(CustomTextures.Start))]
        private static void doobie2(CustomTextures __instance)
        {
            var sky = __instance.skyMaterial;
            //Resources.FindObjectsOfTypeAll<Material>()
            //  .Where(m => m.name == "EndlessSky")
            //    .FirstOrDefault();
            sky.shader = jimshade;

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CustomTextures), nameof(CustomTextures.Start))]
        private static void doobie3(CustomTextures __instance)
        {
            var sky = __instance.skyMaterial;
            //Resources.FindObjectsOfTypeAll<Material>()
            //  .Where(m => m.name == "EndlessSky")
            //    .FirstOrDefault();
            sky.shader = jimshade;

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RevolverAnimationReceiver), "Start")]
        private static void RecolorRevolverToUniverse(RevolverAnimationReceiver __instance)
        {
            var mrs = __instance.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer mr in mrs)
            {
                mr.material = universeMat;
            }
        }

    }
}