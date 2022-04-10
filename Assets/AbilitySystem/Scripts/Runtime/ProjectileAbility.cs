using CombatSystem.Scripts.Runtime;
using CombatSystem.Scripts.Runtime.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace AbilitySystem
{
    public class ProjectileAbility : ActiveAbility
    {
        public new ProjectileAbilityData AbilityDescription => _abilityDescription as ProjectileAbilityData;
        protected CombatController _combatController;
        private ObjectPool<Projectile> _pool;

        public ProjectileAbility(ProjectileAbilityData definition, AbilityController controller) : base(definition, controller)
        {
            _pool = new ObjectPool<Projectile>(OnCreate, OnGet, OnRelease);
            _combatController = controller.GetComponent<CombatController>();
        }

        private void OnRelease(Projectile projectile)
        {
            projectile.Rigidbody.velocity = Vector3.zero;
            projectile.gameObject.SetActive(false);
        }

        private void OnGet(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private Projectile OnCreate()
        {
            Projectile projectile = GameObject.Instantiate(AbilityDescription.ProjectilePrefab);
            projectile.Hit += OnHit;
            return projectile;
        }

        private void OnHit(CollisionData data)
        {
            OnRelease(data.Source as Projectile);
            ApplyEffects(data.Target);
        }

        public void Shoot(GameObject target)
        {
            if (_combatController.RangedWeapons.TryGetValue(AbilityDescription.WeaponId, out RangedWeapon rangedWeapon))
            {
                Projectile projectile = _pool.Get();
                rangedWeapon.Shoot(
                    projectile,
                    target.transform,
                    AbilityDescription.ProjectileSpeed,
                    AbilityDescription.ShotType,
                    AbilityDescription.IsSpinning);
            }
            else
            {
                Debug.LogWarning($"Could not find weapon {AbilityDescription.WeaponId}");
            }
        }
    }
}