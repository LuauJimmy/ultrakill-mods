using System.Collections;
using Unity.Baselib.LowLevel;
using UnityEngine;

namespace EffectChanger
{
    public class RendererFader : MonoBehaviour
    {
        private Sprite muzzleflashSprite;
        private SpriteRenderer sr;
        private float age = 0;
        private float fadeSpeed = 5;
        private float shrinkSpeed = 1;
        private float originalAlpha = 1;
        // Use this for initialization
        private void Start()
        {

            //this.transform.Rotate(new Vector3(0, 0, 1) * Random.Range(0, 360));
        }

        // Update is called once per frame
        private void Update()
        {
            var mat = this.GetComponent<MeshRenderer>().material;
            this.age += Time.deltaTime;
            var scalefactor = 0f;
            //this.transform.localScale *= 0.75f;
            if (age > 0.3)
            {
                scalefactor += age * 0.5f;
                this.GetComponent<MeshRenderer>().material.color = new Color(mat.color.r - scalefactor, mat.color.g - scalefactor, mat.color.b - scalefactor);
            }
            //this.transform.localScale *= 0.95f * Time.deltaTime;
            //var spr = this.GetComponent<SpriteRenderer>();
            //if (spr.color.a >= originalAlpha && fadeSpeed < 0f)
            //{
            //    fadeSpeed = Mathf.Abs(fadeSpeed);
            //}
            //else if (spr.color.a <= 0f && fadeSpeed > 0f)
            //{
            //    fadeSpeed = Mathf.Abs(fadeSpeed) * -1f;
            //}
            //Color color = spr.color;
            //color.r = Random.Range(0, 100) * 0.1f;
            //color.g = Random.Range(0, 100) * 0.1f;
            //color.b = Random.Range(0, 100) * 0.1f;
            //color.a -= fadeSpeed * Time.deltaTime;
            //spr.color = color;
            //if (shrinkSpeed > 0f)
            //{
            //    base.transform.localScale = base.transform.localScale - Vector3.one * shrinkSpeed * Time.deltaTime;
            //}
        }
    }
}