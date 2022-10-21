using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text damageText;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float amount)
        {
            damageText.text = String.Format("{0:0}", amount);
        }
    }
}
