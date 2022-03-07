using Common.Nodes;
using Common.Runtime;
using UnityEngine;

namespace Common.Editor.Nodes
{
    [NodeType(typeof(SubtractNode))]
    [Title("Math", "Subtract")]
    public class SubtractNodeView : NodeView
    {
        public SubtractNodeView()
        {
            title = "Subtract";
            Node = ScriptableObject.CreateInstance<SubtractNode>();
            Output = CreateOutputPort();
            Inputs.Add(CreateInputPort("A"));
            Inputs.Add(CreateInputPort("B"));
        }
    }
}
