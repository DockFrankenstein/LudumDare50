using UnityEngine;

namespace Game.Editor
{
    public class MeasurePoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public static MeasurePreset? currentPreset = null;
        public static float Rotation;

        private void OnDrawGizmos()
        {
            if (currentPreset == null) return;
            MeasurePreset preset = currentPreset ?? new MeasurePreset();

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = preset.color;
            Gizmos.DrawCube(preset.GetPosition(), preset.GetSize());
        }
#endif
    }

#if UNITY_EDITOR
    [System.Serializable]
    public struct MeasurePreset
    {
        public string name;
        public float length;
        public Color color;
        public float width;
        public float height;

        public Vector3 GetPosition() =>
            new Vector3(length / 2f, 0f, 0f);

        public Vector3 GetSize() =>
            new Vector3(length, height, width);

        public MeasurePreset(MeasurePreset preset)
        {
            name = preset.name;
            length = preset.length;
            color = preset.color;
            width = preset.width;
            height = preset.height;
        }
    }
#endif
}