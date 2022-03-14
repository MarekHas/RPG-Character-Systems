using Common.Editor;
using Common.Editor.Nodes;
using UnityEngine;
using StatsSystem.Nodes;
using Common.Runtime;

namespace StatsSystem.Editor.Nodes
{
    [NodeType(typeof(StatNode))]
    [Title("Stat System", "Stat")]
    public class StatNodeView : NodeView
    {
        public StatNodeView()
        {
            title = "Stat";
            Node = ScriptableObject.CreateInstance<StatNode>();
            Output = CreateOutputPort();
        }
    }
}
