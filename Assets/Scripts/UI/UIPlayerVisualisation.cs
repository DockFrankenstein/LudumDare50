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

        bool _displayError;
        float[] _partErrorTime;
        float[] _partErrorSpeed;

        private void Awake()
        {
            DisplayNormal();
        }

        public void DisplayError()
        {
            _displayError = true;
            _partErrorTime = new float[parts.Length];
            for (int i = 0; i < _partErrorTime.Length; i++)
                _partErrorTime[i] = Random.Range(0f, errorBlinkDuration);

            _partErrorSpeed = new float[parts.Length];
            for (int i = 0; i < _partErrorSpeed.Length; i++)
                _partErrorSpeed[i] = Random.Range(errorMinSpeed, errorMaxSpeed);
        }

        public void DisplayNormal()
        {
            _displayError = false;

            for (int i = 0; i < parts.Length; i++)
                parts[i].color = Color.Lerp(lightNormalColor, darkNormalColor, Random.Range(0f, 1f));
        }

        private void Update()
        {
            if (_displayError)
                AnimateError();
        }

        void AnimateError()
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].color = Color.Lerp(darkErrorColor, lightErrorColor, errorCurve.Evaluate(_partErrorTime[i] / errorBlinkDuration));
                _partErrorTime[i] += Time.deltaTime * _partErrorSpeed[i];
            }
        }
    }
}