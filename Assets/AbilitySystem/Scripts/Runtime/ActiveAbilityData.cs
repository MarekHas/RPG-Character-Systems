using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class ActiveAbilityData: AbilityData
    {
        [SerializeField] protected string _name;
        [SerializeField] protected EffectData _costDataEffect;
        [SerializeField] private PersistentEffectData _cooldownEffectData;
        
        public string AnimationName => _name;
        public EffectData CostEffectData => _costDataEffect;
        public PersistentEffectData CooldownEffectData => _cooldownEffectData;
    }
}