using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UltraShade;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltraShade.Weapons
{
    public sealed class _Shotgun : MonoSingleton<_Shotgun>
    {
        public void Awake()
        {

        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(Shotgun), "Start")]
        private static void RecolorToUniverse(Shotgun __instance)
        {
            __instance.gameObject.transform.Find("Shotgun_New/Shotgun_New").GetComponent<SkinnedMeshRenderer>().material = Plugin.universeMat;
            var punches = Resources.FindObjectsOfTypeAll(typeof(Punch));
            foreach (Punch punch in punches)
            {
                punch.GetComponentInChildren<SkinnedMeshRenderer>().material = Plugin.universeMat;
            }
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Shotgun), "Start")]
        //private static void AddDoubleRenderToShotgun(Shotgun __instance)
        //{
        //    var dr = __instance.gameObject.AddComponent<DoubleRender>();

        //    dr.enabled = true;
        //    dr.cc = MonoSingleton<CameraController>.Instance;
        //    dr.currentCam = dr.cc.cam;
        //    dr.radiantMat = new Material(MonoSingleton<PostProcessV2_Handler>.Instance.radiantBuff);
        //    dr.thisRend = __instance.gameObject.transform.Find("Shotgun_New/Shotgun_New").GetComponent<SkinnedMeshRenderer>();
        //    var cb = new CommandBuffer
        //    {
        //        name = "BuffRender"
        //    };
        //    Mesh mesh = null;
        //    if (dr.thisRend is SkinnedMeshRenderer)
        //    {
        //        mesh = (dr.thisRend as SkinnedMeshRenderer).sharedMesh;
        //    }
        //    else if (dr.thisRend is MeshRenderer)
        //    {
        //        mesh = dr.thisRend.GetComponent<MeshFilter>().sharedMesh;
        //    }
        //    if (mesh != null)
        //    {
        //        for (int i = 0; i < mesh.subMeshCount; i++)
        //        {
        //            if (!dr.subMeshesToIgnore.Contains(i))
        //            {
        //                cb.DrawRenderer(dr.thisRend, dr.radiantMat, i);
        //            }
        //        }
        //    }
        //    dr.radiantMat.SetFloat("_ForceOutline", 1f);
        //    dr.currentCam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, cb);
        //    dr.isActive = true;
        //    dr.cb = cb;
        //}
    }
}
