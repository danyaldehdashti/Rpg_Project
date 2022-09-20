using Core;
using UnityEngine;
using UnityEngine.AI;
namespace Movement
{
    public class Mover : MonoBehaviour,IAction
    {
        private NavMeshAgent _player;
        
        private Animator _animator;
        
        private ActionScheduler _actionScheduler;
        
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _player = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            UpdateAnimator();
        }
        
        private void UpdateAnimator()
        {
            Vector3 velocity = _player.velocity;
            Vector3 localVelocity = gameObject.transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            _animator.SetFloat(Speed,speed);
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 destination)
        {
            _player.destination = destination;
            _player.isStopped = false;
        }

        public void Cancel()
        {
            _player.isStopped = true;
        }
    }
}
