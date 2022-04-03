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
        [SerializeField] float dashHeatSpeed = 5f;
        [SerializeField] float dashUpHeatSpeed = 7f;

        private void OnEnable()
        {
            PlayerMovement.OnJump += HandlePlayerJump;
            PlayerMovement.OnDash += HandlePlayerDash;
            PlayerMovement.OnDashUp += HandlePlayerDashUp;
        }

        private void OnDisable()
        {
            PlayerMovement.OnJump -= HandlePlayerJump;
            PlayerMovement.OnDash -= HandlePlayerDash;
            PlayerMovement.OnDashUp -= HandlePlayerDashUp;
        }

        bool _overheated;

        private void Update()
        {
            HeatManager.Heat += idleHeatSpeed * Time.deltaTime;

            if (PlayerMovement.IsWalking)
                HandlePlayerWalk();

            if (!_overheated && HeatManager.Overheated)
            {
                _overheated = true;
                Save.SaveManager.OnRevert += HandleRevert;
                Save.SaveManager.Revert(Mathf.Max(Save.SaveManager.Singleton.CurrentVersion - 1, 0));
            }

            qDebug.DisplayValue("Heat: ", HeatManager.Heat);
        }

        void HandleRevert()
        {
            _overheated = false;
            HeatManager.ResetHeat();
            Save.SaveManager.OnRevert -= HandleRevert;
        }

        void HandlePlayerWalk()
        {
            HeatManager.Heat += (PlayerMovement.IsSprinting ? sprintHeatSpeed : movementHeatSpeed) * Time.deltaTime;
        }

        void HandlePlayerJump()
        {
            StartCoroutine(HeatManager.IncreaseGradually(jumpHeatSpeed));
        }

        void HandlePlayerDash()
        {
            StartCoroutine(HeatManager.IncreaseGradually(dashHeatSpeed));
        }

        void HandlePlayerDashUp()
        {
            StartCoroutine(HeatManager.IncreaseGradually(dashUpHeatSpeed));
        }
    }
}