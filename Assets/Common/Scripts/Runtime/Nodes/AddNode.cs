using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Common.Nodes
{
    public class AddNode : IntermediateNode
    {
        [HideInInspector] public FunctionNode addendA;
        [HideInInspector] public FunctionNode addendB;
        public override float Value => addendA.Value + addendB.Value;

        public override float CalculateValue(GameObject source)
        {
            return addendA.CalculateValue(source) + addendB.CalculateValue(source);
        }

        public override void RemoveChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                addendA = null;
            }
            else
            {
                addendB = null;
            }
        }

        public override void AddChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                addendA = child;
            }
            else
            {
                addendB = child;
            }
        }

        public override ReadOnlyCollection<FunctionNode> children
        {
            get
            {
                List<FunctionNode> nodes = new List<FunctionNode>();
                if (addendA != null)
                {
                    nodes.Add(addendA);
                }

                if (addendB != null)
                {
                    nodes.Add(addendB);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}