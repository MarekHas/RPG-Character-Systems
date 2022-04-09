using System;
using System.Runtime.CompilerServices;
using SavingSystem.Scripts.Runtime;

[assembly: InternalsVisibleTo("StatSystem.Tests")]
namespace StatsSystem
{
    
    public class PrimaryStat : Stat, ISavable
    {
        private int _baseValue;
        public override int BaseValue => _baseValue;

        public PrimaryStat(StatDefinition definition, StatsController controller) : base(definition, controller)
        {
        }

        public override void Initialize()
        {
            _baseValue = Definition.BaseValue;
            base.Initialize();
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

        #region Stat System

        public object Data => new PrimaryStatData
        {
            baseValue = BaseValue
        };

        public void Load(object data)
        {
            PrimaryStatData primaryStatData = (PrimaryStatData)data;
            _baseValue = primaryStatData.baseValue;
            CalculateValue();
        }

        [Serializable]
        protected class PrimaryStatData
        {
            public int baseValue;
        }

        #endregion
    }
}
