using qASIC;
using UnityEngine;

namespace Game.Interaction
{
    public class InteractionPointer : MonoBehaviour, IInteractable
    {
        [InspectorLabel("Target")] public GameObject targetGameObject;
        public IInteractable target;

        private void Awake()
        {
            target = targetGameObject.GetComponent<IInteractable>();

            if (target == null)
                qDebug.LogError("[Interaction Pointer] Target not specified!");
        }

        public void Interact()
        {
            if (target == null)
            {
                qDebug.LogError("[Interaction Pointer] Target not specified!");
                return;
            }

            target.Interact();
        }
    }
}