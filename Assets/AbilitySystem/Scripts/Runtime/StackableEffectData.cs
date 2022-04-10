using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    [EffectType(typeof(StackableEffect))]
    [CreateAssetMenu(fileName = "StackableEffectData", menuName = "AbilitySystem/Effects/StackableEffectData", order = 0)]
    public class StackableEffectData : PersistentEffectData
    {
        [SerializeField] private List<EffectData> _overflowEffects;
        [SerializeField] private bool _denyOverflowApplication;
        [SerializeField] private bool _clearStackOnOverflow;
        [SerializeField] private int _stackLimitCount = 3;
        [SerializeField] private GameplayEffectStackingDurationPolicy _stackDurationRefreshPolicy;
        [SerializeField] private GameplayEffectStackingPeriodPolicy _stackPeriodResetPolicy;
        [SerializeField] private GameplayEffectStackingExpirationPolicy _stackExpirationPolicy;
        
        public ReadOnlyCollection<EffectData> OverflowEffects => _overflowEffects.AsReadOnly();
        public bool DenyOverflowApplication => _denyOverflowApplication;
        public bool ClearStackOnOverflow => _clearStackOnOverflow;
        public int StackLimitCount => _stackLimitCount;
        public GameplayEffectStackingDurationPolicy StackDurationRefreshPolicy => _stackDurationRefreshPolicy;
        public GameplayEffectStackingPeriodPolicy StackPeriodResetPolicy => _stackPeriodResetPolicy;
        public GameplayEffectStackingExpirationPolicy StackExpirationPolicy => _stackExpirationPolicy;
    }
}