using Game.Player;
using UnityEngine;
using qASIC;

namespace Game.Heat
{
    public class HeatController : MonoBehaviour
    {
        [SerializeField] float idleHeatSpeed = 0.1f;
        [SerializeField] float movementHeatSpeed = 0.6f;
        [SerializeField] float sprintHeatSpeed = 1.1f;
        [SerializeField] float jumpHeatSpeed = 2f;

        private void OnEnable()
        {
            PlayerMovement.OnJump += HandlePlayerJump;
        }

        private void OnDisable()
        {
            PlayerMovement.OnJump -= HandlePlayerJump;
        }

        void HandlePlayerJump()
        {
            StartCoroutine(HeatManager.IncreaseGradually(jumpHeatSpeed));
        }

        private void Update()
        {
            HeatManager.Heat += idleHeatSpeed * Time.deltaTime;

            if (PlayerMovement.IsWalking)
                HandlePlayerWalk();

            if (HeatManager.Overheated)
                HeatManager.ResetHeat();

            qDebug.DisplayValue("Heat: ", HeatManager.Heat);
        }

        void HandlePlayerWalk()
        {
            HeatManager.Heat += (PlayerMovement.IsSprinting ? sprintHeatSpeed : movementHeatSpeed) * Time.deltaTime;
        }
    }
}