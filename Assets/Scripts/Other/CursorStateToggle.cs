using UnityEngine;
using qASIC;

namespace Game
{
    public class CursorStateToggle : MonoBehaviour
    {
        [SerializeField] [KeyCodeListener] KeyCode key;
        [SerializeField] string stateName;

        private void Update()
        {
            if (Input.GetKeyDown(key))
                CursorManager.ChangeState(stateName, !CursorManager.GetState(stateName));
        }
    }
}