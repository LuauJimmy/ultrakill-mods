using System.Collections;
using TMPro;
using Unity.Baselib.LowLevel;
using UnityEngine;
using UnityEngine.UI;

namespace UKHitMarker
{
    public class HudFader : MonoBehaviour
    {
        public float hitAge = 0;
        public float deathAge = 0;
        public float parryAge = 0;
        private float fadeSpeed = 5;
        private float duration = 2;
        private TextMeshProUGUI text;
        private Color killColor;
        private Image hitMarker;
        private Color hitColor;
        //private TMPro.TextMeshPro textMeshPro;
        // Update is called once per frame

        private void Start()
        {
            //text = gameObject.AddComponent<TMPro.TextMeshProUGUI>();

            //var vcrOsdMono = GameObject.Find("Canvas/Boss Healths/Boss Health 1/Panel/Filler/Slider/HP Text")
            //    .GetComponent<TextMeshProUGUI>().font;
            //text.font = vcrOsdMono;

            //text.alignment = TMPro.TextAlignmentOptions.Center;
            //text.text = "X";
            //text.color = Settings.hitMarkerColor.value;
            //text.outlineColor = Color.black;
            //text.fontSize = Settings.hitMarkerSize.value;
            //text.enabled = true;
            //text = this.gameObject.GetComponent<TextMeshProUGUI>();
            hitColor = Settings.hitMarkerColor.value;
            killColor = Settings.killMarkerColor.value;
            gameObject.AddComponent<CanvasRenderer>();
            hitMarker = gameObject.AddComponent<Image>();
            hitMarker.sprite = Plugin.hitMarkerSprite;
            hitMarker.color = hitColor;
            hitMarker.color -= new Color(0, 0, 0, 1);
            Debug.Log(hitMarker.transform.localScale);
            var scalefactor = Settings.hitMarkerSize.value * 0.01f;
            Debug.Log(scalefactor);
            hitMarker.transform.localScale *= scalefactor;
            Debug.Log(hitMarker.transform.localScale);

        }

        private void Update()
        {
            hitAge += Time.deltaTime;
            deathAge += Time.deltaTime;
            if (deathAge >= 0.5 && deathAge <= 1)
            {
                hitMarker.color -= new Color(0, 0, 0, 0.05f);
                return;
            }
            if (hitAge >= 0.5 && hitAge <= 1)
            {
                hitMarker.color -= new Color(0, 0, 0, 0.05f);
                return;
            }
        }

        public void ShowHitMarker()
        {
            hitAge = 0;
            if (deathAge <= 1) return;
            hitMarker.color = hitColor;
            //gameObject.SetActive(true);
        }
        public void ShowKillMarker()
        {
            deathAge = 0;
            hitMarker.color = killColor;
            //gameObject.SetActive(true);
        }

        public void ShowParryMarker()
        {
            deathAge = 0;
            hitMarker.color = Color.yellow;
        }
        //public static void ChangeTextSize(StyleHUD __instance)
        //{
        //    TMP_Text styleInfo = __instance.styleInfo;
        //}

    }
}