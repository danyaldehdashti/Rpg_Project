using UnityEngine;
using UnityEngine.UI;

namespace UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab;

        public void Spawn(float damageAmount)
        {
            DamageText damageTextInstance = Instantiate(damageTextPrefab, transform);
            damageTextInstance.SetValue(damageAmount);
        }
    }
}
