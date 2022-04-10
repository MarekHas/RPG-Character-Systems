using CombatSystem.Scripts.Runtime.Core;
using Common.Runtime;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace CombatSystem.Scripts.Runtime
{
    [RequireComponent(typeof(Collider))]
    public class CombatController : MonoBehaviour
    {
        public Dictionary<string, RangedWeapon> RangedWeapons = new Dictionary<string, RangedWeapon>();
        
        [SerializeField] private FloatingText _floatingText;
        
        private ObjectPool<FloatingText> _pool;
        private Collider _collider;
        private IDamageable _damageable;

        private void Awake()
        {
            foreach (RangedWeapon rangedWeapon in GetComponentsInChildren<RangedWeapon>())
            {
                RangedWeapons.Add(rangedWeapon.Id, rangedWeapon);
            }

            _collider = GetComponent<Collider>();
            _damageable = GetComponent<IDamageable>();

            _pool = new ObjectPool<FloatingText>(OnCreate, OnGet, OnRelease);
        }

        private void OnEnable()
        {
            if (!_collider.enabled)
                _collider.enabled = true;

            _damageable.Initialized += OnDamageableInitialized;
            _damageable.WillUninitialize += OnDamageableWillUninitialize;

            if (_damageable.IsInitialized)
                OnDamageableInitialized();
        }

        private void OnDamageableWillUninitialize()
        {
            _damageable.Damaged -= DisplayDamage;
            _damageable.Healed -= DisplayRestorationAmount;
            _damageable.Defeated -= OnDefeated;
        }


        private void OnDamageableInitialized()
        {
            _damageable.Damaged += DisplayDamage;
            _damageable.Healed += DisplayRestorationAmount;
            _damageable.Defeated += OnDefeated;
        }

        private void OnDefeated()
        {
            _collider.enabled = false;
        }

        private void DisplayRestorationAmount(int amount)
        {
            FloatingText floatingText = _pool.Get();
            floatingText.Set(amount.ToString(), Color.green);
        }

        private void DisplayDamage(int magnitude, bool isCriticalHit)
        {
            FloatingText damageText = _pool.Get();

            damageText.Set(magnitude.ToString(), isCriticalHit ? Color.red : Color.white);
            if (isCriticalHit)
                damageText.transform.localScale *= 2;
        }

        private void OnRelease(FloatingText floatingText)
        {
            floatingText.gameObject.SetActive(false);
        }

        private void OnGet(FloatingText floatingText)
        {
            floatingText.transform.position = transform.position + Utils.GetCenterOfCollider(transform);
            floatingText.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            floatingText.gameObject.SetActive(true);
            floatingText.Animate();
        }

        private FloatingText OnCreate()
        {
            FloatingText floatingText = Instantiate(_floatingText);
            floatingText.Finished += _pool.Release;
            return floatingText;
        }
    }
}