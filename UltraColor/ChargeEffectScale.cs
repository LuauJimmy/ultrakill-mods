using System.Collections;
using Unity.Baselib.LowLevel;
using UnityEngine;

namespace EffectChanger
{
    public class ChargeEffectScale : MonoBehaviour
    {
        private void Update()
        {
            this.transform.localScale = Vector3.one * this.GetComponentInParent<Revolver>().pierceShotCharge * 0.032f;
        }
    }
}