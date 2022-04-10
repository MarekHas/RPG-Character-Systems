using StatsSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public class DamageEffectData : AbstractEffectStatModifierData
    {
        public override string StatName => "Health";

        [SerializeField] private bool _canCriticalHit;
        
        public bool CanCriticalHit => _canCriticalHit;
        public override ModifierOperationType ModifierType => ModifierOperationType.Additive;
    }
}