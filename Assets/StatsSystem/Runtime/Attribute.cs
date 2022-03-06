using System;
using UnityEngine;

namespace StatsSystem
{
    public class Attribute : Stat
    {
        protected int _currentValue;
        public int CurrentValue => _currentValue;
        public event Action CurrentValueChanged;
        public event Action<StatModifier> AppliedModifier;

        public Attribute(StatDefinition definition) : base(definition)
        {
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
                AppliedModifier?.Invoke(modifier);
            }
        }
    }
}
