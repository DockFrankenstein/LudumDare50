using Game.Heat;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace Game.UI
{
    public class OverheatUIManager : MonoBehaviour
    {
        [SerializeField] AnimationCurve heatCurve;

        [SerializeField] Image[] images;
        [SerializeField] TMPro.TMP_Text[] texts;
        [SerializeField] Color lightNormalColor;
        [SerializeField] Color darkNormalColor;
        [SerializeField] Color lightErrorColor;
        [SerializeField] Color darkErrorColor;
        [SerializeField] Color textNormalColor;
        [SerializeField] Color textErrorColor;
        [SerializeField] float errorMinSpeed = 1f;
        [SerializeField] float errorMaxSpeed = 1.1f;
        [SerializeField] AnimationCurve errorCurve;
        [SerializeField] float errorBlinkDuration = 0.4f;
        [SerializeField] CanvasGroup infoBar;
        [SerializeField] float overHeatTrigger = 90f;
        [SerializeField] [Min(0.02f)] float infoBarAnimationDuration = 0.2f;

        float[] _partErrorTime;
        float[] _partErrorSpeed;

        Color[] _normalColors;

        bool _triggered;

        public float ErrorLevel => heatCurve.Evaluate(HeatManager.Heat / 100f);

        private void Awake()
        {
            _partErrorTime = new float[images.Length];
            for (int i = 0; i < _partErrorTime.Length; i++)
                _partErrorTime[i] = Random.Range(0f, errorBlinkDuration);

            _partErrorSpeed = new float[images.Length];
            for (int i = 0; i < _partErrorSpeed.Length; i++)
                _partErrorSpeed[i] = Random.Range(errorMinSpeed, errorMaxSpeed);

            _normalColors = new Color[images.Length];
            for (int i = 0; i < images.Length; i++)
                _normalColors[i] = Color.Lerp(lightNormalColor, darkNormalColor, Random.Range(0f, 1f));
        }

        private void Update()
        {
            Animate();

            switch (_triggered)
            {
                case true:
                    if (HeatManager.Heat >= overHeatTrigger) return;
                    _triggered = false;
                    StartCoroutine(AnimateInfoBar(0f));
                    break;
                case false:
                    if (HeatManager.Heat < overHeatTrigger) return;
                    _triggered = true;
                    StartCoroutine(AnimateInfoBar(1f));
                    break;
            }
        }

        IEnumerator AnimateInfoBar(float value)
        {
            float startValue = infoBar.alpha;
            float t = Time.time;

            while (Time.time - t < infoBarAnimationDuration)
            {
                infoBar.alpha = Mathf.Lerp(startValue, value, (Time.time - t) / infoBarAnimationDuration);
                yield return null;
            }

            infoBar.alpha = value;
        }

        void Animate()
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = GetColor(_normalColors[i], _partErrorTime[i]);
                _partErrorTime[i] += Time.deltaTime * _partErrorSpeed[i];
            }

            for (int i = 0; i < texts.Length; i++)
                texts[i].color = Color.Lerp(textNormalColor, textErrorColor, ErrorLevel);
        }

        Color GetColor(Color normalColor, float time)
        {
            Color dark = Color.Lerp(normalColor, darkErrorColor, ErrorLevel);
            Color light = Color.Lerp(normalColor, lightErrorColor, ErrorLevel);
            return Color.Lerp(dark, light, errorCurve.Evaluate(time / errorBlinkDuration));
        }
    }
}