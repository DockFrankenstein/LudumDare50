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

        private void OnTriggerEnter(Collider collision)
        {
            if (!IsReachable() || !collision.gameObject.CompareTag(playerTag)) return;

            if (PlayerMovement.IsGround)
            {
                Register();
                return;
            }

            PlayerMovement.OnLand += HandlePlayerLand;
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
            version = SaveManager.Save();
            CheckpointManager.RegisterReachedCheckpoint(this);
        }

        public void TeleportPlayer()
        {
            PlayerReference.Singleton?.move?.Teleport(transform.position + teleportPosition);
        }

        public void UnRegister()
        {
            version = -1;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + teleportPosition, 0.3f);
        }
    }
}