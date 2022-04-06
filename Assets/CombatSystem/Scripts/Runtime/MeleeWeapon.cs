using System;
using CombatSystem.Scripts.Runtime.Core;
using UnityEngine;

namespace CombatSystem.Scripts.Runtime
{
    public class MeleeWeapon : MonoBehaviour
    {
        public event Action<CollisionData> HitOnTarget;

        private void OnTriggerEnter(Collider other)
        {
            HitOnTarget?.Invoke(new CollisionData
            {
                Target = other.gameObject,
                Source = this
            });
        }
    }
}