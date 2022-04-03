using UnityEngine;

namespace Game
{
    public class ChangeCursor : MonoBehaviour
    {
        [SerializeField] string stateName;
        [SerializeField] bool invert;

        public void ChangeState(bool state) =>
            CursorManager.ChangeState(stateName, state != invert);
    }
}
