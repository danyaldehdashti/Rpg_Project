using System.Collections.Generic;
using UnityEngine;
namespace States
{
    [CreateAssetMenu(fileName = "Progression",menuName = "States/New Progression",order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookUpTable = null;
 
        public float GetStats(Stat stat,CharacterClass characterClass, int level)
        {
            BuildLookUp();

            float[] levels =  _lookUpTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        private void BuildLookUp()
        {
            if (_lookUpTable != null) return;

            _lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels; 
                }
                
                _lookUpTable[progressionClass.characterClass] = statLookUpTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            float[] levels = _lookUpTable[characterClass][stat];

            return levels.Length; 
        }


        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;

            public ProgressionStat[] stats;
        }
        
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
