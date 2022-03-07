using Common.Nodes;
using Common.Runtime;
using UnityEngine;

namespace Common.Editor.Nodes
{
    [NodeType(typeof(PowerNode))]
    [Title("Math", "Power")]
    public class PowerNodeView : NodeView
    {
        public PowerNodeView()
        {
            title = "Power";
            Node = ScriptableObject.CreateInstance<PowerNode>();
            Output = CreateOutputPort();
            Inputs.Add(CreateInputPort("A"));
            Inputs.Add(CreateInputPort("B"));
        }
    }
}
