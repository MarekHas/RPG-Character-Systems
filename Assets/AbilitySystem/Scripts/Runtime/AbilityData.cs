using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class AbilityData : ScriptableObject
    {
        [SerializeField] private List<EffectData> _effectDefinitions;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _maximumLevel = 20;
        [SerializeField] private string _description;

        public ReadOnlyCollection<EffectData> EffectDefinitions => _effectDefinitions.AsReadOnly();
        public Sprite Icon => _icon;
        public int MaximumLevel => _maximumLevel;
        public string Description => _description;
    }
}