using UnityEngine;

namespace Game
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset;
        [SerializeField] bool followX = true;
        [SerializeField] bool followY = true;
        [SerializeField] bool followZ = true;

        private void Update()
        {
            Vector3 pos = transform.position;

            if (followX)
                pos.x = target.position.x + offset.x;

            if (followY)
                pos.y = target.position.y + offset.y;

            if (followZ)
                pos.z = target.position.z + offset.z;

            transform.position = pos;
        }
    }
}