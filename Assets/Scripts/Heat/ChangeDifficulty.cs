using UnityEngine;

namespace Game.Heat
{
    public class ChangeDifficulty : MonoBehaviour
    {
        [SerializeField] bool changeOnAwake;
        [SerializeField] int difficulty;

        private void Awake()
        {
            if (changeOnAwake)
                HeatController.Difficulty = difficulty;
        }

        public void SetDifficulty(int difficulty) =>
            HeatController.Difficulty = difficulty;

        public void SetDifficulty() =>
            HeatController.Difficulty = difficulty;
    } 
}