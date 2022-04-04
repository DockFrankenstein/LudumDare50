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
            point = transform.position;
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
            if (PlayerMovement.IsGround && PlayerMovement.AdditionalVelocity == Vector3.zero && Vector3.Distance(transform.position, point) >= stepLength)
                TakeStep();
        }

        void TakeStep()
        {
            point = transform.position;
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();
        }
    }
}