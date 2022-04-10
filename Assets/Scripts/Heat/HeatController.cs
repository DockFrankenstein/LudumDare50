using Game.Player;
using UnityEngine;
using qASIC;

namespace Game.Heat
{
    public class HeatController : MonoBehaviour
    {
        [SerializeField] HeatDifficulty[] difficulties;

        [System.Serializable]
        public class HeatDifficulty
        {
            public float idleHeatSpeed;
            public float movementHeatSpeed;
            public float sprintHeatSpeed;
            public float jumpHeatSpeed;
            public float dashHeatSpeed;
            public float dashUpHeatSpeed;

            [Space]
            public int minTeleport = 1;
            public int maxTeleport = 2;
        }

        public static int Difficulty { get; set; } = 1;

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

        private void Awake()
        {
            HeatManager.ResetHeat();
        }

        private void Update()
        {
            HeatDifficulty difficulty = GetDifficulty();

            HeatManager.Heat += difficulty.idleHeatSpeed * Time.deltaTime;

            if (PlayerMovement.IsWalking)
                HandlePlayerWalk();

            if (HeatManager.Overheated)
            {
                HeatManager.ResetHeat();
                Save.CheckpointManager.TeleportToPrevious(Random.Range(difficulty.minTeleport, difficulty.maxTeleport));
            }

            qDebug.DisplayValue("Heat: ", HeatManager.Heat);
        }

        //void HandleRevert()
        //{
        //    _overheated = false;
        //    HeatManager.ResetHeat();
        //}

        void HandlePlayerWalk()
        {
            HeatDifficulty difficulty = GetDifficulty();
            HeatManager.Heat += (PlayerMovement.IsSprinting ? difficulty.sprintHeatSpeed : difficulty.movementHeatSpeed) * Time.deltaTime;
        }

        void HandlePlayerJump()
        {
            StartCoroutine(HeatManager.IncreaseGradually(GetDifficulty().jumpHeatSpeed));
        }

        void HandlePlayerDash()
        {
            StartCoroutine(HeatManager.IncreaseGradually(GetDifficulty().dashHeatSpeed));
        }

        void HandlePlayerDashUp()
        {
            StartCoroutine(HeatManager.IncreaseGradually(GetDifficulty().dashUpHeatSpeed));
        }

        HeatDifficulty GetDifficulty()
        {
            if (difficulties.Length == 0)
                return new HeatDifficulty();

            int index = Difficulty;

            if (Difficulty < 0 || Difficulty >= difficulties.Length)
                index = 0;

            return difficulties[index];
        }
    }
}