using UnityEngine;

namespace StatsSystem
{
    [CreateAssetMenu(fileName = "StatsDefinition", menuName = "StatsSystem/StatDefinition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] private int _baseValue;
        [SerializeField] private int _maxValue = -1;

        public int BaseValue => _baseValue;
        public int MaxValue => _maxValue;
    }
}