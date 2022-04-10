using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Common.Runtime;
using SavingSystem.Scripts.Runtime;

namespace AbilitySystem
{
    [RequireComponent(typeof(GameplayEffectController))]
    [RequireComponent(typeof(TagController))]
    public class AbilityController : MonoBehaviour, ISavable
    {
        [SerializeField] private List<AbilityData> _abilityDefinitions;
        
        private TagController _tagController;
        private GameplayEffectController _effectController;
        protected Dictionary<string, Ability> _abilities = new Dictionary<string, Ability>();
        
        public Dictionary<string, Ability> Abilities => _abilities;
        public ActiveAbility CurrentAbility;
        public GameObject Target;

        public event Action<ActiveAbility> ActivatedAbility;

        protected virtual void Awake()
        {
            _effectController = GetComponent<GameplayEffectController>();
            _tagController = GetComponent<TagController>();
        }

        protected virtual void OnEnable()
        {
            _effectController.Initialized += OnEffectControllerInitialized;

            if (_effectController.IsInitialized)
            {
                OnEffectControllerInitialized();
            }
        }

        protected virtual void OnDisable()
        {
            _effectController.Initialized -= OnEffectControllerInitialized;
        }

        private void OnEffectControllerInitialized()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            foreach (AbilityData abilityDefinition in _abilityDefinitions)
            {
                AbilityTypeAttribute abilityAttributeType = abilityDefinition.GetType().GetCustomAttributes(true)
                    .OfType<AbilityTypeAttribute>().FirstOrDefault();

                Ability ability = Activator.CreateInstance(abilityAttributeType.Type, abilityDefinition, this) as Ability;
                _abilities.Add(abilityDefinition.name, ability);
                if (ability is PassiveAbility passiveAbility)
                {
                    passiveAbility.ApplyEffects(gameObject);
                }
            }
        }

        public bool TryActivateAbility(string abilityName, GameObject target)
        {
            if (_abilities.TryGetValue(abilityName, out Ability ability))
            {
                if (ability is ActiveAbility activeAbility)
                {
                    if (!CanActivateAbility(activeAbility))
                        return false;
                    Target = target;
                    CurrentAbility = activeAbility;
                    CommitAbility(activeAbility);
                    ActivatedAbility?.Invoke(activeAbility);

                    return true;
                }
            }
            Debug.Log($"Ability with name {abilityName} not found!");
            return false;
        }

        public bool CanActivateAbility(ActiveAbility ability)
        {
            if (ability.AbilityDescription.CooldownEffectData != null)
            {
                if (_tagController.ContainsAny(ability.AbilityDescription.CooldownEffectData.GrantedTags))
                {
                    Debug.Log($"{ability.AbilityDescription.name} is on cooldown!");
                    return false;
                }
            }

            if (ability.AbilityDescription.CostEffectData != null)
                return _effectController.CanApplyAttributeModifiers(ability.AbilityDescription.CostEffectData);
            return true;
        }
        private void CommitAbility(ActiveAbility ability)
        {
            _effectController.ApplyGameplayEffectToSelf(new AbilityEffect(ability.AbilityDescription.CostEffectData, ability, gameObject));
            _effectController.ApplyGameplayEffectToSelf(new PersistentEffect(ability.AbilityDescription.CooldownEffectData, ability, gameObject));
        }

        #region Save System

        public virtual object Data
        {
            get
            {
                Dictionary<string, object> abilities = new Dictionary<string, object>();
                foreach (Ability ability in _abilities.Values)
                {
                    if (ability is ISavable savable)
                    {
                        abilities.Add(ability.AbilityDescription.name, savable.Data);
                    }
                }

                return new AbilityControllerData
                {
                    Abilities = abilities
                };
            }
        }
        public virtual void Load(object data)
        {
            AbilityControllerData abilityControllerData = (AbilityControllerData)data;
            foreach (Ability ability in _abilities.Values)
            {
                if (ability is ISavable savable)
                {
                    savable.Load(abilityControllerData.Abilities[ability.AbilityDescription.name]);
                }
            }
        }

        [Serializable]
        protected class AbilityControllerData
        {
            public Dictionary<string, object> Abilities;
        }

        #endregion
    }
}