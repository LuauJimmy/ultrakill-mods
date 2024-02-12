using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectChanger
{
    public class RainbowComponent : MonoBehaviour
    {
        private float redComponent;
        private float greenComponent;
        private float blueComponent;
        private List<float> comps;
        // Use this for initialization
        void Start()
        {
            redComponent = Random.value;
            greenComponent = Random.value;
            blueComponent = Random.value;
            comps = [redComponent, greenComponent, blueComponent];
        }

        // Update is called once per frame
        void Update()
        {
            var mr = this.gameObject.GetComponentsInChildren<MeshRenderer>();
            mr[0].material.color = new Color(redComponent += 0.01f, greenComponent += 0.01f, blueComponent += 0.01f);
            if (redComponent > 1) redComponent = 0;
            if (greenComponent > 1) greenComponent = 0;
            if (blueComponent > 1) blueComponent = 0;
            //for (int i = 1; i < comps.Count; i++)
            //{
            //    if (comps[i - 1] > 0) comps[i - 1] = 0;
            //}
        }
    }
}