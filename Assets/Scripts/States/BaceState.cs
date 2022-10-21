using System;
using UnityEngine;

namespace States
{
    public class BaceState : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] private int startLevel;

        [SerializeField] private CharacterClass characterClass;

        [SerializeField] private Progression progression;

        [SerializeField] private GameObject levelUpParticleEffect;

        [SerializeField] private bool shouldUseModifires;

        public event Action onLevelUp;

        private int _currentLevel;

        private Experience _experience;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = CalculateLevel();
        }
        
        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaceStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaceStat(Stat stat)
        {
            return progression.GetStats(stat,characterClass,GetLevel());
        }

       
        public int GetLevel()
        {
            return _currentLevel;
        }
        
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifires) return 0;
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetAdditiveModifier(stat))
                {
                    total += modifiers;
                }
            }

            return total;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifier(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startLevel;
            
            
            float currentXp = GetComponent<Experience>().GetPoint();

            int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp,characterClass);
            
            for (int levels = 1; levels <= maxLevel ; levels++)
            {
                float xpToLevelUp = progression.GetStats(Stat.ExperienceToLevelUp, characterClass, levels);

                if (xpToLevelUp > currentXp)
                {
                    return levels;
                }
            }
            return maxLevel + 1;
        }
    }
}
