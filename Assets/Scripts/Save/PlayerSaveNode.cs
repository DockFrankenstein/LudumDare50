using System.Collections.Generic;
using UnityEngine;

namespace Game.Save
{
    public class PlayerSaveNode : SaveNode
    {
        [SerializeField] Player.PlayerMovement targetPlayer;

        Dictionary<int, PlayerState> states = new Dictionary<int, PlayerState>();

        struct PlayerState
        {
            public Vector3 position;
            public Quaternion rotation;
        }

        private void Reset()
        {
            targetPlayer = GetComponent<Player.PlayerMovement>();
        }

        public override void CreateVersion(int version)
        {
            PlayerState state;
            switch (CheckpointManager.CheckpointsExist())
            {
                case true:
                    Checkpoint checkpoint = CheckpointManager.GetLatestCheckpoint();

                    state = new PlayerState()
                    {
                        position = checkpoint.GetPlayerRespawnPosition(),
                        rotation = checkpoint.transform.rotation,
                    };
                    break;
                default:
                    state = new PlayerState()
                    {
                        position = transform.position,
                        rotation = transform.rotation,
                    };
                    break;
            }

            states.Add(version, state);
        }

        public override void RevertVersion(int version)
        {
            PlayerState state = states[version];
            targetPlayer.Teleport(state.position, state.rotation);
            targetPlayer.ResetAdditionalVelocity();
        }

        public override void DeleteVersion(int version)
        {
            states.Remove(version);
        }
    }
}