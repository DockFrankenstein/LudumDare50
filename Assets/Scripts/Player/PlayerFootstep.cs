using UnityEngine;

namespace Game.Player
{
    public class PlayerFootstep : MonoBehaviour
    {
        [SerializeField] float stepLength = 5f;
        [SerializeField] AudioSource source;
        [SerializeField] AudioClip[] clips;

        Vector3 point;

        private void Reset()
        {
            source = GetComponent<AudioSource>();
        }

        private void Awake()
        {
            ResetPoint();
        }

        private void OnEnable()
        {
            PlayerMovement.OnLand += TakeStep;
        }

        private void OnDisable()
        {
            PlayerMovement.OnLand -= TakeStep;
        }

        private void Update()
        {
            if (PlayerMovement.Noclip)
                ResetPoint();

            if (!PlayerMovement.IsGround ||
                PlayerMovement.AdditionalVelocity != Vector3.zero ||
                Vector3.Distance(transform.position, point) < stepLength * PlayerMovement.SpeedMultiplier)
                return;

            TakeStep();
        }

        void ResetPoint() =>
            point = transform.position;

        void TakeStep()
        {
            ResetPoint();
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();
        }
    }
}
