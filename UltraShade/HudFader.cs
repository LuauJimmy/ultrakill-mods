using System.Collections;
using TMPro;
using Unity.Baselib.LowLevel;
using UnityEngine;

namespace UltraShade
{
    public class HudFader : MonoBehaviour
    {
        private float age = 0;
        private float fadeSpeed = 5;
        private float duration = 2;
        private TextMeshProUGUI text;
        private Color color;
        //private TMPro.TextMeshPro textMeshPro;
        // Update is called once per frame

        private void Start()
        {
            text = this.gameObject.GetComponent<TextMeshProUGUI>();
            color = Settings.hitMarkerColor.value;
        }

        private void Update()
        {
            age += Time.deltaTime;
            if (age >= 0.5) 
            {
                text.color -= new Color(0, 0, 0, 0.05f);
                //this.gameObject.SetActive(false); 
            }
        }

        public void ResetFade()
        {
            age = 0;
            GetComponent<TextMeshProUGUI>().color = color;
            this.gameObject.SetActive(true);
        }
    }
}