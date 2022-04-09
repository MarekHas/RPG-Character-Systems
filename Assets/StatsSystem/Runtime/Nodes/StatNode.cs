using Common.Nodes;
using UnityEngine;

namespace StatsSystem.Nodes
{
    public class StatNode : FunctionNode
    {
        [SerializeField] private string _statName;
        public string StatName => _statName;
        public Stat Stat;
        public override float Value => Stat.Value;

        public override float CalculateValue(GameObject source)
        {
            StatsController statController = source.GetComponent<StatsController>();
            return statController.Stats[_statName].Value;
        }
    }
}
