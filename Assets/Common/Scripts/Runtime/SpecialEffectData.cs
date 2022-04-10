using UnityEngine;

namespace Common.Runtime
{
    [CreateAssetMenu(fileName = "SpecialEffectData", menuName = "Common/SpecialEffectData", order = 0)]
    public class SpecialEffectData : ScriptableObject
    {
        [SerializeField] private EffectPosition _effectPosition;
        [SerializeField] private VisualEffect _prefab;
        public VisualEffect Prefab => _prefab;
        public EffectPosition EffectPosition => _effectPosition;

    }
}