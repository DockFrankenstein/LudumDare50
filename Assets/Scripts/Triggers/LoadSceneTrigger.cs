using UnityEngine;

namespace Game
{
    public class LoadSceneTrigger : MonoBehaviour
    {
        [SerializeField] string playerTag = "Player";
        [SerializeField] string sceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
} 