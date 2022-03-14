using Common.Runtime;
using Common.Editor;
using Common.Editor.Nodes;
using LevelUpSystem.Nodes;
using UnityEngine;

namespace LevelUpSystem.Editor
{
    [NodeType(typeof(LevelNode))]
    [Title("Level System", "Level")]
    public class LevelUpNodeView : NodeView
    {
        public LevelUpNodeView()
        {
            title = "Level";
            Node = ScriptableObject.CreateInstance<LevelNode>();
            Output = CreateOutputPort();
        }
    }
}