using UnityEngine;
using System;

namespace Game.Commentary
{
    public class CommentaryController : MonoBehaviour
    {
        [SerializeField] GameObject text;

        private static event Action OnChangeShow;
        static bool _showCommentary = true;
        public static bool ShowCommentary
        {
            get => _showCommentary;
            set
            {
                _showCommentary = value;
                OnChangeShow?.Invoke();
            }
        }

        private void Awake() =>
            UpdateActive();

        private void OnEnable()
        {
            OnChangeShow += UpdateActive;
        }

        private void OnDisable()
        {
            OnChangeShow -= UpdateActive;
        }

        void UpdateActive()
        {
            text?.SetActive(_showCommentary);
        }
    }
}