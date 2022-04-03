using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
{
    public class UIPlayerVisualisation : MonoBehaviour
    {
        [SerializeField] Image[] parts;
        [SerializeField] Color lightNormalColor;
        [SerializeField] Color darkNormalColor;
        [SerializeField] Color lightErrorColor;
        [SerializeField] Color darkErrorColor;
        [SerializeField] float errorMinSpeed = 1f;
        [SerializeField] float errorMaxSpeed = 1.1f;
        [SerializeField] AnimationCurve errorCurve;
        [SerializeField] float errorBlinkDuration = 0.4f;

        float[] _partErrorTime;
        float[] _partErrorSpeed;

        Color[] _normalColors;

        public float ErrorLevel { get; set; }

        private void Awake()
        {
            _partErrorTime = new float[parts.Length];
            for (int i = 0; i < _partErrorTime.Length; i++)
                _partErrorTime[i] = Random.Range(0f, errorBlinkDuration);

            _partErrorSpeed = new float[parts.Length];
            for (int i = 0; i < _partErrorSpeed.Length; i++)
                _partErrorSpeed[i] = Random.Range(errorMinSpeed, errorMaxSpeed);

            _normalColors = new Color[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                _normalColors[i] = Color.Lerp(lightNormalColor, darkNormalColor, Random.Range(0f, 1f));
        }

        private void Update()
        {
            Animate();
        }

        void Animate()
        {
            for (int i = 0; i < parts.Length; i++)
            {
                Color dark = Color.Lerp(_normalColors[i], darkErrorColor, ErrorLevel);
                Color light = Color.Lerp(_normalColors[i], lightErrorColor, ErrorLevel);
                parts[i].color = Color.Lerp(dark, light, errorCurve.Evaluate(_partErrorTime[i] / errorBlinkDuration));
                _partErrorTime[i] += Time.deltaTime * _partErrorSpeed[i];
            }
        }
    }
}