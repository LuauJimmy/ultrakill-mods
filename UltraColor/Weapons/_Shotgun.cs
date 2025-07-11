﻿using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;
using UnityEngine.Rendering;

namespace EffectChanger.Weapons
{
    public sealed class _Shotgun : MonoSingleton<_Shotgun>
    {
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Sprite? muzzleFlashInnerBase => Plugin.shotgunInnerComponent;
        private static Texture2D? blankExplosionTexture => Plugin.blankExplosionTexture;
        private static Texture2D? chargeBlankTexture => Plugin.chargeBlankTexture;
        private static Texture2D? basicWhiteTexture => Plugin.basicWhiteTexture;
        private static Texture2D? whiteSparkTexture => Plugin.whiteSparkTexture;
        private static Sprite? chargeBlankSprite => Plugin.chargeBlankSprite;
        private static Sprite? shotgunMuzzleFlashSprite => Plugin.blankMuzzleFlashShotgunSprite;
        private static Texture2D? hammerImpactSprite => Plugin.enrageEffectTextureBlank;

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

            if (Settings.shotgunBulletColor.value != Settings.shotgunBulletColor.defaultValue)
            {
                var mr = __instance.bullet.GetComponent<MeshRenderer>();
                Material newMaterial = new Material(mr.material);
                newMaterial.color = Settings.shotgunBulletColor.value;
                newMaterial.mainTexture = basicWhiteTexture;
                __instance.bullet.GetComponent<MeshRenderer>().material = newMaterial;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.Awake))]
        private static bool RecolorHammerParticles(ShotgunHammer __instance)
        {
            if (!Settings.hammerEnabled.value) return true;
            foreach (GameObject item in __instance.hitImpactParticle)
            {
                var srs = item.GetComponentsInChildren<SpriteRenderer>();
                srs[0].color = Settings.hammerSpriteOuterColor.value;
                srs[1].color = Settings.hammerSpriteInnerColor.value;
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Chainsaw), nameof(Chainsaw.Start))]
        private static void RecolorChainsawSprite(Chainsaw __instance)
        {
            if (!Settings.chainsawEnabled.value) return;
            try
            {
                var sr = __instance.gameObject.GetComponentInChildren<SpriteRenderer>();
                sr.sprite = blankMuzzleFlashSprite;
                var colorA = Settings.chainsawSpriteColor.value;
                colorA.a *= 0.85f;
                sr.sprite = muzzleFlashInnerBase;
                
                sr.color = colorA;

                var srGo = __instance.gameObject.transform.Find("New Sprite");
                
                var obj = new GameObject()
                {
                    name = "New Sprite",
                };
                var newSr = obj.AddComponent<SpriteRenderer>();
                newSr.sprite = muzzleFlashInnerBase;
                var interpColor = Color.Lerp(colorA, Color.white, 0.8f);
                interpColor.a = 0.85f;
                newSr.color = interpColor;
                obj.transform.parent = srGo.transform;
                obj.gameObject.AddComponent<MuzzleFlashInnerComponentChainsaw>();
                
            }
            catch(Exception e)
            {
                Debug.Log(e.StackTrace);
            }

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Chainsaw), nameof(Chainsaw.TouchPlayer))]
        private static void TouchPlayer(Chainsaw __instance)
        {
            try
            {
                var mfic = __instance.gameObject.GetComponentInChildren<MuzzleFlashInnerComponentChainsaw>();
                mfic.isTouchingPlayer = true;
                mfic.transform.localScale = Vector3.one * 0.5f;
            }
            catch(Exception e)
            {
                Debug.Log(e.StackTrace);
            }

        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Chainsaw), nameof(Chainsaw.GetPunched))]
        private static void GetPunched(Chainsaw __instance)
        {
            try
            {
                var mfic = __instance.gameObject.GetComponentInChildren<MuzzleFlashInnerComponentChainsaw>();
                mfic.isTouchingPlayer = false;
                mfic.transform.localScale = Vector3.one * 0.5f;
            }
            catch(Exception e)
            {
                Debug.Log(e.StackTrace);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Chainsaw), nameof(Chainsaw.Start))]
        private static void RecolorChainsawTrail(Chainsaw __instance)
        {
            if (!Settings.chainsawEnabled.value) return;
            try
            {
                var tr = __instance.gameObject.GetComponent<TrailRenderer>();
                tr.startColor = Settings.chainsawTrailStartColor.value;
                tr.endColor = Settings.chainsawTrailEndColor.value;
            }
            catch(Exception e)
            {
                Debug.Log(e.StackTrace);
            }
            
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

            //TODO: Change sprite inside explosion?

            //var glowSr = s8.transform.Find("Glow").GetComponent<SpriteRenderer>();

            //glowSr.sprite = chargeBlankSprite;
          
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
        [HarmonyPatch(typeof(Shotgun), "Start")]
        private static void AddExplosionFader(Shotgun __instance)
        {
            var exp = __instance.explosion;
            var s8 = exp.transform.Find("Sphere_8");
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
                var sr = obj.GetComponent<SpriteRenderer>();
                sr.color = interpColor;
                sr.sprite = muzzleFlashInnerBase;
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
            var sr = __instance.transform.Find("GameObject").GetComponentInChildren<SpriteRenderer>();
            sr.sprite = shotgunMuzzleFlashSprite;
            sr.color = Settings.shotgunGrenadeSpriteColor.value;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Punch), nameof(Punch.ParryProjectile))]
        private static void RecolorProjectileBoost(Projectile __instance, object[] __args)
        {
            try {
                Projectile proj = (Projectile)__args[0];
                if (proj.playerBullet)
                {
                    if (proj.TryGetComponent<MeshRenderer>(out var component2) && (bool)component2.material && component2.material.HasProperty("_Color"))
                    {
                        component2.material.SetColor("_Color", Settings.shotgunProjectileBoostStartColor.value);
                    }
                    if (proj.TryGetComponent<TrailRenderer>(out var component3))
                    {
                        component3.startColor = Settings.shotgunProjectileBoostStartColor.value;
                        component3.endColor = Settings.shotgunProjectileBoostEndColor.value;
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
        

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Start))]
        private static void RecolorShotgunSpark(Shotgun __instance)
        {
            var proj = __instance.bullet.GetComponent<Projectile>();
            var r = proj.explosionEffect.GetComponent<ParticleSystemRenderer>();
            var light = proj.GetComponent<Light>();
            light.color = Settings.shotgunBulletColor.value;
            Material newMat = new Material(r.material)
            {
                mainTexture = whiteSparkTexture,
                color = Settings.shotgunBulletColor.value
            };
            r.material = newMat;
        }
    }
}
