using UnityEngine;

namespace AbilitySystem
{
    public class PersistentEffect : AbilityEffect
    {
        public new PersistentEffectData EffectData => _effectData as PersistentEffectData;
        public float RemainingDuration;
        public float RemainingPeriod;
        public bool IsInhibited;

        private float _duration;
        public float Duration => _duration;

        public PersistentEffect(PersistentEffectData definition, object source, GameObject attacker) : base(definition, source, attacker)
        {
            IsInhibited = true;
            RemainingPeriod = definition.Period;

            if (definition.IsInfinite == false)
            {
                RemainingDuration = _duration = definition.DurationFormula.CalculateValue(attacker);
            }
        }
    }
}