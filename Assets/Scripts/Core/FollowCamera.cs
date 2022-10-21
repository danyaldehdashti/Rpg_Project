using UnityEngine;

namespace Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;


        private void LateUpdate()
        {
            gameObject.transform.position = target.position;
        }
    }
}
