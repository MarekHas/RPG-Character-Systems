using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Common.Runtime;

namespace AbilitySystem.Scripts.Runtime
{
    [EffectType(typeof(AbilityEffect))]
    [CreateAssetMenu(fileName = "EffectData", menuName = "AbilitySystem/Effects/EffectData", order = 0)]
    public class EffectData : ScriptableObject
    {
        [SerializeField] protected List<AbstractEffectStatModifierData> _modifierDefinitions;
        [SerializeField] private List<EffectData> _conditionalEffects;
        [SerializeField] private string _description;
        [SerializeField] private SpecialEffectData _specialEffectDefinition;
        [SerializeField] private List<string> _tags;
        [SerializeField] private List<string> _removeEffectsWithTags;
        [SerializeField] private List<string> _requiredTags;
        [SerializeField] private List<string> _forbiddenTags;

        public ReadOnlyCollection<AbstractEffectStatModifierData> ModifierDefinitions =>
            _modifierDefinitions.AsReadOnly();
        public ReadOnlyCollection<EffectData> ConditionalEffects => _conditionalEffects.AsReadOnly();
        public string Description => _description;
        public SpecialEffectData SpecialEffectDefinition => _specialEffectDefinition;
        public ReadOnlyCollection<string> Tags => _tags.AsReadOnly();
        public ReadOnlyCollection<string> RemoveEffectsWithTags => _removeEffectsWithTags.AsReadOnly();
        public ReadOnlyCollection<string> RequiredTags => _requiredTags.AsReadOnly();
        public ReadOnlyCollection<string> ForbiddenTags => _forbiddenTags.AsReadOnly();

    }
}