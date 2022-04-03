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
            states.Add(version, new PlayerState()
            {
                position = transform.position,
                rotation = transform.rotation,
            });
        }

        public override void RevertVersion(int version)
        {
            PlayerState state = states[version];
            targetPlayer.Teleport(state.position, state.rotation);
            targetPlayer.ResetAdditionalVelocity();
        }
    }
}