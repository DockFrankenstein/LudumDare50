using UnityEngine;
using qASIC;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Game.Save
{
    public class SaveManager : MonoBehaviour
    {
        private const float revertDuration = 0.2f;

        public static SaveManager Singleton { get; private set; }

        int newestVersion = -1;
        int currentVersion = -1;
        List<SaveNode> nodes = new List<SaveNode>();

        public static event Action OnRevert;

        public int NewestVersion => newestVersion;
        public int CurrentVersion => currentVersion;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                return;
            }

            if (Singleton != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            Save();
        }

        public static void AddNode(SaveNode node)
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Save] Cannot add node, singleton not assigned!");
                return;
            }

            Singleton.nodes.Add(node);
        }

        public static int Save()
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Save] Cannot save, singleton not assigned!");
                return 0;
            }

            Singleton.currentVersion = ++Singleton.newestVersion;

            for (int i = 0; i < Singleton.nodes.Count; i++)
                Singleton.nodes[i].CreateVersion(Singleton.currentVersion);

            qDebug.Log($"[Save] Successfully created state save, version: {Singleton.currentVersion}", "save");
            return Singleton.currentVersion;
        }

        public static void RevertInstant(int version)
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Save] Cannot revert, singleton not assigned!");
                return;
            }

            if (version < 0 || version > Singleton.newestVersion)
            {
                qDebug.LogError("[Save] Cannot revert, version out of range!");
                return;
            }

            Singleton.currentVersion = version;

            for (int i = 0; i < Singleton.nodes.Count; i++)
                Singleton.nodes[i].RevertVersion(Singleton.currentVersion);

            qDebug.Log($"[Save] Save reverted to version '{Singleton.currentVersion}'", "save");
        }

        public static void RevertInstant()
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Save] Cannot revert, singleton not assigned!");
                return;
            }

            RevertInstant(Singleton.currentVersion);
        }

        public static void Revert()
        {
            if (Singleton == null)
            {
                qDebug.LogError("[Save] Cannot revert, singleton not assigned!");
                return;
            }

            OnRevert?.Invoke();
            Singleton.StartCoroutine(WaitForRevert());
        }

        private static IEnumerator WaitForRevert()
        {
            yield return new WaitForSecondsRealtime(revertDuration);
            RevertInstant();
        }
    }
}