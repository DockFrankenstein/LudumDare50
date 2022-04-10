using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] string format = "v{0}";

        private void Awake()
        {
            text.text = string.Format(format, Application.version);
        }

        private void Reset()
        {
            text = GetComponent<TMP_Text>();
        }
    }
}
