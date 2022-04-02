using UnityEngine;
using Game.Interaction;
using qASIC;
using System.Collections.Generic;

namespace Game.Logic
{
    public class Switch : MonoBehaviour, IInteractable
    {
        [Header("Setup")]
        [InspectorLabel("Targets")] [SerializeField] GameObject[] targetGameObjects;
        [HideInInspector] public List<IActivatable> targets = new List<IActivatable>();

        [SerializeField] bool oneTimeUse = true;
        [SerializeField] bool startState = false;

        [Header("Animation")]
        [SerializeField] Animator anim;
        [SerializeField] string animBoolName = "activated";

        public bool CurrentState { get; private set; } = false;

        private void Reset()
        {
            anim = GetComponent<Animator>();
        }

        private void Awake()
        {
            for (int i = 0; i < targetGameObjects.Length; i++)
            {
                IActivatable activatable = targetGameObjects[i].GetComponent<IActivatable>();
                if (activatable == null)
                    targets.Add(activatable);
            }
        }

        private void Start()
        {
            ChangeState(startState);
        }

        public void Interact() =>
            ToggleState();

        public void ToggleState() =>
            ChangeState(!CurrentState);

        public void ChangeState(bool state)
        {
            if (CurrentState == state || oneTimeUse && CurrentState) return;
            CurrentState = state;

            anim.SetBool(animBoolName, state);
            for (int i = 0; i < targets.Count; i++)
                targets[i].Activate(state);
        }
    }
}