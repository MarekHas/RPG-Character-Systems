using System.Collections.ObjectModel;

namespace Common.Nodes
{
    public abstract class IntermediateNode : FunctionNode
    {
        public abstract void RemoveChild(FunctionNode child, string portName);
        public abstract void AddChild(FunctionNode child, string portName);
        public abstract ReadOnlyCollection<FunctionNode> children { get; }
    }
}
