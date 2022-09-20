using System;
using Core;
using Movement;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction

    {
        [SerializeField] private float weaponRange;

        [SerializeField] private float timeBetweenAttacks;

        private Transform _target;

        private float _timeSinceLastAttack = 0;

        private Mover _mover;

        private ActionScheduler _actionScheduler;

        private Animator _animator;
        private static readonly int Attack1 = Animator.StringToHash("attack");

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;

            if (!GetInRange())
            {
                _mover.MoveTo(_target.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack > timeBetweenAttacks)
            {
                _animator.SetTrigger(Attack1);
                _timeSinceLastAttack = 0;
            }
        }

        private bool GetInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < weaponRange;
        }

        private void Hit()
        {
            
        }

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}
