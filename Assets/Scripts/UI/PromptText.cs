using TMPro;
using UnityEngine;
using qASIC.InputManagement;

namespace Game
{
    public class PromptText : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] string format;
        [SerializeField] InputActionReference input;
        [SerializeField] int keyIndex = 0;

        private void Reset()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            text.text = string.Format(format, InputManager.GetKeyCode(input.GroupName, input.ActionName, keyIndex));
        } 
    }
}
