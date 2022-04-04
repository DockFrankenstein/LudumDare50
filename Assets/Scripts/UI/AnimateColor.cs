using UnityEngine.UI;
using UnityEngine;

namespace Game
{
    public class AnimateColor : MonoBehaviour
    {
        [SerializeField] Image target;
        [SerializeField] AnimationCurve curve;
        [SerializeField] float duration;
        [SerializeField] Color minColor;
        [SerializeField] Color maxColor;

        private void Update()
        {
            target.color = Color.Lerp(minColor, maxColor, curve.Evaluate(Time.time)); 
        }
    }
}
