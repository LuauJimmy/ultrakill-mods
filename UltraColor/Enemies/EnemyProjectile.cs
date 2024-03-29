using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace EffectChanger.Enemies
{
    public class EnemyProjectile : MonoBehaviour
    {
        private static bool shouldInit = true;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Projectile), nameof(Projectile.Start))]
        private static void RecolorEnemyProjectile(Projectile __instance)
        {
            if (!shouldInit) return;
            __instance.GetComponent<TrailRenderer>().startColor = new Color(0.3f, 0, 1, 1);
            __instance.GetComponent<TrailRenderer>().endColor = new Color(0.3f, 0, 1, 1);
            
        }


    }
}