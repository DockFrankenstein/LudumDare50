using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace Game
{
    public class ExecuteWithDelay : MonoBehaviour
    {
        [SerializeField] UnityEvent OnExecute;

        public void Execute(float delay)
        {
            StartCoroutine(DelayAndExecute(delay));
        }

        IEnumerator DelayAndExecute(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            OnExecute.Invoke();
        }
    }
} 