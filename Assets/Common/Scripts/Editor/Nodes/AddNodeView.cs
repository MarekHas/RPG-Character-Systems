using Common.Nodes;
using Common.Runtime;
using UnityEngine;

namespace Common.Editor.Nodes
{
    [NodeType(typeof(AddNode))]
    [Title("Math", "Add")]
    public class AddNodeView : NodeView
    {
        public AddNodeView()
        {
            title = "Add";
            Node = ScriptableObject.CreateInstance<AddNode>();
            Output = CreateOutputPort();
            Inputs.Add(CreateInputPort("A"));
            Inputs.Add(CreateInputPort("B"));
        }
    }
}
