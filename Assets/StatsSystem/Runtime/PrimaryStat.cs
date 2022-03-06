using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StatSystem.Tests")]
namespace StatsSystem
{
    
    public class PrimaryStat : Stat
    {
        private int _baseValue;
        public override int BaseValue => _baseValue;

        public PrimaryStat(StatDefinition definition) : base(definition)
        {
            _baseValue = definition.BaseValue;
            CalculateValue();
        }

        internal void Add(int amount)
        {
            _baseValue += amount;
            CalculateValue();
        }

        internal void Subtract(int amount)
        {
            _baseValue -= amount;
            CalculateValue();
        }
    }
}
