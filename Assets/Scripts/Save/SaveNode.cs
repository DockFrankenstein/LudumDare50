using UnityEngine;

namespace Game.Save
{
    public abstract class SaveNode : MonoBehaviour
    {
        private void Awake()
        {
            SaveManager.AddNode(this);
        }

        public abstract void CreateVersion(int version);
        public abstract void RevertVersion(int version);
        public abstract void DeleteVersion(int version);
    }
}