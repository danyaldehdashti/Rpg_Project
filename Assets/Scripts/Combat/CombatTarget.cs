using Attributes;
using Control;
using Core;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour,IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController collingController)
        {
            if (!collingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return  false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                collingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
