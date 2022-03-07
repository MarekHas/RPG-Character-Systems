using Common.Nodes;
using Common.Runtime;
using UnityEngine;

namespace Common.Editor.Nodes
{
    [NodeType(typeof(MultiplyNode))]
    [Title("Math", "Multiply")]
    public class MultiplyNodeView : NodeView
    {
        public MultiplyNodeView()
        {
            title = "Multiply";
            Node = ScriptableObject.CreateInstance<MultiplyNode>();
            Output = CreateOutputPort();
            Inputs.Add(CreateInputPort("A"));
            Inputs.Add(CreateInputPort("B"));
        }
    }
}