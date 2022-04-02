using qASIC.Options;
using UnityEngine;

namespace Game
{
    public class PlayerCamera : MonoBehaviour
    {
        [OptionsSetting("sensitivity", 1f)]
        public static void ChangeSensitivity(float value) => Sensitivity = value;

        public static float Sensitivity { get; set; } = 1f;

        [HideInInspector] public float yRotation;

        public Camera cam;

        private void Update()
        {
            if (CursorManager.CanLook)
                Look();
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
