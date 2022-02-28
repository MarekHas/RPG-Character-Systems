using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    [CreateAssetMenu(fileName = "StatsDatabase", menuName = "StatsSystem/StatsDatabase", order = 0)]
    public class StatDatabase : ScriptableObject
    {
        public List<StatDefinition> Stats;
        public List<StatDefinition> Attributes;
        public List<StatDefinition> PrimaryStats;
    }
}
