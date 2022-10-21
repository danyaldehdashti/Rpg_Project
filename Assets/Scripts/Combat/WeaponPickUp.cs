using System;
using System.Collections;
using Attributes;
using Control;
using UnityEngine;
using UnityEngine.UIElements;

namespace Combat
{
    public class WeaponPickUp : MonoBehaviour,IRaycastable
    {
        [SerializeField] private WeaponConfig weaponConfig;

        [SerializeField] private float healthToRestore;

        [SerializeField] private float respawnTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>())
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject  subject)
        {
            if (weaponConfig != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weaponConfig);
                StartCoroutine(HideForSeconds(respawnTime));
            }

            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(seconds);
            ShowPickUp(true);
        }

        private void ShowPickUp(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            gameObject.transform.GetChild(0).gameObject.SetActive(shouldShow);

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }

        public bool HandleRaycast(PlayerController collingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(collingController.gameObject);
            }

            return true;
        }
    }
}
