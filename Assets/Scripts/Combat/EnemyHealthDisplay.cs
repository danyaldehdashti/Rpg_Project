using System;
using Attributes;
using Control;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = FindObjectOfType<PlayerController>().GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            Health health = _fighter.GetTarget();
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}",health.GetHealthPoints(),health.GetMaxHealthPoints());
            
            
        }
    }
}
