using System;
using Control;
using UnityEngine;
using UnityEngine.UI;

namespace Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;

        private void Awake()
        {
            _health = FindObjectOfType<PlayerController>().GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}",_health.GetHealthPoints(),_health.GetMaxHealthPoints());
        }
    }
}
