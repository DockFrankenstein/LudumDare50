using qASIC.Options;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

namespace Game
{
    public class PlayerCamera : MonoBehaviour
    {
        [OptionsSetting("sensitivity", 1f)]
        public static void ChangeSensitivity(float value) => Sensitivity = value;

        public static float Sensitivity { get; set; } = 1f;

        [HideInInspector] public float yRotation;

        public Camera cam;
        public Camera uiCam;
        public UniversalAdditionalCameraData camData;
        public UniversalAdditionalCameraData uiCamData;

        public static bool PostProcessing { get; private set; } = true;
        public static float FOV { get; private set; } = 60f;

        [OptionsSetting("postprocessing", true)]
        public static void ChangePostProcessing(bool value)
        {
            PostProcessing = value;
            OnUpdateCameras?.Invoke();
        }

        [OptionsSetting("fov", 60f)]
        public static void ChangeFOV(float value)
        {
            FOV = value;
            OnUpdateCameras?.Invoke();
        }

        static event Action OnUpdateCameras;

        private void Awake()
        {
            UpdateCamera();
        }

        private void OnEnable()
        {
            OnUpdateCameras += UpdateCamera;
        }

        private void OnDisable()
        {
            OnUpdateCameras -= UpdateCamera;
        }

        private void Update()
        {
            if (CursorManager.CanLook)
                Look();
        }

        void UpdateCamera()
        {
            camData.renderPostProcessing = PostProcessing;
            uiCamData.renderPostProcessing = PostProcessing;
            cam.fieldOfView = FOV;
        }

        void Look()
        {
            yRotation = Mathf.Clamp(yRotation - Input.GetAxis("Mouse Y") * Sensitivity, -90f, 90f);

            Vector3 camRotation = cam.transform.eulerAngles;
            camRotation.x = yRotation;
            cam.transform.eulerAngles = camRotation;

            Vector3 transformRotation = transform.eulerAngles;
            transformRotation.y += Input.GetAxis("Mouse X") * Sensitivity;
            transform.eulerAngles = transformRotation;
        }
    }
}
