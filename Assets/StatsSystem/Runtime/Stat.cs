using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StatsSystem
{
    public class Stat
    {
        protected StatDefinition _definition;
        protected int _value;
        public int Value => _value;
        public virtual int BaseValue => _definition.BaseValue;
        public event Action ValueChanged;
        protected List<StatModifier> _modifiers = new List<StatModifier>();

        public Stat(StatDefinition definition)
        {
            _definition = definition;
            CalculateValue();
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            CalculateValue();
        }

        public void RemoveModifierFromSource(Object source)
        {
            _modifiers = _modifiers.Where(m => m.Source.GetInstanceID() != source.GetInstanceID()).ToList();
            CalculateValue();
        }

        protected void CalculateValue()
        {
            int newValue = BaseValue;

            _modifiers.Sort((x, y) => x.Type.CompareTo(y.Type));

            for (int i = 0; i < _modifiers.Count; i++)
            {
                StatModifier modifier = _modifiers[i];
                if (modifier.Type == ModifierOperationType.Additive)
                {
                    newValue += modifier.Magnitude;
                }
                else if (modifier.Type == ModifierOperationType.Multiplier)
                {
                    newValue *= modifier.Magnitude;
                }
            }

            if (_definition.Cap >= 0)
            {
                newValue = Mathf.Min(newValue, _definition.Cap);
            }

            if (_value != newValue)
            {
                _value = newValue;
                ValueChanged?.Invoke();
            }
        }
    }
}