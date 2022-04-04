using qASIC.AudioManagement;
using UnityEngine;

namespace Game.Audio
{
    public class StopAudio : MonoBehaviour
    {
        [SerializeField] string channelName;

        private void Awake()
        {
            AudioManager.Stop(channelName);
        }
    }
}