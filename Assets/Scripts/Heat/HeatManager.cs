using UnityEngine;
using System.Collections;

namespace Game.Heat
{
    public static class HeatManager
    {
        const float minHeat = 0f;
        const float maxHeat = 100f;
        const float gradualIncreaseTime = 1f;

        public static bool StopHeat { get; set; }

        private static float _heat = 0f;
        public static float Heat 
        { 
            get => _heat;
            set
            {
                if (StopHeat) return;
                _heat = Mathf.Clamp(value, minHeat, maxHeat);
            }
        }
        public static bool Overheated => Heat >= maxHeat;

        public static void ResetHeat() =>
            _heat = minHeat;

        public static IEnumerator IncreaseGradually(float amount)
        {
            float time = 0f;
            while (time < gradualIncreaseTime)
            {
                time += Time.deltaTime;
                Heat += amount * Time.deltaTime;
                yield return null;
            }
            Heat += amount * (time - 1f);
        }
    }
}