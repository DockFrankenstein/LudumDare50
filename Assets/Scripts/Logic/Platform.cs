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

        public bool Reverse { get => _reverse; set => _reverse = value; }
        public float PlatformTime { get => _time; set => _time = value; }


        float _time;
        private bool _reverse;
        Vector3 _pos;
        float _distance;

        private void Awake()
        {
            Active = invertActivation;
            CalcDistance();
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

#if UNITY_EDITOR
            CalcDistance();
#endif

            _time += Time.deltaTime * speed * (_reverse ? -1f : 1f);
            _time = Mathf.Clamp(_time, loop ? -_distance : 0f, _distance);

            SetPosition();

            if (Mathf.Abs(_time) >= _distance && loop)
                _reverse = !_reverse;
        }

        void CalcDistance() =>
            _distance = Mathf.Abs(Vector3.Distance(startPos, endPos));

        public void SetPosition()
        {
            Vector3 oldPos = platform.transform.position;
            _pos = Vector3.Lerp(startPos, endPos, Mathf.Abs(_time / _distance));
            Vector3 newPos = transform.position + offset + _pos;
            platform.transform.position = transform.position + offset + _pos;
            platform.velocity = (newPos - oldPos).normalized * speed;
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