using TMPro;
using UnityEngine;
using qASIC.Options;

namespace Game
{
    public class TutorialTextController : MonoBehaviour
    {
        [SerializeField] TMP_Text text;

        public static bool ShowTutorial { get; set; } = true;

        [OptionsSetting("tutorial", true)]
        public static void ChangeShowTutorial(bool showTutorial) =>
            ShowTutorial = showTutorial;

        private void Reset()
        {
            text = GetComponent<TMP_Text>();
        }

        public static string TutorialText { get; set; }

        private void Update()
        {
            text.text = ShowTutorial ? TutorialText : string.Empty;
        }
    }
} 