using UnityEngine;

namespace Common.Nodes
{
    public class ResultNode : FunctionNode
    {
        [HideInInspector] public FunctionNode child;
        public override float Value => child.Value;

        public override float CalculateValue(GameObject source)
        {
            return child.CalculateValue(source);
        }
    }
}
