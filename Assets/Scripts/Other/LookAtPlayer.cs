using Game.Player;
using UnityEngine;

namespace Game
{
    public class LookAtPlayer : MonoBehaviour
    {
        private void Update()
        {
            if (PlayerReference.Singleton?.move?.cameraTransform == null) return;

            PlayerMovement move = PlayerReference.Singleton.move;

            transform.eulerAngles = new Vector3(move.cameraTransform.eulerAngles.x, move.transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}