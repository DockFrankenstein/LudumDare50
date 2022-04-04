using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{ 
    public class EnviromentCommunicationText : MonoBehaviour
    {
        [SerializeField] float triggerRange;
        [SerializeField] TextTypeAnimation anim;
        [SerializeField] UnityEvent OnTrigger;

        bool _triggered;

        private void Update()
        {
            if (_triggered) return;
            if (PlayerReference.Singleton == null) return;

            if (Vector3.Distance(PlayerReference.Singleton.transform.position, transform.position) <= triggerRange)
            {
                _triggered = true;
                anim?.Play();
                OnTrigger.Invoke();
            }
        }
    }
}
