using UnityEngine;

namespace Game.Logic
{
    public class VelocityTransmitter : MonoBehaviour
    {
        public Vector3 velocity;

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.Lerp(Color.green, Color.red, velocity.magnitude / 40);
            Vector3 pointPos = transform.position + velocity.normalized * 2f;
            Gizmos.DrawSphere(pointPos, 0.3f);
            Gizmos.DrawLine(transform.position, pointPos);
        }
    }
}