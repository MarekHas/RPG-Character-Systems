using UnityEngine;
using Common.Runtime;

namespace StatsSystem
{
    [CreateAssetMenu(fileName = "StatDefinition", menuName = "StatsSystem/StatDefinition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] private int _baseValue;
        [SerializeField] private int _cap = -1;
        [SerializeField] private NodeGraph _formula;
        public int BaseValue => _baseValue;
        public int Cap => _cap;
        public NodeGraph Formula => _formula;

    }
}