using System.Collections;
using UnityEngine;

namespace EffectChanger
{
    public class MuzzleFlashInnerComponent : MonoBehaviour
    {
        private Sprite muzzleflashSprite;
        private float age = 0;

        // Use this for initialization
        void Start()
        {
            this.transform.localScale *= 4;
            
        }

        // Update is called once per frame
        void Update()
        {
            this.age += Time.deltaTime;
            if (this.age >= 0.25)
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }
}