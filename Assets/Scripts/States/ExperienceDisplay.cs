using System;
using Control;
using UnityEngine;
using UnityEngine.UI;

namespace States
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;

        private void Awake()
        {
            _experience = FindObjectOfType<PlayerController>().GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}",_experience.GetPoint());
        }
    }
}
