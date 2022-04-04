using UnityEngine;
using TMPro;

namespace Game
{
    public class TextTypeAnimation : MonoBehaviour
    {
        public TMP_Text text;
        [Min(1)] public int charactersPerSecond = 30;
        public bool auto = false;

        string _message;
        bool _isAnimating = false;
        float _time = 0f;

        public void Awake()
        {
            if (text == null)
                text = GetComponent<TMP_Text>();
        }

        public void Start()
        {
            _isAnimating = auto;

            if (!text) return;
            _message = text.text;
            text.text = "";
        }

        public void Play() =>
            _isAnimating = true;

        public void Update()
        {
            if (!text || !_isAnimating) return;

            int character = Mathf.RoundToInt(Mathf.Min(_message.Length, charactersPerSecond * _time));
            text.text = _message.Substring(0, character);
            _time += Time.deltaTime;
        }
    }
}