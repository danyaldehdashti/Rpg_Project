using System;
using UnityEngine;

namespace Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float _wayPointGizmozRaduis = 0.3f;
        
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i),_wayPointGizmozRaduis);
                Gizmos.DrawLine(GetWayPoint(i),GetWayPoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == gameObject.transform.childCount)
            {
                return 0;
            }
            
            return i + 1;
        }

        public Vector3 GetWayPoint(int i)
        {
            return gameObject.transform.GetChild(i).position;
        } 
    }
}
