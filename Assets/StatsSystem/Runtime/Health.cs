using Common.Runtime;

namespace StatsSystem
{
    public class Health : Attribute
    {
        public Health(StatDefinition definition, StatsController controller) : base(definition, controller)
        {
        }

        public override void ApplyModifier(StatModifier modifier)
        {
            ITaggable source = modifier.DamageSource as ITaggable;

            if (source != null)
            {
                if (source.Tags.Contains("physical"))
                {
                    modifier.Magnitude += _controller.Stats["PhysicalDefense"].Value;
                }
                else if (source.Tags.Contains("magical"))
                {
                    modifier.Magnitude += _controller.Stats["MagicalDefense"].Value;
                }
                else if (source.Tags.Contains("pure"))
                {
                    // do nothing
                }
            }

            base.ApplyModifier(modifier);
        }
    }
}