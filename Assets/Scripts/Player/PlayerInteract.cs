using qASIC;
using qASIC.InputManagement;
using UnityEngine;
using Game.Interaction;

namespace Game.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] float range = 4f;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] LayerMask targetMask;
        [SerializeField] InputActionReference interactionKey;
        [SerializeField] Transform castPoint;
        [SerializeField] GameObject interactText;

        IInteractable target;

        private void Reset()
        {
            castPoint = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            Cast();
            interactText.SetActive(target != null);
            qDebug.DisplayValue("Interactable: ", target != null);
        }

        void Cast()
        {
            bool hits = Physics.Raycast(castPoint.position, castPoint.TransformDirection(Vector3.forward), out RaycastHit hit, range, detectionMask);
            Debug.DrawRay(castPoint.position, castPoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            if (!hits)
            {
                target = null;
                return;
            }

            if (((1 << hit.transform.gameObject.layer) & targetMask) == 0)
            {
                target = null;
                return;
            }

            target = hit.transform.GetComponent<IInteractable>();
        }

        private void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            if (!CursorManager.CanLook) return;

            bool isInteracting = InputManager.GetInputDown(interactionKey);

            if (isInteracting && target != null)
            {
                target.Interact();
            }
        }
    }
}