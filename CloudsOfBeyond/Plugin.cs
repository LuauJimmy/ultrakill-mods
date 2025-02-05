using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CloudsOfBeyond
{
    //[BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginInfo.guid, PluginInfo.name, "0.0.4")]
    public sealed class Plugin : BaseUnityPlugin
    {
        public static string? AssetPath;

        public static Material? jimmat;
        private static bool debugMode;
        public static string aapath = "hello";
        public void Awake()
        {
            AssetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
            if (!Directory.Exists(AssetPath))
            {
                AssetPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            debugMode = false;

            Harmony.CreateAndPatchAll(this.GetType());

            if (debugMode) Harmony.CreateAndPatchAll(typeof(DebugPatches));

            Addressables.LoadContentCatalogAsync($"{AssetPath}\\catalog.json").WaitForCompletion();

            Addressables.UpdateCatalogs();

            Addressables.LoadAssetAsync<Material>("Assets/fbm.mat").Completed += (h) =>
            {
                Debug.Log("Successfully loaded skybox material");
                jimmat = h.Result;
            };

        }

        public void Start()
        {
            CloudsOfBeyond.SetLogger(this.Logger);

        }

        public void Update()
        {
            _ = CloudsOfBeyond.Instance;
        }

        private static Vector4 CreateCloudColorVector()
        {
            System.Random r = new System.Random();
            var budget = 2f;
            float x = 0f, y = 0f, z = 0f;
            while(budget > 0)
            {
                var redAmt = (float)r.NextDouble() * budget;
                x += redAmt;
                budget -= redAmt;
                var greenAmt = (float)r.NextDouble() * budget;
                y += greenAmt;
                budget -= greenAmt;
                var blueAmt = (float)r.NextDouble() * budget;
                z += blueAmt;
                budget -= blueAmt;
            }
            return new Vector4(x, y, z, 0f);

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EndlessGrid), nameof(EndlessGrid.NextWave))]
        private static void UpdateSkyboxMaterialAfterWave(EndlessGrid __instance)
        {
            if (__instance.specialAntiBuffer == 0) Debug.Log("SpecialAntiBuffer is 0\n\n\n\n");
            var olm = (OutdoorLightMaster)FindObjectsOfType(typeof(OutdoorLightMaster))[0];
            olm.UpdateSkyboxMaterial();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(OutdoorLightMaster), nameof(OutdoorLightMaster.UpdateSkyboxMaterial))]
        private static void UpdateSkyboxMaterialInit(OutdoorLightMaster __instance)
        {
            if (jimmat == null) return;

            System.Random r = new System.Random();
            
            var XspeedCoefficient = r.NextDouble() * 0.4;
            if (r.Next(0, 2) == 1) XspeedCoefficient *= -1;

            var YspeedCoefficient = r.NextDouble() * 0.25;
            
            var deformCoefficient = r.NextDouble() * 8;

            jimmat.SetFloat("_XSpeedCoefficient", (float)XspeedCoefficient);
            jimmat.SetFloat("_YSpeedCoefficient", (float)YspeedCoefficient);
            jimmat.SetFloat("_DeformCoefficient", (float)deformCoefficient);
            jimmat.SetVector("_Color", CreateCloudColorVector());
            __instance.skyboxMaterial = jimmat;
        }

    }
}