using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraColor;
using UnityEngine;

namespace EffectChanger.Player
{
    public class _Movement : MonoBehaviour
    {
        static private GradientAlphaKey[] slowFade = new GradientAlphaKey[] { new GradientAlphaKey(0.2f, 0.0f), new GradientAlphaKey(0.0f, 0.7f) };
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))]
        private static void RecolorSlideScrapeParticle(NewMovement __instance)
        {
            if (!Settings.slideScrapeEnabled.value) return;
            Color color1 = Settings.slideScrapeSparksStartColor.value;
            Color color2 = Settings.slideScrapeSparksEndColor.value;
            Gradient gradient = new Gradient();
            try
            {
                var myMat = Plugin.Fetch<Material>("Assets/Materials/Dev/Additive.mat");
                var unlitShader = Plugin.Fetch<Shader>("Assets/Shaders/Transparent/ULTRAKILL-simple-additive.shader"); //Fetch<Shader>("Assets/Shaders/Particles/Particle_Additive.shader");
                var scrapeSlide = __instance.slideScrapePrefab;//Resources.FindObjectsOfTypeAll<GameObject>().Where(s => s.gameObject.name == "ScrapeSlide").First();
                var sparks = scrapeSlide.transform.Find("Sparks");
                var ps = sparks.GetComponent<ParticleSystem>();
                var psr = sparks.GetComponent<ParticleSystemRenderer>();

                var newMat = new Material(myMat)
                {
                    color = new Color(1, 1, 1, 0.3f),
                };

                psr.trailMaterial = newMat;

                var trails = ps.trails;
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 0.5f) },
                    slowFade
                );

                trails.colorOverLifetime = gradient;
                //trails.colorOverTrail = gradient;
                trails.inheritParticleColor = false;
                newMat.shader = unlitShader;
                //psr.trailMaterial = newMat;
            }
            catch
            {
                return;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))]
        private static void RecolorFallParticle(NewMovement __instance)
        {
            if (!Settings.slamEnabled.value) return;
            Color color1 = Settings.slamStartColor.value;
            Color color2 = Settings.slamEndColor.value;
            Gradient gradient = new Gradient();
            try
            {
                var myMat = Plugin.Fetch<Material>("Assets/Materials/Dev/Additive.mat");
                var unlitShader = Plugin.Fetch<Shader>("Assets/Shaders/Transparent/ULTRAKILL-simple-additive.shader"); //Fetch<Shader>("Assets/Shaders/Particles/Particle_Additive.shader");
                var fallParticle = __instance.fallParticle;//Resources.FindObjectsOfTypeAll<GameObject>().Where(s => s.gameObject.name == "ScrapeSlide").First();
                //var sparks = scrapeSlide.transform.Find("Sparks");
                var ps = fallParticle.GetComponentInChildren<ParticleSystem>();
                var psr = fallParticle.GetComponentInChildren<ParticleSystemRenderer>();

                var newMat = new Material(myMat)
                {
                    color = new Color(1, 1, 1, 1f),
                };

                psr.trailMaterial = newMat;

                var trails = ps.trails;
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 0.5f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 0.7f) });

                trails.colorOverLifetime = gradient;
                trails.colorOverTrail = gradient;
                trails.inheritParticleColor = false;
                newMat.shader = unlitShader;
                //psr.trailMaterial = newMat;
            }
            catch
            {
                return;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))]
        private static void RecolorSlideParticle(NewMovement __instance)
        {
            if (!Settings.slideScrapeEnabled.value) return;
            Color color1 = Settings.dodgeStartColor.value;
            Color color2 = Settings.dodgeEndColor.value;
            Gradient gradient = new Gradient();
            try
            {
                var myMat = Plugin.Fetch<Material>("Assets/Materials/Dev/Additive.mat");
                var unlitShader = Plugin.Fetch<Shader>("Assets/Shaders/Transparent/ULTRAKILL-simple-additive.shader"); //Fetch<Shader>("Assets/Shaders/Particles/Particle_Additive.shader");
                var slideParticle = __instance.slideParticle;//Resources.FindObjectsOfTypeAll<GameObject>().Where(s => s.gameObject.name == "ScrapeSlide").First();
                //var sparks = scrapeSlide.transform.Find("Sparks");
                var ps = slideParticle.GetComponentInChildren<ParticleSystem>();
                var psr = slideParticle.GetComponentInChildren<ParticleSystemRenderer>();

                var newMat = new Material(myMat)
                {
                    color = new Color(1, 1, 1, 1f),
                };

                psr.trailMaterial = newMat;

                var trails = ps.trails;
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 0.5f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 0.7f) });

                trails.colorOverLifetime = gradient;
                //trails.colorOverTrail = gradient;
                trails.inheritParticleColor = false;
                newMat.shader = unlitShader;
                //psr.trailMaterial = newMat;
            }
            catch
            {
                return;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))]
        private static void RecolorDodgeParticle(NewMovement __instance)
        {
            if (!Settings.dodgeEnabled.value) return;
            Color color1 = Settings.dodgeStartColor.value;
            Color color2 = Settings.dodgeEndColor.value;
            Gradient gradient = new Gradient();
            try
            {
                var myMat = Plugin.Fetch<Material>("Assets/Materials/Dev/Additive.mat");
                var unlitShader = Plugin.Fetch<Shader>("Assets/Shaders/Transparent/ULTRAKILL-simple-additive.shader"); //Fetch<Shader>("Assets/Shaders/Particles/Particle_Additive.shader");
                var dodgeParticle = __instance.dodgeParticle;//Resources.FindObjectsOfTypeAll<GameObject>().Where(s => s.gameObject.name == "ScrapeSlide").First();
                //var sparks = scrapeSlide.transform.Find("Sparks");
                var ps = dodgeParticle.GetComponent<ParticleSystem>();
                var psr = dodgeParticle.GetComponent<ParticleSystemRenderer>();

                var newMat = new Material(myMat)
                {
                    color = new Color(1, 1, 1, 1f),
                };

                psr.trailMaterial = newMat;

                var trails = ps.trails;
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 0.5f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 0.7f) });

                trails.colorOverLifetime = gradient;
                trails.colorOverTrail = gradient;
                trails.inheritParticleColor = false;
                newMat.shader = unlitShader;
                //psr.trailMaterial = newMat;
            }
            catch
            {
                return;
            }
        }
    }
}
