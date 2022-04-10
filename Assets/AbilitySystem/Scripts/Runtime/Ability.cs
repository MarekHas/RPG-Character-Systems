using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AbilitySystem.Scripts.Runtime;
using SavingSystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class Ability : ISavable
    {
        protected AbilitySystem.AbilityData _abilityDescription;
        protected AbilityController _abilityController;
        private int _actualLevel = 5;
        
        public event Action LevelChanged;
        
        public AbilitySystem.AbilityData AbilityDescription => _abilityDescription;

        public int Level
        {
            get => _actualLevel;
            
            internal set
            {
                int nextLevel = Mathf.Min(value, AbilityDescription.MaximumLevel);
                
                if (nextLevel != _actualLevel)
                {
                    _actualLevel = nextLevel;
                    LevelChanged?.Invoke();
                }
            }
        }

        public Ability(AbilitySystem.AbilityData abilityData, AbilityController controller)
        {
            _abilityDescription = abilityData;
            _abilityController = controller;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (EffectData effectDefinition in AbilityDescription.EffectDefinitions)
            {
                EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>().FirstOrDefault();
                
                AbilityEffect effect =
                    Activator.CreateInstance(attribute.Type, effectDefinition, this, _abilityController.gameObject) as
                        AbilityEffect;
                
                stringBuilder.Append(effect).AppendLine();
            }

            return stringBuilder.ToString();
        }

        internal void ApplyEffects(GameObject other)
        {
            ApplyEffectsInternal(_abilityDescription.EffectDefinitions, other);
        }

        private void ApplyEffectsInternal(ReadOnlyCollection<EffectData> effectDefinitions, GameObject other)
        {
            if (other.TryGetComponent(out GameplayEffectController effectController))
            {
                foreach (EffectData effectDefinition in effectDefinitions)
                {
                    EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                        .OfType<EffectTypeAttribute>().FirstOrDefault();
                    
                    AbilityEffect effect =
                        Activator.CreateInstance(attribute.Type, effectDefinition, this, _abilityController.gameObject) as
                            AbilityEffect;
                    
                    effectController.ApplyGameplayEffectToSelf(effect);
                }
            }
        }

        #region Save System

        public object Data => new AbilityData
        {
            Level = _actualLevel
        };

        public void Load(object data)
        {
            AbilityData abilityData = (AbilityData)data;
            _actualLevel = abilityData.Level;
            LevelChanged?.Invoke();
        }

        [Serializable]
        protected class AbilityData
        {
            public int Level;
        }

        #endregion

    }
}