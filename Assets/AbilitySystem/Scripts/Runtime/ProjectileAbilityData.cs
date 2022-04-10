using CombatSystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    [AbilityType(typeof(ProjectileAbility))]
    [CreateAssetMenu(fileName = "ProjectileAbilityData", menuName = "AbilitySystem/Ability/ProjectileAbilityData", order = 0)]
    public class ProjectileAbilityData : ActiveAbilityData
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ShotType _shotType;
        [SerializeField] private string _weaponId;
        [SerializeField] private float _projectileSpeed = 10f;
        [SerializeField] private bool _isSpinning;

        public Projectile ProjectilePrefab => _projectilePrefab;
        public ShotType ShotType => _shotType;
        public string WeaponId => _weaponId;
        public float ProjectileSpeed => _projectileSpeed;
        public bool IsSpinning => _isSpinning;
    }
}