using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Common.Nodes
{
    public class DivideNode : IntermediateNode
    {
        [HideInInspector] public FunctionNode dividend;
        [HideInInspector] public FunctionNode divisor;
        public override float Value => dividend.Value / divisor.Value;

        public override float CalculateValue(GameObject source)
        {
            return dividend.CalculateValue(source) / divisor.CalculateValue(source);
        }

        public override void RemoveChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                dividend = null;
            }
            else
            {
                divisor = null;
            }
        }

        public override void AddChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                dividend = child;
            }
            else
            {
                divisor = child;
            }
        }

        public override ReadOnlyCollection<FunctionNode> children
        {
            get
            {
                List<FunctionNode> nodes = new List<FunctionNode>();
                if (dividend != null)
                {
                    nodes.Add(dividend);
                }

                if (divisor != null)
                {
                    nodes.Add(divisor);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}