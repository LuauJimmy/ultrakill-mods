using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Weapons
{
    public sealed class _Idol : MonoSingleton<_Idol>
    {
        private static bool _shouldInitBlessing = true;
        private static bool _shouldInitIdol = true;
        private static Texture? _whiteSparkTexture = Plugin.whiteSparkTexture;
        

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Idol), nameof(Idol.Start))]
        private static void RecolorBlessedGlow(Idol __instance)
        {
            if (!_shouldInitBlessing || !Settings.blessingEnabled.value) return;

            var glow = Resources.FindObjectsOfTypeAll<Transform>().Where(s => s.name == "BlessingGlow").First();
            if (glow != null) 
            { 
                glow.GetComponentInChildren<SpriteRenderer>().color = Settings.blessingColor.value;
                var particleEffect = glow.GetComponentInChildren<ParticleSystem>();
                particleEffect.startColor = Settings.blessingBeamColor.value;
            }

            _shouldInitBlessing = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Idol), nameof(Idol.Start))]
        private static void RecolorIdolGlow(Idol __instance)
        {
            if (!_shouldInitIdol || !Settings.blessingEnabled.value) return;
            var glow = Resources.FindObjectsOfTypeAll<Transform>()
                .Where(s => s.name == "Idol" && s.parent == null)
                .First();
            
            var halo = glow.gameObject.transform.Find("Halo (1)/New Sprite");
            halo.gameObject.GetComponent<SpriteRenderer>().color = Settings.idolHaloColor.value;
            var thisHalo = __instance.gameObject.transform.Find("Halo (1)/New Sprite");
            thisHalo.gameObject.GetComponent<SpriteRenderer>().color = Settings.idolHaloColor.value;

            var beam = glow.gameObject.transform.Find("Beam").GetComponent<LineRenderer>();
            beam.startColor = Settings.blessingColor.value;
            beam.endColor = Settings.blessingColor.value;
            var thisBeam = __instance.gameObject.transform.Find("Beam").GetComponent<LineRenderer>();
            thisBeam.startColor = Settings.blessingColor.value;
            thisBeam.endColor = Settings.blessingColor.value;

            var spikeParticlesGO = glow.gameObject.transform.Find("Halo (1)/ParticleEffects/Particle System");
            var spikeParticles = spikeParticlesGO.GetComponent<ParticleSystem>();
            //var col = spikeParticles.colorOverLifetime;
            var unlitShader = Plugin.Fetch<Shader>("Assets/Shaders/Particles/Particle_Additive.shader");

            var psr = spikeParticlesGO.GetComponent<ParticleSystemRenderer>();

            Material newMat = new Material(psr.trailMaterial)
            {
                color = Settings.idolSpikesColor.value,
                //mainTexture = _whiteSparkTexture
            };

            newMat.shader = unlitShader;
            psr.trailMaterial = newMat;

            __instance.gameObject.transform.Find("Halo (1)/ParticleEffects/Particle System").GetComponent<ParticleSystemRenderer>().trailMaterial = newMat;
            //psr.trailMaterial.color = Settings.blessingColor.value;
            //spikeParticles.startColor = Settings.blessingColor.value;

            var pointLight = glow.gameObject.transform.Find("Halo (1)/ParticleEffects/Point Light").GetComponent<Light>();

            pointLight.color = Settings.blessingColor.value;

            var thisPointLight = __instance.gameObject.transform.Find("Halo (1)/ParticleEffects/Point Light").GetComponent<Light>();
            
            thisPointLight.color = Settings.blessingColor.value;

            _shouldInitIdol = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.RestartScene))]
        [HarmonyPatch(typeof(SceneHelper), nameof(SceneHelper.LoadScene))]
        private static void ResetGlowInitState()
        {
            _shouldInitBlessing = true;
            _shouldInitIdol = true;
        }
     
    }
}
