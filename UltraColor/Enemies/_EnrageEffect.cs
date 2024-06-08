using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace EffectChanger.Enemies
{
    public sealed class _EnrageEffect : MonoSingleton<_EnrageEffect>
    {
        private static Texture2D? blankEnrageSprite => Plugin.enrageEffectTextureBlank;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EnrageEffect), nameof(EnrageEffect.Start))]
        private static void RecolorEnrageEffectSprite(EnrageEffect __instance)
        {
            if (!Settings.enrageEnabled.value) return;
            
            var sprColor = Settings.enrageSpriteColor.value;
            var lightningColor = Settings.enrageLightningColor.value;

            var mr = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
            mr.material.mainTexture = blankEnrageSprite;
            mr.material.color = sprColor;

            var pl = __instance.gameObject.GetComponentInChildren<Light>();
            pl.color = sprColor;

            var ps = __instance.gameObject.GetComponentInChildren<ParticleSystem>();
            var psr = __instance.gameObject.GetComponentInChildren<ParticleSystemRenderer>();

            Gradient gradient = new Gradient();
            try
            {
                var myMat = Plugin.Fetch<Material>("Assets/Materials/Dev/Additive.mat");
                var unlitShader = Plugin.Fetch<Shader>("Assets/Shaders/Transparent/ULTRAKILL-simple-additive.shader");
                gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(lightningColor, 0.0f), new GradientColorKey(lightningColor, 0.5f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 0.7f) });

                var newMat = new Material(myMat)
                {
                    color = new Color(1, 1, 1, 1f),
                };

                ps.startColor = lightningColor;

                var trails = ps.trails;

                trails.colorOverLifetime = gradient;
                trails.colorOverTrail = gradient;
                trails.inheritParticleColor = false;
                newMat.shader = unlitShader;

            }
            catch
            {

            }
        }
    }
}
