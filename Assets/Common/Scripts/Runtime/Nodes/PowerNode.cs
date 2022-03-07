using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Common.Nodes
{
    public class PowerNode : IntermediateNode
    {
        [HideInInspector] public FunctionNode exponent;
        [HideInInspector] public FunctionNode @base;
        public override float Value => (float)Math.Pow(@base.Value, exponent.Value);
        public override void RemoveChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                @base = null;
            }
            else
            {
                exponent = null;
            }
        }

        public override void AddChild(FunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                @base = child;
            }
            else
            {
                exponent = child;
            }
        }

        public override ReadOnlyCollection<FunctionNode> children
        {
            get
            {
                List<FunctionNode> nodes = new List<FunctionNode>();
                if (@base != null)
                {
                    nodes.Add(@base);
                }

                if (exponent != null)
                {
                    nodes.Add(exponent);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}
