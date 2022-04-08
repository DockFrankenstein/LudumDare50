using TMPro;
using UnityEngine;
using qASIC.Options;
using UnityEngine.SceneManagement;

namespace Game
{
    public class TutorialTextController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
            {
                if (mode == LoadSceneMode.Single)
                    TutorialText = string.Empty;
            };
        }

        [SerializeField] TMP_Text text;

        public static bool ShowTutorial { get; set; } = true;
        public static string TutorialText { get; set; }

        [OptionsSetting("tutorial", true)]
        public static void ChangeShowTutorial(bool showTutorial) =>
            ShowTutorial = showTutorial;

        private void Reset()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            text.text = ShowTutorial ? TutorialText : string.Empty;
        }
    }
} 