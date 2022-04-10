using System;
using CombatSystem.Scripts.Runtime.Core;
using Common.Runtime;
using StatsSystem;
using UnityEngine;
using Attribute = StatsSystem.Attribute;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
    [RequireComponent(typeof(StatsController))]
    public class CombatableCharacter : MonoBehaviour, IDamageable
    {
        private const string _health = "Health";
        private bool _isInitialized;
        public int Health => (_statController.Stats[_health] as Attribute).CurrentValue;
        public int maxHealth => _statController.Stats[_health].Value;
        public event Action HealthChanged;
        public event Action MaxHealthChanged;
        public bool IsInitialized => _isInitialized;
        public event Action Initialized;
        public event Action WillUninitialize;
        public event Action Defeated;
        public event Action<int> Healed;
        public event Action<int, bool> Damaged;

        protected StatsController _statController;
        protected virtual void Awake()
        {
            _statController = GetComponent<StatsController>();
        }

        protected virtual void OnEnable()
        {
            _statController.Initialized += OnStatControllerInitialized;
            _statController.WillUninitialize += OnStatControllerWillUninitialize;
            
            if (_statController.IsInitialized)
            {
                OnStatControllerInitialized();
            }
        }

        protected virtual void OnDisable()
        {
            _statController.Initialized -= OnStatControllerInitialized;
            _statController.WillUninitialize -= OnStatControllerWillUninitialize;
            
            if (_statController.IsInitialized)
            {
                OnStatControllerWillUninitialize();
            }
        }

        private void OnStatControllerWillUninitialize()
        {
            WillUninitialize?.Invoke();

            _statController.Stats[_health].ValueChanged -= OnMaxHealthChanged;
            (_statController.Stats[_health] as Attribute).CurrentValueChanged -= OnHealthChanged;
            (_statController.Stats[_health] as Attribute).AppliedModifier -= OnAppliedModifier;
        }

        private void OnStatControllerInitialized()
        {
            _statController.Stats[_health].ValueChanged += OnMaxHealthChanged;
            (_statController.Stats[_health] as Attribute).CurrentValueChanged += OnHealthChanged;
            (_statController.Stats[_health] as Attribute).AppliedModifier += OnAppliedModifier;

            _isInitialized = true;
            Initialized?.Invoke();
        }

        private void OnAppliedModifier(StatModifier modifier)
        {
            if (modifier.Magnitude > 0)
            {
                Healed?.Invoke(modifier.Magnitude);
            }
            else
            {
                if (modifier is HealthModifier healthModifier)
                {
                    Damaged?.Invoke(modifier.Magnitude, healthModifier.IsCriticalHit);
                }
                else
                {
                    Damaged?.Invoke(modifier.Magnitude, false);
                }

                if ((_statController.Stats[_health] as Attribute).CurrentValue == 0)
                    Defeated?.Invoke();
            }
        }

        private void OnHealthChanged()
        {
            HealthChanged?.Invoke();
        }

        private void OnMaxHealthChanged()
        {
            MaxHealthChanged?.Invoke();
        }

        public void TakeDamage(IDamage rawDamage)
        {
            (_statController.Stats[_health] as Attribute).ApplyModifier(new HealthModifier
            {
                Magnitude = rawDamage.Magnitude,
                Type = ModifierOperationType.Additive,
                DamageSource = rawDamage.DamageSource,
                IsCriticalHit = rawDamage.IsCriticalHit,
                Attacker = rawDamage.Attacker
            });
        }

        public void ApplyDamage(Object source, GameObject target)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            HealthModifier rawDamage = new HealthModifier
            {
                Attacker = gameObject,
                Type = ModifierOperationType.Additive,
                Magnitude = -1 * _statController.Stats["PhysicalAttack"].Value,
                DamageSource = source,
                IsCriticalHit = false
            };

            if (_statController.Stats["CriticalHitChance"].Value / 100f >= Random.value)
            {
                rawDamage.Magnitude =
                    Mathf.RoundToInt(rawDamage.Magnitude * _statController.Stats["CriticalHitMultiplier"].Value /
                                     100f);
                rawDamage.IsCriticalHit = true;
            }

            damageable.TakeDamage(rawDamage);
        }
    }
}