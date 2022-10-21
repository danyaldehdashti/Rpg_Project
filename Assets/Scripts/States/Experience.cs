using System;
using Saving;
using UnityEngine;

namespace States
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] private float experiencePoint;
        
        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoint += experience;
            onExperienceGained();
        }

        public float GetPoint()
        {
            return experiencePoint;
        }

        public object CaptureState()
        {
            return experiencePoint;
        }

        public void RestoreState(object state)
        {
            experiencePoint = (float)state;
            onExperienceGained();
        }
    }
}
