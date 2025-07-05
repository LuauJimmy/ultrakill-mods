using BepInEx;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace EffectChanger.Weapons
{
    public sealed class _Nailgun : MonoSingleton<_Nailgun>
    {
        private static Sprite? blankMuzzleFlashSprite => Plugin.blankMuzzleFlashSprite;
        private static Sprite? muzzleFlashInnerBase => Plugin.muzzleFlashInnerBase;
        private static Gradient? ElectricLineGradient = Plugin.ElectricLineGradient;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Nailgun), "Start")]
        private static void RecolorNailgunProjecileTrail(Nailgun __instance)
        {
            switch (__instance.gameObject.name)
            {
                case "Nailgun Magnet(Clone)":
                    if (!Settings.magnetNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.magnetNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.magnetNailgunTrailEndColor.value;

                    break;

                case "Nailgun Overheat(Clone)":
                    if (!Settings.overheatNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.overheatNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.overheatNailgunTrailEndColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().startColor = Settings.overheatNailgunHeatedNailTrailStartColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().endColor = Settings.overheatNailgunHeatedNailTrailEndColor.value;

                    break;

                case "Sawblade Launcher Magnet(Clone)":
                    if (!Settings.altMagnetNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.altMagnetNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.altMagnetNailgunTrailEndColor.value;
                    break;

                case "Sawblade Launcher Overheat(Clone)":
                    if (!Settings.altOverheatNailgunEnabled.value) return;
                    __instance.nail.GetComponent<TrailRenderer>().startColor = Settings.altOverheatNailgunTrailStartColor.value;
                    __instance.nail.GetComponent<TrailRenderer>().endColor = Settings.altOverheatNailgunTrailEndColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().startColor = Settings.altOverheatNailgunHeatedNailTrailStartColor.value;
                    __instance.heatedNail.GetComponent<TrailRenderer>().endColor = Settings.altOverheatNailgunHeatedNailTrailEndColor.value;
                    break;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Nailgun), "OnEnable")]
        private static void RecolorNailgunMuzzleFlash(Nailgun __instance)
        {
            Color color;
            switch (__instance.gameObject.name)
            {
                case "Nailgun Magnet(Clone)":
                    if (!Settings.magnetNailgunEnabled.value) return;
                    color = Settings.magnetNailgunMuzzleFlashColor.value;
                    break;

                case "Nailgun Overheat(Clone)":
                    if (!Settings.overheatNailgunEnabled.value) return;
                    color = Settings.overheatNailgunMuzzleFlashColor.value;
                    break;

                case "Sawblade Launcher Magnet(Clone)":
                    if (!Settings.altMagnetNailgunEnabled.value) return;
                    color = Settings.altMagnetNailgunMuzzleFlashColor.value;
                    break;

                case "Sawblade Launcher Overheat(Clone)":
                    if (!Settings.altOverheatNailgunEnabled.value) return;
                    color = Settings.altOverheatNailgunMuzzleFlashColor.value;
                    break;

                default: return;
            }
            var light = __instance.muzzleFlash.GetComponentInChildren<Light>();
            light.color = color;
            var muzzleFlashes = __instance.muzzleFlash.GetComponentsInChildren<SpriteRenderer>();
            foreach (var muzzle in muzzleFlashes)
            {
                muzzle.sprite = blankMuzzleFlashSprite;
                muzzle.color = color;
            };
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.RestartScene))]
        [HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.LoadScene))]
        private static void LoadConductorGradient(SceneHelper __instance)
        {
            if (!Settings.conductorEnabled.value) return;
            var gradient = new Gradient();

            var colors = new GradientColorKey[2];
            colors[0] = new GradientColorKey(Settings.conductorStartColor.value, 0.0f);
            colors[1] = new GradientColorKey(Settings.conductorEndColor.value, 1.0f);

            var alphas = new GradientAlphaKey[1];
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            //alphas[1] = new GradientAlphaKey(0.0f, 1.0f);

            gradient.SetKeys(colors, alphas);
            ElectricLineGradient = gradient;
            return;

        }
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Zapper), nameof(Zapper.Zap))]
        //private static bool Zap(Zapper __instance)
        //{
        //    if ((bool)__instance.attachedEnemy)
        //    {
        //        __instance.attachedEnemy.hitter = "zapper";
        //        __instance.attachedEnemy.hitterAttributes.Add(HitterAttribute.Electricity);
        //        __instance.attachedEnemy.DeliverDamage(__instance.hitLimb.gameObject, Vector3.up * 100000f, __instance.broken ? __instance.hitLimb.transform.position : __instance.transform.position, __instance.damage, tryForExplode: true, 0f, __instance.sourceWeapon);
        //        MonoSingleton<WeaponCharges>.Instance.naiZapperRecharge = 0f;
        //        EnemyIdentifierIdentifier[] componentsInChildren = __instance.attachedEnemy.GetComponentsInChildren<EnemyIdentifierIdentifier>();
        //        foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in componentsInChildren)
        //        {
        //            if (enemyIdentifierIdentifier != __instance.hitLimb && enemyIdentifierIdentifier.gameObject != __instance.attachedEnemy.gameObject)
        //            {
        //                __instance.attachedEnemy.DeliverDamage(enemyIdentifierIdentifier.gameObject, Vector3.zero, enemyIdentifierIdentifier.transform.position, Mathf.Epsilon, tryForExplode: false);
        //            }
        //            Instantiate(__instance.zapParticle, enemyIdentifierIdentifier.transform.position, Quaternion.identity).transform.localScale *= 0.5f;
        //            Debug.Log($"zappa\n{__instance.zapParticle.gameObject.name}");
        //        }
        //    }
        //    __instance.Break(successful: true);
        //    return false;
        //}


        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(ElectricityLine), nameof(ElectricityLine.))]
        //private static void RecolorNailgunElectricityLine(ElectricityLine __instance)
        //{
        //    Gradient g = new Gradient();
        //    g.colorKeys.AddItem(new GradientColorKey(Color.green, 0.5f));
        //    g.colorKeys.AddItem(new GradientColorKey(Color.cyan, 0.5f));
        //    __instance.colors = g;
        //}


        [HarmonyPrefix]
        [HarmonyPatch(typeof(ElectricityLine), nameof(ElectricityLine.Update))]
        private static bool UpdateElectricityLine(ElectricityLine __instance)
        {
            if (!Settings.conductorEnabled.value) return true;
            
            __instance.fadeLerp = Mathf.MoveTowards(__instance.fadeLerp, 0f, Time.deltaTime * __instance.fadeSpeed);
            if (__instance.fadeLerp <= 0f)
            {
                __instance.gameObject.SetActive(value: false);
            }
            if (__instance.cooldown > 0f)
            {
                __instance.cooldown = Mathf.MoveTowards(__instance.cooldown, 0f, Time.deltaTime);
                return false;
            }
            __instance.cooldown = 0.05f;
            if (!__instance.lr)
            {
                __instance.lr = __instance.GetComponent<LineRenderer>();
            }
            //__instance.lr.material = __instance.lightningMats[UnityEngine.Random.Range(0, __instance.lightningMats.Length)];
            __instance.lr.widthMultiplier = UnityEngine.Random.Range(__instance.minWidth, __instance.maxWidth);
            __instance.lr.startColor = ElectricLineGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
            __instance.lr.endColor = ElectricLineGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
            Debug.Log($"g.Eval\n{ElectricLineGradient.Evaluate(UnityEngine.Random.Range(0f, 1f))}");
            //__instance.lr.startColor = new Color(__instance.lr.startColor.r, __instance.lr.startColor.g, __instance.lr.startColor.b, __instance.lr.startColor.a * __instance.fadeLerp);
            //__instance.lr.endColor = new Color(__instance.lr.endColor.r, __instance.lr.endColor.g, __instance.lr.endColor.b, __instance.lr.endColor.a * __instance.fadeLerp);
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Nailgun), "Shoot")]
        private static void AddMuzzleFlashInnerComponent_Nailgun(Nailgun __instance)
        {
            var muzzleFlashes = __instance.muzzleFlash.GetComponentsInChildren<SpriteRenderer>();

            foreach (var flash in muzzleFlashes)
            {
                var colorA = flash.GetComponent<SpriteRenderer>().color;
                var obj = Instantiate(flash);
                var interpColor = Color.Lerp(colorA, Color.white, 0.8f);
                interpColor.a = 0.8f;
                obj.GetComponent<SpriteRenderer>().color = interpColor;
                obj.GetComponent<SpriteRenderer>().sprite = muzzleFlashInnerBase;
                obj.transform.position = __instance.shootPoints[0].transform.position;
                obj.transform.rotation = __instance.shootPoints[0].transform.rotation;
                obj.gameObject.AddComponent<MuzzleFlashInnerComponent>();
            };
        }
    }
}
