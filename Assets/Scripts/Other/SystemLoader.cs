using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using qASIC;

namespace Game
{
    public class SystemLoader : MonoBehaviour
    {
        const string sceneName = "Systems";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void LoadSystems()
        {
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                qDebug.LogError("Cannot load systems scene - scene doesn't exist!");
                return;
            }

            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        private void FixedUpdate()
        {
            List<GameObject> systems = new List<GameObject>();
            gameObject.scene.GetRootGameObjects(systems);
            if (systems.Count > 1) return;

            SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
        }
    }
}