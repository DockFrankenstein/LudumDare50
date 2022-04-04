using UnityEngine;

namespace Game
{
    public class IgnorePlayerMoveMouse : MonoBehaviour
    {
        public void Ignore()
        {
            Player.PlayerMovement.IgnoreMouseUntillUp = true;
        }
    }
}