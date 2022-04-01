using UnityEngine;

namespace Game
{
    public class AddToDontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}