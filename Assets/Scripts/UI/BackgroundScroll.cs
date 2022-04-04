using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(RawImage))]
    [ExecuteInEditMode]
    public class BackgroundScroll : MonoBehaviour
    {
        private RawImage image;

        private void Awake() => image = GetComponent<RawImage>();

        public Vector2 offset = new Vector2();
        public Vector2 speed = new Vector2(1f, 1f);
        public Vector2 repeatCount = new Vector2(1f, 1f);

        private void Update()
        {
            offset += speed * Time.unscaledDeltaTime;
            Vector2 newSize = new Vector2(repeatCount.x * ((float)Screen.width / Screen.height), repeatCount.y);
            image.uvRect = new Rect(offset, newSize);
        }
    }
} 