using AbilitySystem.Scripts.Runtime;
using Common.Runtime;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AbilitySystem
{

    [EffectType(typeof(PersistentEffect))]
    [CreateAssetMenu(fileName = "PersistentEffectData", menuName = "AbilitySystem/Effects/PersistentEffectData", order = 0)]
    public class PersistentEffectData : EffectData
    {
        [SerializeField] protected bool _isInfinite;
        [SerializeField] protected NodeGraph _durationFormula;
        [SerializeField] protected bool _isPeriodic;
        [SerializeField] protected float _period;
        [SerializeField] private bool _executePeriodicEffectOnApplication;
        [SerializeField] protected List<string> _grantedTags;
        [SerializeField] private SpecialEffectData _specialPersistentEffectDefinition;
        [SerializeField] private List<string> _grantedApplicationImmunityTags;
        [SerializeField] private List<string> _uninhibitedMustBePresentTags;
        [SerializeField] private List<string> _uninhibitedMustBeAbsentTags;
        [SerializeField] private GameplayEffectPeriodInhibitionRemovedPolicy _periodicInhibitionPolicy;
        [SerializeField] private List<string> _persistMustBePresentTags;
        [SerializeField] private List<string> _persistMustBeAbsentTags;

        public bool IsInfinite => _isInfinite;
        public NodeGraph DurationFormula => _durationFormula;
        public bool IsPeriodic => _isPeriodic;
        public float Period => _period;
        public bool ExecutePeriodicEffectOnApplication => _executePeriodicEffectOnApplication;
        public ReadOnlyCollection<string> GrantedTags => _grantedTags.AsReadOnly();
        public SpecialEffectData SpecialPersistentEffectDefinition => _specialPersistentEffectDefinition;
        public ReadOnlyCollection<string> GrantedApplicationImmunityTags => _grantedApplicationImmunityTags.AsReadOnly();
        public ReadOnlyCollection<string> UninhibitedMustBePresentTags => _uninhibitedMustBePresentTags.AsReadOnly();
        public ReadOnlyCollection<string> UninhibitedMustBeAbsentTags => _uninhibitedMustBeAbsentTags.AsReadOnly();
        public GameplayEffectPeriodInhibitionRemovedPolicy PeriodicInhibitionPolicy => _periodicInhibitionPolicy;
        public ReadOnlyCollection<string> PersistMustBePresentTags => _persistMustBePresentTags.AsReadOnly();
        public ReadOnlyCollection<string> PersistMustBeAbsentTags => _persistMustBeAbsentTags.AsReadOnly();
    }
}