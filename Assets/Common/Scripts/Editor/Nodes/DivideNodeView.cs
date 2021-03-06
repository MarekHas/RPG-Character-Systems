using Common.Nodes;
using Common.Runtime;
using UnityEngine;

namespace Common.Editor.Nodes
{
    [NodeType(typeof(DivideNode))]
    [Title("Math", "Divide")]
    public class DivideNodeView : NodeView
    {
        public DivideNodeView()
        {
            title = "Divide";
            Node = ScriptableObject.CreateInstance<DivideNode>();
            Output = CreateOutputPort();
            Inputs.Add(CreateInputPort("A"));
            Inputs.Add(CreateInputPort("B"));
        }
    }
}