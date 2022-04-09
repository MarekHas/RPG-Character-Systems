using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Common.Nodes
{
    public class SubtractNode : IntermediateNode
    {
        [HideInInspector] public FunctionNode minuend;
        [HideInInspector] public FunctionNode subtrahend;
        public override float Value => minuend.Value - subtrahend.Value;

        public override float CalculateValue(GameObject source)
        {
            return minuend.CalculateValue(source) - subtrahend.CalculateValue(source);
        }

        public override void RemoveChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                minuend = null;
            }
            else
            {
                subtrahend = null;
            }
        }

        public override void AddChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                minuend = child;
            }
            else
            {
                subtrahend = child;
            }
        }

        public override ReadOnlyCollection<FunctionNode> children
        {
            get
            {
                List<FunctionNode> nodes = new List<FunctionNode>();
                if (minuend != null)
                {
                    nodes.Add(minuend);
                }

                if (subtrahend != null)
                {
                    nodes.Add(subtrahend);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}