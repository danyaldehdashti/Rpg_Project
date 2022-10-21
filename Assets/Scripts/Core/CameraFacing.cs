using UnityEngine;

namespace Core
{
    public class CameraFacing : MonoBehaviour
    {
        void LateUpdate()
        {
            gameObject.transform.forward = Camera.main.transform.forward;   
        }
    }
}
