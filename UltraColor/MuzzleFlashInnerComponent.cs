using System.Collections;
using Unity.Baselib.LowLevel;
using UnityEngine;

namespace EffectChanger
{
    public class MuzzleFlashInnerComponent : MonoBehaviour
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
            this.transform.localScale *= 6;
            this.transform.Rotate(new Vector3(0, 0, 1) * Random.Range(0, 360));
        }

        // Update is called once per frame
        private void Update()
        {
            this.age += Time.deltaTime;
            ////this.transform.localScale *= 0.75f;
            if (this.age >= 0.25)
            {
                Destroy(this.gameObject);
                return;
            }
            this.transform.localScale *= 0.95f;
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
            //color.a -= fadeSpeed * Time.deltaTime;
            //spr.color = color;
            //if (shrinkSpeed > 0f)
            //{
            //    base.transform.localScale = base.transform.localScale - Vector3.one * shrinkSpeed * Time.deltaTime;
            //}
        }
    }
}