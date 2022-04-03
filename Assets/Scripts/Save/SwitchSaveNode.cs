using System.Collections.Generic;
using Game.Logic;

namespace Game.Save
{
    public class SwitchSaveNode : SaveNode
    {
        public Switch switchTarget;

        Dictionary<int, bool> states = new Dictionary<int, bool>();

        private void Reset()
        {
            switchTarget = GetComponent<Switch>();
        }

        public override void CreateVersion(int version)
        {
            states.Add(version, switchTarget.CurrentState);
        }

        public override void RevertVersion(int version)
        {
            switchTarget.ForceChangeState(states[version]);
        }

        public override void DeleteVersion(int version)
        {
            states.Remove(version);
        }
    }
}