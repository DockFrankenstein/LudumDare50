using UnityEngine;

namespace Game.Player
{
    public class PlayerReference : MonoBehaviour
    {
        public static PlayerReference Singleton { get; private set; }
        public PlayerMovement move;
        public PlayerCamera cam;

        private void Awake()
        {
            AssignScripts();
            AssignSingleton();
        }

        private void Reset()
        {
            AssignScripts();
        }

        void AssignScripts()
        {
            if (move == null)
                move = GetComponent<PlayerMovement>();

            if (cam == null)
                cam = GetComponent<PlayerCamera>();
        }

        void AssignSingleton()
        {
            if (Singleton == null)
            {
                Singleton = this;
                return;
            }

            if (Singleton != this)
                Destroy(gameObject);
        }
    }
}