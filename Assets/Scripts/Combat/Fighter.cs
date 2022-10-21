using System;
using System.Collections.Generic;
using Attributes;
using Core;
using Movement;
using Saving;
using States;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction,ISaveable,IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks;
        
        [SerializeField] private Transform rightHandTransform;

        [SerializeField] private Transform leftHandTransform;

        [FormerlySerializedAs("defaultWeapon")] [SerializeField] private WeaponConfig defaultWeaponConfig;
        
        private Health _target;
        
        private WeaponConfig _currentWeaponConfig;

        private Weapon _currentWeapon;

        private Mover _mover;

        private ActionScheduler _actionScheduler;

        private Animator _animator;
        
        private float _timeSinceLastAttack = Mathf.Infinity;

        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");
        

       
        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _currentWeaponConfig = defaultWeaponConfig;
            _currentWeapon = SetupDefaultWeapon();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponConfig);
        }
        
        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            _currentWeaponConfig = weaponConfig;
            _currentWeapon = AttachWeapon(weaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            if(_target.IsDead()) return;
            

            if (!GetInRange(_target.transform))
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            gameObject.transform.LookAt(_target.transform);
            
            if (_timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger(StopAttack);
            _animator.SetTrigger(Attack1);
        }

        
        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.GetPercentageBonus();
            }
        }

        private bool GetInRange( Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.transform.position) < _currentWeaponConfig.GetRange();
        }


        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) {return false;}

            if (!_mover.CanMoveTo(combatTarget.transform.position) &&
                !GetInRange(combatTarget.transform))
            {
                return false;
            }
            
            Health targetToTest = combatTarget.GetComponent<Health>();

            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }
        
        //Animation Event
        private void Hit()
        {
            if(_target == null) {return;}

            float damage = GetComponent<BaceState>().GetStat(Stat.Damage);

            if (_currentWeapon != null)
            {
                _currentWeapon.OnHit();
            }

            if (_currentWeaponConfig.HasProjectile())
            {
                _currentWeaponConfig.LaunchProjectile(rightHandTransform,leftHandTransform,_target,gameObject,damage);
            }
            else
            {
                _target.TakeDamage(gameObject,damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        
        private void StopAttackTrigger()
        {
            _animator.ResetTrigger(StopAttack);
            _animator.SetTrigger(StopAttack);
        }

        public void Cancel()
        {
            StopAttackTrigger();
            _target = null;
            _mover.Cancel();
        }
        
        
        public object CaptureState()
        {
            return _currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weaponConfig = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weaponConfig);
        }
        public Health GetTarget()
        {
            return _target;
        }
    }
}
