using Common.Nodes;
using Common.Runtime;

namespace Common.Editor.Nodes
{
  [NodeType(typeof(ResultNode))]
    public class ResultNodeView : NodeView
    {
        public ResultNodeView()
        {
            title = "Result";
            Inputs.Add(CreateInputPort());
        }
    }
}