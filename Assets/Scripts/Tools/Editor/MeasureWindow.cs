using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using qASIC.FileManagement;

namespace Game.Editor
{
    public class MeasureWindow : EditorWindow
    {
        [MenuItem("Window/Measure")]
        static void ShowWindow()
        {
            MeasureWindow window = (MeasureWindow)GetWindow(typeof(MeasureWindow));
            window.titleContent = new GUIContent("Measure");
            window.Show();
        }

        private void OnEnable()
        {
            LoadPresets();
            currentPreset = presets[CurrentPreset];
        }

        string GetPath() =>
            $"{Application.persistentDataPath}/editor-measurements.json";

        void LoadPresets()
        {
            PresetWrapper loadedPresets = new PresetWrapper();
            if (!FileManager.TryReadFileJSON(GetPath(), loadedPresets)) return;

            presets = loadedPresets.presets;
        }

        static List<MeasurePreset> presets = new List<MeasurePreset>();

        MeasurePreset currentPreset = new MeasurePreset()
        {
            name = "Empty",
            length = 3f,
            color = Color.green,
            width = 1f,
            height = 0.3f,
        };

        int? _currentPreset = null;
        public int CurrentPreset
        {
            get
            {
                if (_currentPreset == null)
                    _currentPreset = EditorPrefs.GetInt("measurement_preset", -1);

                return _currentPreset ?? -1;
            }
            set
            {
                EditorPrefs.SetInt("measurement_preset", value);
                _currentPreset = value;
            }
        }

        int? presetPopUp = null;

        private void OnGUI()
        {
            string[] presetsContent = new string[presets.Count + 1];
            presetsContent[0] = "Custom";
            for (int i = 0; i < presets.Count; i++)
                presetsContent[i + 1] = presets[i].name;

            if (presetPopUp == null)
                presetPopUp = CurrentPreset;

            presetPopUp = EditorGUILayout.Popup("Presets", (presetPopUp ?? -1) + 1, presetsContent) - 1;
            if (GUILayout.Button("Load") && presetPopUp >= 0)
            {
                CurrentPreset = presetPopUp ?? 0;
                currentPreset = new MeasurePreset(presets[CurrentPreset]);
            }

            if (GUILayout.Button("Delete") && presetPopUp >= 0)
                presets.RemoveAt(presetPopUp ?? 0);

            EditorGUILayout.Space();

            currentPreset.name = EditorGUILayout.TextField("Name", currentPreset.name);
            currentPreset.length = EditorGUILayout.FloatField("Length", currentPreset.length);
            currentPreset.color = EditorGUILayout.ColorField("Color", currentPreset.color);
            currentPreset.width = EditorGUILayout.FloatField("Width", currentPreset.width);
            currentPreset.height = EditorGUILayout.FloatField("Height", currentPreset.height);

            EditorGUILayout.Space();

            if (GUILayout.Button("Save preset"))
                SavePreset();

            if (GUILayout.Button("Save"))
                Save();

            MeasurePoint.currentPreset = currentPreset;
        }

        void SavePreset()
        {
            for (int i = 0; i < presets.Count; i++)
            {
                if (presets[i].name != currentPreset.name) continue;
                presets[i] = new MeasurePreset(currentPreset);
                return;
            }

            presets.Add(new MeasurePreset(currentPreset));
        }

        void Save()
        {
            FileManager.SaveFileJSON(GetPath(), new PresetWrapper(presets), true);
        }
    }   

    [System.Serializable]
    public class PresetWrapper
    {
        public List<MeasurePreset> presets = new List<MeasurePreset>();

        public PresetWrapper() { }

        public PresetWrapper(List<MeasurePreset> presets)
        {
            this.presets = presets;
        }
    }
}