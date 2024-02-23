using BepInEx;
using EffectChanger.Enum;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Weapons
{
    public sealed class _Shotgun : MonoSingleton<_Shotgun>
    {
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Sprite? muzzleFlashInnerBase => Plugin.shotgunInnerComponent;
        private static Texture2D? blankExplosionTexture => Plugin.blankExplosionTexture;
        private static Sprite? shotgunMuzzleFlashSprite => Plugin.blankMuzzleFlashShotgunSprite;
        public void Awake()
        {
            
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
                    muzzle.sprite = shotgunMuzzleFlashSprite;
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
                interpColor.a = 0.95f;
                obj.GetComponent<SpriteRenderer>().color = interpColor;
                obj.GetComponent<SpriteRenderer>().sprite = muzzleFlashInnerBase;
                obj.transform.position = __instance.shootPoints[0].transform.position;
                obj.transform.rotation = __instance.shootPoints[0].transform.rotation;
                obj.gameObject.AddComponent<MuzzleFlashInnerComponent>();
            };
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Grenade), "Start")]
        private static void RecolorGrenadeSprite(Grenade __instance)
        {
            if (!Settings.shotgunEnabled.value) return;
            Sprite newSprite = ColorHelper.LoadMuzzleFlashSprite(Settings.shotgunGrenadeSpriteColor.value);
            __instance.transform.Find("GameObject").GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Punch), nameof(Punch.ParryProjectile))]
        private static void RecolorProjectileBoost(Projectile __instance, object[] __args)
        {
            try {
                Projectile proj = (Projectile)__args[0];
                if (proj.playerBullet)
                {
                    Debug.Log(__instance.gameObject);
                    if (proj.TryGetComponent<MeshRenderer>(out var component2) && (bool)component2.material && component2.material.HasProperty("_Color"))
                    {
                        component2.material.SetColor("_Color", Settings.shotgunProjectileBoostStartColor.value);
                    }
                    if (proj.TryGetComponent<TrailRenderer>(out var component3))
                    {
                        component3.startColor = Settings.shotgunProjectileBoostStartColor.value;
                        component3.endColor = Settings.shotgunProjectileBoostEndColor.value;
                        Debug.Log(component3.startColor);
                        Debug.Log(component3.endColor);
                    }
                    if (proj.TryGetComponent<Light>(out var component4))
                    {
                        component4.color = Settings.shotgunProjectileBoostStartColor.value;
                    }
                }
            }
            catch {
                return;
            } 
            
        }
    }
}
