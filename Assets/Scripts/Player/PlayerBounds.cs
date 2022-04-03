using System;
using UnityEngine;

namespace Game
{
    public class PlayerBounds : MonoBehaviour
    {
        public int maxHeight;

        [Header("Debug")]
        [SerializeField] bool showDebugBorders;
        [SerializeField] Vector3 debugNorderSize = new Vector3(100f, 0.3f, 100f);

        [HideInInspector] public bool outOfBounds;

        public event Action OnExitBounds;
        public event Action OnEnterBounds;

        private void Awake()
        {
            OnExitBounds += () =>
            {
                Save.SaveManager.Revert();
            };
        }

        private void FixedUpdate()
        {
            bool lastOutOfBounds = outOfBounds;
            outOfBounds = transform.position.y < maxHeight;

            switch (outOfBounds, lastOutOfBounds)
            {
                case (true, false):
                    OnExitBounds?.Invoke();
                    break;
                case (false, true):
                    OnEnterBounds?.Invoke();
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            if (!showDebugBorders) return;
            Gizmos.color = Color.yellow;
            Vector3 pos = transform.position;
            pos.y = maxHeight;
            Gizmos.DrawCube(pos, debugNorderSize);
        }
    }
}