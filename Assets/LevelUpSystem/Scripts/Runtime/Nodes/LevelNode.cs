using Common.Nodes;

namespace LevelUpSystem.Nodes
{
    public class LevelNode : FunctionNode
    {
        public ICanLevelUp levelable;
        public override float Value => levelable.Level;
    }
}