using System.Collections;
using Unity.Baselib.LowLevel;
using UnityEngine;

namespace EffectChanger
{
    public class ExplosionFader : MonoBehaviour
    {
        private float age = 0;
        private float scalefactor = 0f;
        private void Start()
        {

            //this.transform.Rotate(new Vector3(0, 0, 1) * Random.Range(0, 360));
        }

        private void Update()
        {
            
            var mat = this.GetComponent<MeshRenderer>().material;
            this.age += Time.deltaTime;

            //this.transform.localScale *= 0.75f;
            if (age > 0.5)
            {
                scalefactor += 0.01f;
                
                var redComponent = Mathf.Max(mat.color.r - scalefactor, 0.001f);
                var greenComponent = Mathf.Max(mat.color.g - scalefactor, 0.001f);
                var blueComponent = Mathf.Max(mat.color.b - scalefactor, 0.001f);
                var updatedColor = new Color(redComponent, greenComponent, blueComponent);
                this.GetComponent<MeshRenderer>().material.color = updatedColor;
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