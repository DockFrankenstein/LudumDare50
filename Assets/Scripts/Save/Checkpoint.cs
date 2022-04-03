using Game.Player;
using UnityEngine;

namespace Game.Save
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] string playerTag = "Player";

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
                version = SaveManager.Save();
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
            version = SaveManager.Save();
            PlayerMovement.OnLand -= HandlePlayerLand;
        }
    }
}