using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Common.Nodes
{
    public class MultiplyNode : IntermediateNode
    {
        [HideInInspector] public FunctionNode factorA;
        [HideInInspector] public FunctionNode factorB;
        public override float Value => factorA.Value * factorB.Value;
        public override void RemoveChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                factorA = null;
            }
            else
            {
                factorB = null;
            }
        }

        public override void AddChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                factorA = child;
            }
            else
            {
                factorB = child;
            }
        }

        public override ReadOnlyCollection<FunctionNode> children
        {
            get
            {
                List<FunctionNode> nodes = new List<FunctionNode>();
                if (factorA != null)
                {
                    nodes.Add(factorA);
                }

                if (factorB != null)
                {
                    nodes.Add(factorB);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}