using System;
using Attributes;
using Combat;
using Core;
using Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        
        private Health _health;

     
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings;

        [SerializeField] private float maxNavMeshProjectionDistance;

        [SerializeField] private float rayCastRadius;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
                _health = GetComponent<Health>();
        }
    
        private void Update()
        { 
            if(InteractWithUi()) return;

            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            
            if(InteractWithComponent()) return;
            
            if (InteractWithMovement()) return;
            
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach (RaycastHit raycastHit in hits)
            {
                IRaycastable[] raycastables = raycastHit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false; 
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(),rayCastRadius);

            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            
            return hits;
        }


        private bool InteractWithUi()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.Ui);
                return true;
            }
            return false;   
        }

        private bool InteractWithMovement()
        {
            Vector3 target;

            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (!_mover.CanMoveTo(target)) return false;
                
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }


        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }
        
        private void SetCursor(CursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture,mapping.hotspot,CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == cursorType)
                {
                    return mapping;
                }
            }

            return cursorMappings[0];
        }

       
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
