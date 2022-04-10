using System.Text;

namespace AbilitySystem
{
    public class ActiveAbility : Ability
    {
        public new ActiveAbilityData AbilityDescription => _abilityDescription as ActiveAbilityData;
        public ActiveAbility(ActiveAbilityData abilityData, AbilityController controller) : base(abilityData, controller)
        {
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(base.ToString());

            if (AbilityDescription.CostEffectData != null)
            {
                AbilityEffect cost = new AbilityEffect(AbilityDescription.CostEffectData, this, _abilityController.gameObject);
                stringBuilder.Append(cost).AppendLine();
            }

            if (AbilityDescription.CooldownEffectData != null)
            {
                PersistentEffect cooldown =
                    new PersistentEffect(AbilityDescription.CooldownEffectData, this, _abilityController.gameObject);
                stringBuilder.Append(cooldown);
            }

            return stringBuilder.ToString();
        }
    }
}