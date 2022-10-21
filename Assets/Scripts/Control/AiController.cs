using System;
using Attributes;
using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class AiController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance;

        [SerializeField] private float suspicionTime;
        
        [SerializeField] private float agroCoolDownTime ;

        [SerializeField] private PatrolPath patrolPath;

        [SerializeField] private float wayPointToLerance;

        [SerializeField] private float wayPointDwellTime;

        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction;
        
        [SerializeField] private float shoutDistance;
            
        private Fighter _fighter;

        private GameObject _player;

        private Health _health;
        
        private Mover _mover;

        private Vector3 _guardLocation;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        private float _timeSinceArrivedAtWayPoint = Mathf.Infinity;

        private float _timeSinceAggrevate = Mathf.Infinity;

        private int _currentWayPointIndex = 0;



        private void Awake()
        {
            _fighter = GetComponent<Fighter>(); 
            _player = FindObjectOfType<PlayerController>().gameObject;
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
        }

        private void Start()
        {
            _guardLocation = gameObject.transform.position;
        }
        

        private void Update()
        {
            if(_health.IsDead()) return;

            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                _fighter.Cancel();
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            _timeSinceAggrevate = 0;
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWayPoint += Time.deltaTime;
            _timeSinceAggrevate += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardLocation;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    _timeSinceArrivedAtWayPoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }

            if (_timeSinceArrivedAtWayPoint > wayPointDwellTime)
            {
                _mover.StartMoveAction(nextPosition,patrolSpeedFraction) ;
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(gameObject.transform.position, GetCurrentWayPoint());

            return distanceToWayPoint < wayPointToLerance;
        }

        private void CycleWayPoint()
        {
            _currentWayPointIndex = patrolPath.GetNextIndex(_currentWayPointIndex); 
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(_currentWayPointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(gameObject.transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits)
            {
                AiController ai = hit.collider.GetComponent<AiController>();
                
                if(ai == null) continue;
                
                ai.Aggrevate();
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            float distance = Vector3.Distance(_player.transform.position, gameObject.transform.position);
            return distance < chaseDistance || _timeSinceAggrevate < agroCoolDownTime;
        }

        // Called By Unity
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position,chaseDistance);
        }
        
    }
}
