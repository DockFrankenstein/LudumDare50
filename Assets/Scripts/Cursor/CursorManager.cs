using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class CursorManager
    {
        public static List<string> States { get; private set; } = new List<string>();

        public static bool CanLook => States.Count == 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void Initialize()
        {
            ChangeState("global", false);
        }

        public static void ChangeState(string stateName, bool state)
        {
            stateName = stateName.ToLower();
            if (state)
            {
                States.Add(stateName);
                RefreshLockMode();
                return;
            }

            if (States.Contains(stateName))
                States.Remove(stateName);

            RefreshLockMode();
        }

        public static bool GetState(string stateName) =>
            States.Contains(stateName.ToLower());

        static void RefreshLockMode()
        {
            Cursor.lockState = CanLook ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
