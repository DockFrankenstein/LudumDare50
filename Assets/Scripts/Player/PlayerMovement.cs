using qASIC.InputManagement;
using UnityEngine;
using qASIC;
using System;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public static bool Noclip { get; private set; } = false;
        public static bool UnlockAirTime { get; set; } = false;
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
        [SerializeField] Vector3 topPointOffset = new Vector3(0f, 0.6f, 0f);
        [SerializeField] float topPointRadius = 0.5f;
        [SerializeField] LayerMask layer;
        [SerializeField] float coyoteTime = 0.15f;
        [SerializeField] float jumpQueue = 0.2f;

        [Header("Dash")]
        [SerializeField] int dashLimit = 3;
        [SerializeField] float dashDuration = 0.5f;
        [SerializeField] float dashRechargeDuration = 0.2f;
        [SerializeField] float dashSpeed = 30f;
        [SerializeField] float dashLiftLimit = 0.5f;
        [SerializeField] float dashUpHeight = 4f;

        Vector3 _lastPath;

        public static event Action OnJump;

        public static bool IsWalking { get; private set; }
        public static bool IsSprinting { get; private set; }
        public static Vector3 AdditionalVelocity { get; private set; }

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

            _lastPath = path;
            path += AdditionalVelocity;

            switch (controller?.enabled == true)
            {
                case true:
                    controller?.Move(path * Time.deltaTime);
                    break;
                case false:
                    transform.position += path * Time.deltaTime;
                    break;
            }


            qDebug.DisplayValue("input", input);
            qDebug.DisplayValue("path", path);
            qDebug.DisplayValue("ground", $"{IsGround}, prev: {IsGroundPrevious}");
            qDebug.DisplayValue("isWalking", IsWalking);
            qDebug.DisplayValue("isSprinting", IsSprinting);
            qDebug.DisplayValue("AdditionalVelocity", AdditionalVelocity);
            qDebug.DisplayValue("isDashing", _isDashing);
            qDebug.DisplayValue("dashDirection", _dashDirection);
            qDebug.DisplayValue("usedDashes", $"{_usedDashes}/{dashLimit}");
        }

        int _usedDashes;

        bool _isDashing;
        float _dashStartTime;
        Vector3 _dashDirection;

        Vector3 GetNormalPath(Vector2 input)
        {
            Vector3 path = new Vector3();
            Vector3 inputPath = (transform.right * input.x + transform.forward * input.y).normalized;
            bool dash = !IsGround && Input.GetMouseButtonDown(0) && _usedDashes < dashLimit && Time.time - _dashStartTime > dashRechargeDuration && GravityVelovity < dashLiftLimit;

            float gravityPath = _isDashing ? 0f : GetGravityPath();

            if (dash)
                Dash(inputPath);

            switch (IsGround || UnlockAirTime)
            {
                case true:
                    IsSprinting = InputManager.GetInput(sprintKey);
                    IsWalking = input.magnitude > 0;

                    CheckForAdditionalVelocity();

                    path = inputPath;
                    path *= IsSprinting ? sprint : speed;
                    path *= SpeedMultiplier;
                    break;
                case false:
                    switch (_isDashing)
                    {
                        case true:
                            path += _dashDirection * dashSpeed * SpeedMultiplier;

                            if (Time.time - _dashStartTime > dashDuration)
                                StopDash();
                            break;
                        case false:
                            path = _lastPath + inputPath * airSpeed * SpeedMultiplier;
                            path.y = 0f;
                            path = Vector3.ClampMagnitude(path, sprint * SpeedMultiplier);
                            break;
                    }
                    break;
            }

            path.y += gravityPath;
            return path;
        }

        void Dash(Vector3 inputPath)
        {
            _usedDashes++;

            if (inputPath.magnitude == 0)
            {
                GravityVelovity = Mathf.Sqrt(dashUpHeight * 2f * gravity);
                _lastPath = Vector3.zero;
                return;
            }

            _isDashing = true;
            _dashStartTime = Time.time;
            _dashDirection = inputPath;
            GravityVelovity = 0f;
        }

        public void StopDash()
        {
            _isDashing = false;
            _dashDirection = Vector3.zero;
        }

        bool _resetVelocityNextFixedUpdate;

        private void FixedUpdate()
        {
            if (_resetVelocityNextFixedUpdate)
            {
                if (!CheckForGround())
                    AdditionalVelocity = Vector3.zero;
                _resetVelocityNextFixedUpdate = false;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (((1 << hit.gameObject.layer) & layer) == 0)
                return;

            StopDash();
            if (!CheckForGround())
                _resetVelocityNextFixedUpdate = true;
        }

        void CheckForAdditionalVelocity()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + groundPointOffset, groundPointRadius, layer);

            for (int i = 0; i < colliders.Length; i++)
            {
                Logic.VelocityTransmitter transmitter = colliders[i].GetComponent<Logic.VelocityTransmitter>();
                if (transmitter == null) continue;
                AdditionalVelocity = transmitter.velocity;
                return;
            }
        }

        float _lastGroundTime;
        bool _acceptCoyoteTime;
        float _lastJumpQueueTime;

        float GetGravityPath()
        {
            IsGroundPrevious = IsGround;
            IsGround = CheckForGround();
            bool jumpPressed = InputManager.GetInputDown(jumpKey);

            bool forceJump = false;
            if (IsGround && !IsGroundPrevious)
            {
                if (Time.time - _lastJumpQueueTime <= jumpQueue)
                    forceJump = true;

                StopDash();
                _usedDashes = 0;
                _acceptCoyoteTime = true;
                _lastJumpQueueTime = 0f;
                _resetVelocityNextFixedUpdate = false;
            }

            switch (IsGround)
            {
                case true:
                    _lastGroundTime = Time.time;
                    GravityVelovity = -groundVelocity;
                    if (!forceJump)
                        AdditionalVelocity = Vector3.zero;
                    break;
                case false:
                    if (jumpPressed)
                        _lastJumpQueueTime = Time.time;

                    GravityVelovity -= gravity * Time.deltaTime;

                    //Check if head touches a celling
                    if (GravityVelovity > 0 && Physics.CheckSphere(transform.position + topPointOffset, topPointRadius, layer))
                        GravityVelovity = 0f;
                    break;
            }

            if ((forceJump || //force jump
                (IsGround || (Time.time - _lastGroundTime <= coyoteTime && _acceptCoyoteTime)) && //coyote time
                jumpPressed) && 
                CursorManager.CanLook)
            {
                OnJump?.Invoke();
                GravityVelovity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                _acceptCoyoteTime = false;
            }

            return GravityVelovity;
        }

        bool CheckForGround() =>
            Physics.CheckSphere(transform.position + groundPointOffset, groundPointRadius, layer);


        Vector3 GetNoclipPath(Vector2 input)
        {
            IsWalking = false;
            IsSprinting = false;

            return (cameraTransform.right * input.x + cameraTransform.forward * input.y + //WASD movement
                ((InputManager.GetInput(jumpKey) ? 1f : 0f) - (InputManager.GetInput(crouchKey) ? 1f : 0f)) * Vector3.up) //Up and down
                .normalized * noclipSpeed * SpeedMultiplier; //Speed
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + groundPointOffset, groundPointRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + topPointOffset, topPointRadius);
        }
    }
}