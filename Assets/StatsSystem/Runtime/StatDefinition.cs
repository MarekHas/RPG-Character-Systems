using UnityEngine;

namespace StatsSystem
{
    [CreateAssetMenu(fileName = "StatDefinition", menuName = "StatsSystem/StatDefinition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] private int _baseValue;
        [SerializeField] private int _cap = -1;
        public int BaseValue => _baseValue;
        public int Cap => _cap;

    }
}