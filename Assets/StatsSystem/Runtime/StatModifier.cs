namespace StatsSystem
{
    public class StatModifier
    {
        public object DamageSource { get; set; }
        public int Magnitude { get; set; }
        public ModifierOperationType Type { get; set; }

        public override string ToString()
        {
            return Magnitude.ToString();
        }
    }
}