using System.Collections;
using HarmonyLib;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Enemies
{
    public class EnemyProjectile : MonoBehaviour
    {
        private static bool shouldInit = true;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Projectile), nameof(Projectile.Start))]
        private static void RecolorEnemyProjectile(Projectile __instance)
        {
            if (__instance.name.Contains("Homing") || __instance.friendly ) return;
            __instance.GetComponent<TrailRenderer>().startColor = Settings.enemyProjTrailStartColor.value;
            __instance.GetComponent<TrailRenderer>().endColor = Settings.enemyProjTrailStartColor.value;
            __instance.GetComponent<MeshRenderer>().material.mainTexture = Plugin.chargeBlankTexture;
            __instance.GetComponent<MeshRenderer>().material.color = Settings.enemyProjInnerGlowColor.value;
            __instance.GetComponentsInChildren<MeshRenderer>()[1].material.color = Settings.enemyProjOuterGlowColor.value;
        }


    }
}