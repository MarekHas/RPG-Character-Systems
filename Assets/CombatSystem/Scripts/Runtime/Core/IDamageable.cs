using System;
using Common.Runtime;

namespace CombatSystem.Scripts.Runtime.Core
{
    public interface IDamageable
    {
        int Health { get; }
        int maxHealth { get; }
        event Action HealthChanged;
        event Action MaxHealthChanged;
        bool IsInitialized { get; }
        event Action Initialized;
        event Action WillUninitialize;
        event Action Defeated;
        event Action<int> Healed;
        event Action<int, bool> Damaged;
        void TakeDamage(IDamage rawDamage);
    }
}