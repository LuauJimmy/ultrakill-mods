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
            //Harmony.CreateAndPatchAll(typeof(_Shotgun));
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

            Addressables.LoadAssetAsync<Material>("Assets/fbm.mat").Completed += (h) =>
            {
                Debug.Log("\n\n\n");
                jimmat = h.Result;
            };

            Addressables.LoadAssetAsync<Shader>("Assets/fbm.shader").Completed += (h) =>
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
        [HarmonyPatch(typeof(Revolver), nameof(Revolver.ThrowCoin))]
        private static void CoinDebug(Revolver __instance)
        {
            var olm = (OutdoorLightMaster)FindObjectsOfType(typeof(OutdoorLightMaster))[0];
            olm.UpdateSkyboxMaterial();
        }



        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(OutdoorLightMaster), nameof(OutdoorLightMaster.UpdateSkyboxMaterial))]
        //private static void UpdateSkyboxMaterial(OutdoorLightMaster __instance)
        //{

        //    Vector4 rand = Random.insideUnitSphere;
        //    rand.z = 1;
        //    __instance.skyboxMaterial.SetVector("_Color", rand);

        //}

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EndlessGrid), nameof(EndlessGrid.NextWave))]
        private static void UpdateSkyboxMaterialAfterWave(EndlessGrid __instance)
        {
            var olm = (OutdoorLightMaster)FindObjectsOfType(typeof(OutdoorLightMaster))[0];
            olm.UpdateSkyboxMaterial();

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(OutdoorLightMaster), nameof(OutdoorLightMaster.UpdateSkyboxMaterial))]
        private static void UpdateSkyboxMaterialInit(OutdoorLightMaster __instance)
        {
            //jimmat.SetFloat("_Color", 0f);
            var newcol = Random.insideUnitSphere;
            jimmat.SetVector("_Color", new Vector4(newcol.x, newcol.y, newcol.z, 1));
            __instance.skyboxMaterial = jimmat;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RevolverAnimationReceiver), "Start")]
        private static void RecolorRevolverToUniverse(RevolverAnimationReceiver __instance)
        {
            var mrs = __instance.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer mr in mrs)
            {
                //mr.material = universeMat;
            }
        }

    }
}