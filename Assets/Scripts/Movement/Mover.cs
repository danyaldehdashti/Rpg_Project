using System;
using System.Collections.Generic;
using Attributes;
using Core;
using Saving;
using UnityEngine;
using UnityEngine.AI;
namespace Movement
{
    public class Mover : MonoBehaviour,IAction,ISaveable
    {
        [SerializeField] private float maxSpeed;

        [SerializeField] private bool isPlayer;
        
        [SerializeField] private float maxNavPathLength;

        
        private NavMeshAgent _navMeshAgent;
        
        private Animator _animator;
        
        private ActionScheduler _actionScheduler;

        private Health _health;

        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();
            
            UpdateAnimator();
        }
        
        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = gameObject.transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            _animator.SetFloat(Speed,speed);
        }
        
        private float  GatPathLength(NavMeshPath path)
        {
            float total = 0;

            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            
            return 0;
        }
        

        public void StartMoveAction(Vector3 destination,float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination,speedFraction);
        }

        public bool CanMoveTo(Vector3 direction)
        {
            NavMeshPath path = new NavMeshPath();
            
            bool hasPath =  NavMesh.CalculatePath(transform.position, direction, NavMesh.AllAreas, path);
            
            if (!hasPath) return false;
            
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            
            if (GatPathLength(path) > maxNavPathLength) return false;

            return true;
        }
        
        public void MoveTo(Vector3 destination,float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 Position;
            public SerializableVector3 Rotation;
        }

        public object CaptureState()
        {
            if (isPlayer) return null;

            MoverSaveData data = new MoverSaveData();

            data.Position = new SerializableVector3(gameObject.transform.position);

            data.Rotation = new SerializableVector3(gameObject.transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            if (isPlayer) return;

            MoverSaveData data = (MoverSaveData)state;
            
            _navMeshAgent.enabled = false;

            gameObject.transform.position = data.Position.ToVector();
            gameObject.transform.eulerAngles = data.Rotation.ToVector();

            _navMeshAgent.enabled = true;
        }
    }
}
