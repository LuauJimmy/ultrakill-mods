using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UltraColor;
using UnityEngine;
using Color = UnityEngine.Color;
namespace EffectChanger.Weapons
{
    public sealed class _RocketLauncher : MonoSingleton<_RocketLauncher>
    {
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Texture2D? basicWhiteTexture => Plugin.basicWhiteTexture;


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
        [HarmonyPatch(typeof(Cannonball), "Start")]
        private static void RecolorCannonballTrail(Cannonball __instance)
        {
            if (!Settings.cannonballRocketLauncherEnabled.value) return;
            var tr = __instance.GetComponentInChildren<TrailRenderer>();
            tr.startColor = Settings.cannonballTrailStartColor.value;
            tr.endColor = Settings.cannonballTrailEndColor.value;
        }

        //[HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.LoadSceneAsync))]
        //[HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.RestartScene))]
        [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.Start))]
        [HarmonyPostfix]
        static void RecolorGasProjectile(RocketLauncher __instance)
        {
            try
            {
                var shad = Resources.FindObjectsOfTypeAll<Shader>().Where(s => s.name == "psx/unlit/unlit").FirstOrDefault();
                //this is fucking retarded but it only runs once so i dont care
                
                GameObject? gasProj = __instance.napalmProjectile.gameObject;
                Debug.Log($"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n {gasProj.gameObject.name}");
                var tr = gasProj.GetComponent<TrailRenderer>();
                tr.material.shader = shad;
                tr.material.color = new Color(1, 1, 1, 1);
                tr.material.mainTexture = basicWhiteTexture;
                tr.startColor = Color.white;
                tr.endColor = new Color(0.8f, 0.8f, 0.8f, 0);
                var muzzleFlashes = __instance.napalmMuzzleFlashTransform.gameObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (var muzzle in muzzleFlashes)
                {
                    muzzle.sprite = blankMuzzleFlashSprite;
                    muzzle.color = Color.white;
                };
                __instance.napalmMuzzleFlashParticles.startColor = new Color(0.85f, 0.85f, 0.8f, 1);
            }
            catch
            {

            }
        }

        [HarmonyPatch(typeof(GasolineStain), nameof(GasolineStain.Awake))]
        [HarmonyPostfix]
        static void RecolorGasStain(GasolineStain __instance)
        {
            MeshRenderer component = __instance.GetComponent<MeshRenderer>();
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            component.GetPropertyBlock(materialPropertyBlock);
            //component.GetPropertyBlock(materialPropertyBlock);
            //materialPropertyBlock.SetFloat("_IsOil", 0f);
            //materialPropertyBlock.SetFloat("_Index", Random.Range(0, 5));
            materialPropertyBlock.SetColor("_Color", Color.white);
            

            component.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
