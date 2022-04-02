using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Game.UI
{
    [ExecuteInEditMode]
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Image")]
        [SerializeField] Image background;
        [SerializeField] Color unselectedBackground;
        [SerializeField] Color selectedBackground = Color.white;

        [Header("Text")]
        [SerializeField] TMP_Text text;
        [SerializeField] Color unselectedTextColor = Color.white;
        [SerializeField] Color selectedTextColor = Color.black;
        [SerializeField] string unselectedTextFormat = "{0}";
        [SerializeField] string selectedTextFormat = ">{0}";

        string buttonText;

        private void Reset()
        {
            background = GetComponent<Image>();
            text = GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            if (text.text == null) return;
            text.text = gameObject.name;
        }

        private void Awake()
        {
            buttonText = text.text;
            ResetButton();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            background.color = selectedBackground;
            text.text = string.Format(selectedTextFormat, buttonText);
            text.color = selectedTextColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetButton();
        }

        void ResetButton()
        {
            background.color = unselectedBackground;
            text.text = string.Format(unselectedTextFormat, buttonText);
            text.color = unselectedTextColor;
        }
    }
}