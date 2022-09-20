using System;
using Core;
using Movement;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction

    {
        [SerializeField] private float weaponRange;

        private Transform _target;

        private Mover _mover;

        private ActionScheduler _actionScheduler;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if (_target == null) return;

            if (!GetInRange())
            {
                _mover.MoveTo(_target.position);
            }
            else
            {
                _mover.Cancel();
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
