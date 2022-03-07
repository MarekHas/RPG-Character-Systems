using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Common.Nodes;
using UnityEditor;
using UnityEngine;

namespace Common.Editor.Nodes
{
    public class NodeView : Node
    {
        public FunctionNode Node;
        public List<Port> Inputs = new List<Port>();
        public Port Output;
        public Action<NodeView> NodeSelected;

        protected Port CreateOutputPort(string portName = "", Port.Capacity capacity = Port.Capacity.Single)
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(float));
            outputPort.portName = portName;
            outputContainer.Add(outputPort);
            RefreshPorts();
            return outputPort;
        }

        protected Port CreateInputPort(string portName = "", Port.Capacity capacity = Port.Capacity.Single)
        {
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(float));
            inputPort.portName = portName;
            inputContainer.Add(inputPort);
            RefreshPorts();
            return inputPort;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            NodeSelected?.Invoke(this);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.Position.x = newPos.xMin;
            Node.Position.y = newPos.yMin;
            EditorUtility.SetDirty(Node);
        }
    }
}