using Common.Editor;
using Common.Editor.Nodes;
using Common.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    [NodeType(typeof(AbilityLevelNode))]
    [Title("Ability System", "Ability", "Level")]
    public class AbilityLevelNodeView : NodeView
    {
        public AbilityLevelNodeView()
        {
            title = "Ability Level";
            Node = ScriptableObject.CreateInstance<AbilityLevelNode>();
            Output = CreateOutputPort();
        }
    }
}