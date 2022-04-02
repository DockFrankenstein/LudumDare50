using qASIC.InputManagement;
using UnityEngine;
using qASIC;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public static bool Noclip { get; private set; } = false;
        public static float SpeedMultiplier { get; set; } = 1f;

        public static void SetNoclipActive(bool state)
        {
            Noclip = state;

            if (PlayerReference.Singleton?.move == null) return;
            PlayerMovement singleton = PlayerReference.Singleton.move;

            if (singleton.controller == null) return;

            singleton.controller.enabled = !Noclip;
        }

        public CharacterController controller;
        public Transform cameraTransform;

        [Header("Input")]
        [SerializeField] InputAxisReference horizontalAxis;
        [SerializeField] InputAxisReference verticalAxis;
        [SerializeField] InputActionReference sprintKey;
        [SerializeField] InputActionReference jumpKey;
        [SerializeField] InputActionReference crouchKey;

        [Header("Movement")]
        [SerializeField] float speed = 6f;
        [SerializeField] float airSpeed = 3f;
        [SerializeField] float sprint = 10f;
        [SerializeField] float noclipSpeed = 16f;

        //Gravity
        public bool IsGround { get; private set; }
        public bool IsGroundPrevious { get; private set; }
        public float GravityVelovity { get; private set; }

        [Header("Gravity")]
        [SerializeField] float gravity = 30f;
        [SerializeField] float groundVelocity = 2f;
        [SerializeField] float jumpHeight = 2f;
        [SerializeField] Vector3 groundPointOffset = new Vector3(0f, -0.6f, 0f);
        [SerializeField] float groundPointRadius = 0.5f;
        [SerializeField] LayerMask layer;
        [SerializeField] float coyoteTime = 0.15f;
        [SerializeField] float jumpQueue = 0.2f;

        Vector3 _lastPath;

        private void Awake()
        {
            if (controller == null)
                controller = GetComponent<CharacterController>();

            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            if (controller != null)
            {
                controller.enabled = !Noclip;
            }
        }

        private void Update()
        {
            Move();
        }

        void Move()
        {
            Vector3 input = new Vector2(InputManager.GetMapAxisRaw(horizontalAxis), InputManager.GetMapAxisRaw(verticalAxis));
            Vector3 path = new Vector2();

            switch (Noclip)
            {
                case false:
                    path = GetNormalPath(input);
                    break;
                case true:
                    path = GetNoclipPath(input);
                    break;
            }

            switch (controller?.enabled == true)
            {
                case true:
                    controller?.Move(path * Time.deltaTime);
                    break;
                case false:
                    transform.position += path * Time.deltaTime;
                    break;
            }

            _lastPath = path;

            qDebug.DisplayValue("input", input);
            qDebug.DisplayValue("path", path);
            qDebug.DisplayValue("ground", $"{IsGround}, prev: {IsGroundPrevious}");
        }

        Vector3 GetNormalPath(Vector2 input)
        {
            Vector3 path = new Vector3();
            Vector3 inputPath = (transform.right * input.x + transform.forward * input.y).normalized;

            switch (IsGround)
            {
                case true:
                    path = inputPath;
                    path *= InputManager.GetInput(sprintKey) ? sprint : speed;
                    break;
                case false:
                    path = _lastPath + inputPath * airSpeed;
                    path.y = 0f;
                    path = Vector3.ClampMagnitude(path, sprint);
                    break;
            }

            path.y = GetGravityPath();
            return path * SpeedMultiplier;
        }

        float _lastGroundTime;
        bool _acceptCoyoteTime;
        float _lastJumpQueueTime;

        float GetGravityPath()
        {
            IsGround = Physics.CheckSphere(transform.position + groundPointOffset, groundPointRadius, layer);
            bool jumpPressed = InputManager.GetInputDown(jumpKey);

            bool forceJump = false;
            if (IsGround && !IsGroundPrevious)
            {
                if (Time.time - _lastJumpQueueTime <= jumpQueue)
                    forceJump = true;

                _acceptCoyoteTime = true;
                _lastJumpQueueTime = 0f;
            }

            switch (IsGround)
            {
                case true:
                    _lastGroundTime = Time.time;
                    GravityVelovity = -groundVelocity;
                    break;
                case false:
                    if (jumpPressed)
                        _lastJumpQueueTime = Time.time;

                    GravityVelovity -= gravity * Time.deltaTime;
                    break;
            }

            IsGroundPrevious = IsGround;

            if ((forceJump || //force jump
                (IsGround || (Time.time - _lastGroundTime <= coyoteTime && _acceptCoyoteTime)) && //coyote time
                jumpPressed) && 
                CursorManager.CanLook)
            {
                GravityVelovity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                _acceptCoyoteTime = false;
            }

            return GravityVelovity;
        }


        Vector3 GetNoclipPath(Vector2 input) =>
            (cameraTransform.right * input.x + 
            cameraTransform.forward * input.y + 
            ((InputManager.GetInput(jumpKey) ? 1f : 0f) - (InputManager.GetInput(crouchKey) ? 1f : 0f)) * Vector3.up)
            .normalized * noclipSpeed * SpeedMultiplier;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + groundPointOffset, groundPointRadius);
        }
    }
}