using System.Collections.Generic;
using Game.Logic;

namespace Game.Save
{
    public class PlatformSaveNode : SaveNode
    {
        public Platform platformTarget;

        Dictionary<int, PlatformState> states = new Dictionary<int, PlatformState>();

        struct PlatformState
        {
            public bool reverse;
            public float time;
        }

        private void Reset()
        {
            platformTarget = GetComponent<Platform>();
        }

        public override void CreateVersion(int version)
        {
            states.Add(version, new PlatformState()
            { 
                reverse = platformTarget.Reverse,
                time = platformTarget.PlatformTime,
            });
        }

        public override void RevertVersion(int version)
        {
            PlatformState state = states[version];

            platformTarget.Reverse = state.reverse;
            platformTarget.PlatformTime = state.time;
            platformTarget.SetPosition();
        }

        public override void DeleteVersion(int version)
        {
            states.Remove(version);
        }
    }
}