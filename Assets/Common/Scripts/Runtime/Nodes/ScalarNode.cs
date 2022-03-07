using UnityEngine;

namespace Common.Nodes
{
    public class ScalarNode : FunctionNode
    {
        [SerializeField] protected float _value;
        public override float Value => _value;
    }
}