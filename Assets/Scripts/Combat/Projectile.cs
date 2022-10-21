using System;
using Attributes;
using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;

        [SerializeField] private bool isHoming;

        [SerializeField] private GameObject hitEffect;

        [SerializeField] private float maxLifeTime;

        [SerializeField] private float lifeAfterImpact;  

        [SerializeField] private GameObject [] destroyOnHit;

        [SerializeField] private UnityEvent onHit;
        
        private Health _target;

        private GameObject _instigator;

        private float _damage;


        private void Start()
        {
            gameObject.transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if(_target == null) return;

            if (isHoming && !_target.IsDead())
            {
                gameObject.transform.LookAt(GetAimLocation());
            }
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsuleCollider = _target.GetComponent<CapsuleCollider>();

            if (capsuleCollider == null)
            {
                return _target.transform.position;
            }
            
            return _target.transform.position + Vector3.up * capsuleCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;
            
            _target.TakeDamage(_instigator,_damage);
            
            if(_target.IsDead()) return;

            speed = 0;
            
            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), gameObject.transform.rotation);  
            }

            foreach (GameObject obGameObject in destroyOnHit)
            {
                Destroy(obGameObject);
            }
            
            Destroy(gameObject,lifeAfterImpact);
        }

        public void SetTarget(Health target,GameObject instigator,float damage)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;
            Destroy(gameObject,maxLifeTime);
        }
    }
}
