using UnityEngine.UI;
using UnityEngine;
using Game.Heat;

namespace Game.UI
{
    public class HeatDisplay : MonoBehaviour
    {
        [Header("Slider")]
        [SerializeField] Slider heatSlider;

        [Header("Text")]
        [SerializeField] TMPro.TMP_Text text;
        [SerializeField] string textFormatting = "{0}°C";
        [SerializeField] int minTemperatureValue = 20;
        [SerializeField] int maxTemperatureValue = 80;

        private void Update()
        {
            float baseSliderValue = (float)minTemperatureValue / maxTemperatureValue;
            heatSlider.value = baseSliderValue + HeatManager.Heat / 100f * (1f - baseSliderValue);

            text.text = string.Format(textFormatting,
                minTemperatureValue + Mathf.RoundToInt((maxTemperatureValue - minTemperatureValue) * HeatManager.Heat / 100f));
        }
    }
}