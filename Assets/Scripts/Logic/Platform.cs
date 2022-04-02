using UnityEngine;
using qASIC;

namespace Game.Logic
{
    public class Platform : MonoBehaviour, IActivatable
    {
        [InspectorLabel("Start Position")] [SerializeField] Vector3 startPos;
        [InspectorLabel("Start Position")] [SerializeField] Vector3 endPos;
        [SerializeField] Vector3 offset;
        [SerializeField] VelocityTransmitter platform;
        [SerializeField] float speed = 3f;
        [SerializeField] bool invertActivation;
        [SerializeField] bool loop;

        [Header("Debug")]
        [SerializeField] Vector3 platformSize = Vector3.one;
        [SerializeField] Vector3 debugOffset;
        [SerializeField] TMPro.TextMeshPro debugText;

        public bool Active { get; private set; } = false;
        public static bool Debug { get; set; } = false;

        float _time;
        private bool _reverse;
        Vector3 _pos;

        private void Awake()
        {
            Active = invertActivation;
        }

        public void Activate(bool state)
        {
            Active = state == !invertActivation;
        }

        private void Update()
        {
            MovePlatform();

            if (debugText != null)
                debugText.text = $"start: {startPos}\n" +
                    $"end: {endPos}\n" +
                    $"offset: {offset} speed: {speed} loop: {loop} invert: {invertActivation}\n" +
                    $"active: {Active}\n" +
                    $"current: {_pos}\n" +
                    $"time: {_time}\n" +
                    $"reverse: {_reverse}";
        }

        private void FixedUpdate()
        {
            debugText.gameObject.SetActive(Debug);
        }

        void MovePlatform()
        {
            if (!Active && loop)
            {
                platform.velocity = Vector3.zero;
                return;
            }

            if (!loop)
                _reverse = !Active;

            float distance = Mathf.Abs(Vector3.Distance(startPos, endPos));

            _time += Time.deltaTime * speed * (_reverse ? -1f : 1f);
            _time = Mathf.Clamp(_time, loop ? -distance : 0f, distance);

            Vector3 oldPos = platform.transform.position;
            _pos = Vector3.Lerp(startPos, endPos, Mathf.Abs(_time / distance));
            Vector3 newPos = transform.position + offset + _pos;
            platform.transform.position = transform.position + offset + _pos;
            platform.velocity = (newPos - oldPos).normalized * speed;


            if (Mathf.Abs(_time) >= distance && loop)
                _reverse = !_reverse;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + offset + debugOffset + startPos, platformSize);

            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position + offset + debugOffset + endPos, platformSize);
        }
    }
}