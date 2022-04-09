using System;
using UnityEngine;
using SavingSystem.Scripts.Runtime;

namespace StatsSystem
{
    public class Attribute : Stat, ISavable
    {
        protected int _currentValue;
        public int CurrentValue => _currentValue;
        public event Action CurrentValueChanged;
        public event Action<StatModifier> AppliedModifier;

        public Attribute(StatDefinition definition, StatsController controller) : base(definition, controller)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _currentValue = Value;
        }

        public virtual void ApplyModifier(StatModifier modifier)
        {
            int newValue = _currentValue;
            
            switch (modifier.Type)
            {
                case ModifierOperationType.Override:
                    newValue = modifier.Magnitude;
                    break;
                case ModifierOperationType.Additive:
                    newValue += modifier.Magnitude;
                    break;
                case ModifierOperationType.Multiplier:
                    newValue *= modifier.Magnitude;
                    break;
            }

            newValue = Mathf.Clamp(newValue, 0, _value);

            if (CurrentValue != newValue)
            {
                _currentValue = newValue;
                CurrentValueChanged?.Invoke();
            }
            AppliedModifier?.Invoke(modifier);
        }

        #region Save System

        public object Data => new AttributeData
        {
            currentValue = CurrentValue
        };
        public void Load(object data)
        {
            AttributeData attributeData = (AttributeData)data;
            _currentValue = attributeData.currentValue;
            CurrentValueChanged?.Invoke();
        }

        [Serializable]
        protected class AttributeData
        {
            public int currentValue;
        }

        #endregion
    }
}
