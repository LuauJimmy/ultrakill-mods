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
using UnityEngine.Animations;
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

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Revolver), nameof(Revolver.ThrowCoin))]
        //private static void CoinDebug(Revolver __instance)
        //{
        //    var olm = (OutdoorLightMaster)FindObjectsOfType(typeof(OutdoorLightMaster))[0];
        //    olm.UpdateSkyboxMaterial();
        //}

        private static Vector4 CreateCloudColorVector()
        {
            //float minbright = 0.4f;
            //var newcol = Random.insideUnitSphere;
            //Vector4 outvec = new Vector4(
            //    Mathf.Max(newcol.x, minbright),
            //    Mathf.Max(newcol.y, minbright),
            //    Mathf.Max(newcol.z, minbright),
            //    0f
            //);
            //return outvec;
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

            Debug.Log("-----------");
            Debug.Log(XspeedCoefficient);
            Debug.Log(YspeedCoefficient);
            jimmat.SetFloat("_XSpeedCoefficient", (float)XspeedCoefficient);
            jimmat.SetFloat("_YSpeedCoefficient", (float)YspeedCoefficient);
            jimmat.SetFloat("_DeformCoefficient", (float)deformCoefficient);
            jimmat.SetVector("_Color", CreateCloudColorVector());
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