using Common.Nodes;
using Common.Runtime;
using UnityEngine;

namespace Common.Editor.Nodes
{
    [NodeType(typeof(ScalarNode))]
    [Title("Math", "Scalar")]
    public class ScalarNodeView : NodeView
    {
        public ScalarNodeView()
        {
            title = "Scalar";
            Node = ScriptableObject.CreateInstance<ScalarNode>();
            Output = CreateOutputPort();
        }
    }
}
