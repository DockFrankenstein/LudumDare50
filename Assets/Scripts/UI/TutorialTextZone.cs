using qASIC.InputManagement;
using UnityEngine;

namespace Game
{
    public class TutorialTextZone : MonoBehaviour
    {
        [SerializeField] bool triggerOneTime;
        [SerializeField] [TextArea(3, 5)] string format;
        [SerializeField] InputActionReference[] references;

        bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered) return;

            string[] keys = new string[references.Length];
            for (int i = 0; i < references.Length; i++)
                keys[i] = InputManager.GetKeyCode(references[i].GroupName, references[i].ActionName, 0).ToString();

            TutorialTextController.TutorialText = string.Format(format, keys);
        }

        private void OnTriggerExit(Collider other)
        {
            TutorialTextController.TutorialText = string.Empty;

            if (triggerOneTime)
                _triggered = true;
        }
    }
} 