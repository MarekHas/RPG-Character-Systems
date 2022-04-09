using UnityEngine;

namespace Common.Nodes
{
    public abstract class FunctionNode : AbstractNode
    {
        public abstract float Value { get; }
        public abstract float CalculateValue(GameObject source);
    }
}
