using qASIC.AudioManagement;
using UnityEngine;

namespace Game.Audio
{
    public class PlayAudio : MonoBehaviour
    {
        [SerializeField] string audioChannel;
        [SerializeField] AudioData data;

        private void Awake()
        {
            AudioManager.Play(audioChannel, data);
        }
    } 
}