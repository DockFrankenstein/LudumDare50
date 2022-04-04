using UnityEngine;

namespace Game.Audio
{
    public class PlayerHeatAudio : MonoBehaviour
    {
        [SerializeField] AudioSource source;
        [SerializeField] AnimationCurve curve;
        [SerializeField] float minVolume = 0f;
        [SerializeField] float maxVolume = 0.6f;

        private void Reset()
        {
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            source.volume = curve.Evaluate(minVolume + Heat.HeatManager.Heat / 100 * (maxVolume - minVolume)) * maxVolume;
        }
    }
}