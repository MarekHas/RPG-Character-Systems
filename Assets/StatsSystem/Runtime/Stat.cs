using System;

namespace StatsSystem
{
    public class Stat
    {
        protected StatDefinition _definition;
        protected int _value;
        public int Value => _value;
        public virtual int BaseValue => _definition.baseValue;
        public event Action ValueChanged;

        public Stat(StatDefinition definition)
        {
            _definition = definition;
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            CalculateValue();
        }

        public void RemoveModifierFromSource(Object source)
        {
            _modifiers = _modifiers.Where(m => m.source.GetInstanceID() != source.GetInstanceID()).ToList();
            CalculateValue();
        }

        internal void CalculateValue()
        {
            int newValue = BaseValue;

            if (_definition.formula != null && _definition.formula.rootNode != null)
            {
                newValue += Mathf.RoundToInt(_definition.formula.rootNode.value);
            }

            _modifiers.Sort((x, y) => x.type.CompareTo(y.type));

            for (int i = 0; i < _modifiers.Count; i++)
            {
                StatModifier modifier = _modifiers[i];
                if (modifier.Type == ModifierOperationType.Additive)
                {
                    newValue += modifier.magnitude;
                }
                else if (modifier.Type == ModifierOperationType.Multiplier)
                {
                    newValue *= modifier.Magnitude;
                }
            }

            if (_definition.MaxValue >= 0)
            {
                newValue = Mathf.Min(newValue, _definition.MaxValue);
            }

            if (_value != newValue)
            {
                _value = newValue;
                ValueChanged?.Invoke();
            }
        }
    }
}