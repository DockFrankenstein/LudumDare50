using qASIC;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

namespace Game.Save
{
    public class CheckpointManager : MonoBehaviour
    {
        public List<Checkpoint> registeredCheckpoints;

        public static CheckpointManager Singleton { get; set; }

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                return;
            }

            if (Singleton != this)
                Destroy(gameObject);
        }

        public static void RegisterReachedCheckpoint(Checkpoint checkpoint)
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Checkpoint manager] Cannot register, no singleton!");
                return;
            }

            Singleton.registeredCheckpoints.Add(checkpoint);
        }

        public static void TeleportToPrevious(int amount)
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Checkpoint manager] Cannot revert, no singleton!");
                return;
            }

            if (PlayerReference.Singleton?.move == null)
            {
                qDebug.LogError("[Checkpoint manager] Cannot revert, no player!");
                return;
            }

            int index = Mathf.Max(0, Singleton.registeredCheckpoints.Count - 1 - amount);
            Singleton.registeredCheckpoints[index].TeleportPlayer();

            for (int i = index; i < Singleton.registeredCheckpoints.Count; i++)
                Singleton.registeredCheckpoints[i].UnRegister();

            Singleton.registeredCheckpoints.RemoveRange(index, Singleton.registeredCheckpoints.Count - index);

            qDebug.Log("[Checkpoint manager] Player reverted!", "checkpoint");
        }

        public static bool CheckpointsExist() =>
            Singleton != null && Singleton.registeredCheckpoints.Count != 0;

        public static Checkpoint GetLatestCheckpoint()
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Checkpoint manager] Cannot get latest checkpoint, no singleton!");
                return null;
            }

            List<Checkpoint> checkpoints = Singleton.registeredCheckpoints;
            return checkpoints.Count == 0 ? null : checkpoints[checkpoints.Count - 1];
        }
    }
}