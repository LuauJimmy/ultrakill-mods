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
            try
            {
                if (__instance.friendly) return;
                if (__instance.name.Contains("Homing") && Settings.homingProjEnabled.value) {
                    __instance.GetComponent<TrailRenderer>().startColor = Settings.homingProjTrailStartColor.value;
                    __instance.GetComponent<TrailRenderer>().endColor = Settings.homingProjTrailStartColor.value;
                    __instance.GetComponent<MeshRenderer>().material.mainTexture = Plugin.skullBlankTexture;
                    __instance.GetComponent<MeshRenderer>().material.color = Settings.homingProjInnerGlowColor.value;
                    var outerGlowMR = __instance.GetComponentsInChildren<MeshRenderer>()[1];
                    // Review: Using base texture (instead of hue-zeroed blank texture) seems to look better
                    //outerGlowMR.material.mainTexture = Plugin.chargeBlankTexture;
                    outerGlowMR.material.color = Settings.homingProjOuterGlowColor.value;
                }
                else
                {
                    if (!Settings.enemyProjEnabled.value) return;
                    __instance.GetComponent<TrailRenderer>().startColor = Settings.enemyProjTrailStartColor.value;
                    __instance.GetComponent<TrailRenderer>().endColor = Settings.enemyProjTrailStartColor.value;
                    __instance.GetComponent<MeshRenderer>().material.mainTexture = Plugin.skullBlankTexture;
                    __instance.GetComponent<MeshRenderer>().material.color = Settings.enemyProjInnerGlowColor.value;
                    var outerGlowMR = __instance.GetComponentsInChildren<MeshRenderer>()[1];
                    // Review: Using base texture (instead of hue-zeroed blank texture) seems to look better
                    //outerGlowMR.material.mainTexture = Plugin.chargeBlankTexture;
                    outerGlowMR.material.color = Settings.enemyProjOuterGlowColor.value;
                }
            }
            // poo
            catch { return; }
        }


    }
}