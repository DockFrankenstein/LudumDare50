using UnityEngine;

namespace Game.Tools
{
    public class TestPoint : MonoBehaviour
    {
        public static TestPoint Singleton { get; private set; }

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                return;
            }

            if (Singleton != this)
                Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}