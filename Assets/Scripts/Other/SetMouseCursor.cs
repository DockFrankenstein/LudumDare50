using UnityEngine;

namespace Game.UI
{
    public class SetMouseCursor : MonoBehaviour
    {
        [SerializeField] string stateName;
        [SerializeField] bool state;

        private void Awake()
        {
            CursorManager.ChangeState(stateName, state);
        }
    } 
}