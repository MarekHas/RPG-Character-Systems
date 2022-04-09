using Common.Runtime;
using UnityEngine;

namespace StatsSystem
{
    public class HealthModifier : StatModifier, IDamage
    {
        public bool IsCriticalHit { get; set; }
        public GameObject Attacker { get; set; }
    }
}