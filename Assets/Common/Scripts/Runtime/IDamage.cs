using UnityEngine;

namespace Common.Runtime
{
    public interface IDamage
    {
        bool IsCriticalHit { get; }
        int Magnitude { get; }
        GameObject Attacker { get; }
        object DamageSource { get; }
    }
}