using UnityEngine;

namespace AbilitySystem
{
    public class SingleTargetAbility : ActiveAbility
    {
        public SingleTargetAbility(SingleTargetAbilityData abilityData, AbilityController controller) : base(abilityData, controller)
        {
        }

        public void Cast(GameObject target)
        {
            ApplyEffects(target);
        }
    }
}