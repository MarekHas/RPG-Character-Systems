using StatsSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public class EffectStatModifierData : AbstractEffectStatModifierData
    {
        [SerializeField] private string _statName;
        [SerializeField] private ModifierOperationType _type;

        public override string StatName => _statName;
        public override ModifierOperationType ModifierType => _type;
    }
}