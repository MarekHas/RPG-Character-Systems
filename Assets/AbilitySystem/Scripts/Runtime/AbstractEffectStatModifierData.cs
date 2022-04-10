using Common.Runtime;
using StatsSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public abstract class AbstractEffectStatModifierData : ScriptableObject
    {
        public abstract string StatName { get; }
        public abstract ModifierOperationType ModifierType { get; }
        [SerializeField] private NodeGraph _formula;
        public NodeGraph Formula => _formula;
    }
}