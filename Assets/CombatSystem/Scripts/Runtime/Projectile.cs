using System;
using CombatSystem.Scripts.Runtime.Core;
using Common.Runtime;
using UnityEngine;

namespace CombatSystem.Scripts.Runtime
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public event Action<CollisionData> Hit;
        public Rigidbody Rigidbody => _rigidbody;
        protected Rigidbody _rigidbody;
        protected Collider _collider;
        [SerializeField] private VisualEffect _collisionEffect;

        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            HandleCollision(other.gameObject);
        }

        protected void HandleCollision(GameObject other)
        {
            if (_collisionEffect != null)
            {
                VisualEffect collisionVisualEffect = Instantiate(_collisionEffect, transform.position, transform.rotation);
                
                collisionVisualEffect.finished += effect => Destroy(effect.gameObject);
                collisionVisualEffect.Play();
            }
            
            Hit?.Invoke(new CollisionData{ Source = this, Target = other});
        }
    }
}