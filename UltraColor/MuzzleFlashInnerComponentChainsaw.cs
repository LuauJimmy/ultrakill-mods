using System.Collections;
using Unity.Baselib.LowLevel;
using UnityEngine;

namespace EffectChanger
{
    public class MuzzleFlashInnerComponentChainsaw : MonoBehaviour
    {
        private Sprite muzzleflashSprite;
        private SpriteRenderer sr;
        private float age = 0;
        private float fadeSpeed = 5;
        private float shrinkSpeed = 1;
        private float originalAlpha = 1;
        private Vector3 offset = new Vector3(0.99f, 0.99f, 0.99f);

        public bool isTouchingPlayer;
        // Use this for initialization
        private void Start()
        {
            isTouchingPlayer = false;
            // this.transform.localScale = new Vector3(0.50f, 0.50f, 0.50f);
        }

        // Update is called once per frame
        private void Update()
        {
            // var v = this.transform.parent.position;
            // this.transform.localPosition = offsetv;
            this.transform.position = this.transform.parent.position;// * 0.99999f;
            
            Vector3 offsetv = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -0.00005f);
            this.transform.localPosition = offsetv;
            this.transform.rotation = this.transform.parent.rotation;
            // this.transform.localScale = this.transform.parent.lossyScale;
        }
        
        
    }
}