using Game.Heat;
using UnityEngine;
using UnityEngine.UI;

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

        float[] _partErrorTime;
        float[] _partErrorSpeed;

        Color[] _normalColors;

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