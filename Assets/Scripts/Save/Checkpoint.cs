using Game.Player;
using UnityEngine;

namespace Game.Save
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] string playerTag = "Player";
        [SerializeField] Vector3 teleportPosition;

        [Header("Debug")]
        [SerializeField] TMPro.TMP_Text debugText;

        int version = -1;

        public static bool DebugMode { get; set; }

        private void FixedUpdate()
        {
            debugText.gameObject.SetActive(DebugMode);
            debugText.text = $"reached: {version != -1}\n" +
                $"version: {version}\n" +
                $"currentLevel: {SaveManager.Singleton.CurrentVersion}\n" +
                $"isReachable: {IsReachable()}";
        }

        public bool IsReachable() =>
            version == -1 || SaveManager.Singleton.CurrentVersion < version;

        private void Awake()
        {
            SaveManager.OnRevert += () => HandleRevert();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsReachable() || !other.gameObject.CompareTag(playerTag)) return;

            if (PlayerMovement.IsGround)
            {
                Register();
                return;
            }

            PlayerMovement.OnLand += HandlePlayerLand;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(playerTag)) return;

            PlayerMovement.OnLand -= HandlePlayerLand;
        }

        void HandleRevert()
        {
            if (SaveManager.Singleton.CurrentVersion < version)
                version = -1;
        }

        void HandlePlayerLand()
        {
            Register();
            PlayerMovement.OnLand -= HandlePlayerLand;
        }

        void Register()
        {
            CheckpointManager.RegisterReachedCheckpoint(this);
            version = SaveManager.Save();
        }

        public void TeleportPlayer(bool save = true)
        {
            PlayerReference.Singleton?.move?.Teleport(GetPlayerRespawnPosition());
            if (save)
                Register();
        }

        public void UnRegister()
        {
            version = -1;
        }

        public Vector3 GetPlayerRespawnPosition() =>
            transform.position + teleportPosition;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + teleportPosition, 0.3f);
        }
    }
}