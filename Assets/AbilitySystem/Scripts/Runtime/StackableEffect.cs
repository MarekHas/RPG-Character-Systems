using UnityEngine;

namespace AbilitySystem
{
    public class StackableEffect : PersistentEffect
    {
        public new StackableEffectData EffectData => _effectData as StackableEffectData;
        public int StackCount;

        public StackableEffect(StackableEffectData effectData, object source, GameObject attacker) : base(effectData, source, attacker)
        {
            StackCount = 1;
        }
    }
}