using BepInEx;
using Configgy;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ConfiggyTest
{
    //[BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("ConfiggyTester", "ConfiggyTester", "0.0.1")]
    public sealed class Plugin : BaseUnityPlugin
    {
        public static string? AssetPath;
        private ConfigBuilder config;
        [Configgable("Shotgun")]
        private static ConfigColor ColorTest = new ConfigColor(Color.red);
        public static Material? jimmat;
        private static bool debugMode;
        public static string aapath = "hello";
        public void Awake()
        {
            config = new ConfigBuilder("ConfiggyTester", "ConfiggyTester");
            config.BuildAll();

            AssetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
            if (!Directory.Exists(AssetPath))
            {
                AssetPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            debugMode = false;

            Harmony.CreateAndPatchAll(this.GetType());

        }

        public void Start()
        {
            ConfiggyTest.SetLogger(this.Logger);

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Start))]
        public static void RecolorShotgunShot(Shotgun __instance)
        {
            var color = ColorTest.Value;
            __instance.bullet.GetComponent<TrailRenderer>().startColor = ColorTest.Value;
            __instance.bullet.GetComponent<TrailRenderer>().endColor = ColorTest.Value;

            if (true)
            {
                var light = __instance.muzzleFlash.GetComponentInChildren<Light>();
                light.color = color;
                var muzzleFlashes = __instance.muzzleFlash.GetComponentsInChildren<SpriteRenderer>();
                foreach (var muzzle in muzzleFlashes)
                {
                    muzzle.color = color;
                };
            }

                var mr = __instance.bullet.GetComponent<MeshRenderer>();
                Material newMaterial = new Material(mr.material);
            newMaterial.color = ColorTest.Value;
                __instance.bullet.GetComponent<MeshRenderer>().material = newMaterial;

        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Grenade), nameof(Grenade.PlayerRideStart))]
        private static void SetIsRocketRiding(Grenade __instance)
        {
            isRidingRocket = true;
            ridingRocket = __instance;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Grenade), nameof(Grenade.PlayerRideEnd))]
        private static void DismountRocket(Grenade __instance)
        {
            isRidingRocket = false;

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Grenade), nameof(Grenade.Update))]
        private static void SetRocketRidingRotation(Grenade __instance)
        {
            if (isRidingRocket)
            rocketPos = __instance.transform;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CameraController), nameof(Update))]
        private static bool HackCameraPre(CameraController __instance)
        {
            if (isRidingRocket) __instance.transform.rotation = rocketPos.rotation;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CameraController), nameof(Update))]
        private static void HackCamera(CameraController __instance)
        {
            if (isRidingRocket) __instance.transform.rotation = rocketPos.rotation;
            MonoSingleton<NewMovement>.Instance.gc.transform.rotation = rocketPos.rotation;
        }

        public void Update()
        {
            _ = ConfiggyTest.Instance;
        }

    }
}