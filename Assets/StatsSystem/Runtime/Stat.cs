using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    public class Stat
    {
        protected StatDefinition _definition;
        protected StatsController _controller;
        public StatDefinition Definition => _definition;
        protected int _value;
        public int Value => _value;
        public virtual int BaseValue => _definition.BaseValue;
        public event Action ValueChanged;
        protected List<StatModifier> _modifiers = new List<StatModifier>();

        public Stat(StatDefinition definition, StatsController controller)
        {
            _definition = definition;
            _controller = controller;
        }

        public virtual void Initialize()
        {
            CalculateValue();
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            CalculateValue();
        }

        public void RemoveModifierFromSource(object source)
        {
            int number = _modifiers.RemoveAll(modifier => modifier.DamageSource == source);
            
            if (number > 0)
            {
                CalculateValue();
            }
            
            CalculateValue();
        }

        internal void CalculateValue()
        {
            int newValue = BaseValue;

            _modifiers.Sort((x, y) => x.Type.CompareTo(y.Type));

            if (_definition.Formula != null && _definition.Formula.RootNode != null)
            {
                newValue += Mathf.RoundToInt(_definition.Formula.RootNode.Value);
            }

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