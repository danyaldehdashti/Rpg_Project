using Core;
using Saving;
using States;
using UnityEngine;
using UnityEngine.Events;

namespace Attributes
{
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] private float regenerationPercentage;

        [SerializeField] private UnityEvent<float> takeDamage;

        [SerializeField] private UnityEvent onDie;  
        
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }
        
        private float _healthPoint;

        private Animator _animator;
        
        private bool _isDead = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _healthPoint = GetInitialHealth();
        }

        private float GetInitialHealth()
        {
            Debug.Log(GetComponent<BaceState>().GetStat(Stat.Health));
            return GetComponent<BaceState>().GetStat(Stat.Health);
        }
        
        private void OnEnable()
        {
            GetComponent<BaceState>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaceState>().onLevelUp -= RegenerateHealth;

        }

        private void Die()
        {
            if(_isDead) return;

            _isDead = true; 
            _animator.SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            
            if(experience == null) return;
            
            experience.GainExperience(GetComponent<BaceState>().GetStat(Stat.ExperienceReward ));
        }
        
        private void RegenerateHealth()
        {
            float reagenHealthPoint = GetComponent<BaceState>().GetStat(Stat.Health) * regenerationPercentage / 100;

            _healthPoint = Mathf.Max(_healthPoint, reagenHealthPoint);
        }

        
        public bool IsDead()
        {
            return _isDead;
        }
        
        public void TakeDamage(GameObject instigator,float damage)
        {
            Debug.Log(gameObject.name + " Took Damage: " + damage);
            
            _healthPoint = Mathf.Max(_healthPoint - damage, 0);
            
            if (_healthPoint == 0)
            {
                if (_isDead) return;
                AwardExperience(instigator);
                onDie.Invoke();
                Die();
            }
            else
            {
                takeDamage.Invoke(damage); 
            }
        }

        public void Heal(float amount)
        {
            _healthPoint = Mathf.Min(_healthPoint + amount, GetMaxHealthPoints());
        }

        public float GetHealthPoints()
        {
            return _healthPoint;
        }

        public float GetMaxHealthPoints()
        {
            return  GetComponent<BaceState>().GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return _healthPoint;
        }
        
        public float GetFraction()
        {
            return _healthPoint / GetComponent<BaceState>().GetStat(Stat.Health);
        }

        public void RestoreState(object state)
        {
            _healthPoint = (float)state;
            
            if (_healthPoint  == 0)
            { 
                Die();
            }
        }
    }
}
